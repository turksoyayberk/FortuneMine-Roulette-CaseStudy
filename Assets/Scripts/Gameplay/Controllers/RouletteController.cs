using System.Collections;
using System.Linq;
using Core.Events;
using Core.Services;
using Core.Utilities;
using GameData.Variations;
using Gameplay.Board;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay.Controllers
{
    public class RouletteController : MonoBehaviour
    {
        [SerializeField] private Button spinButton;

        [SerializeField] private RouletteBoard board;
        [SerializeField] private RouletteUIController rouletteUIController;
        [SerializeField] private AssetReferenceT<RouletteVariationBase> variationReference;

        private RouletteVariationBase _variation;
        private IRewardManager _rewardManager;

        private bool _isSpinning;
        private bool _isInitialized;

        private void Awake()
        {
            SetupListeners();
        }

        private void Start()
        {
            StartCoroutine(InitializeRoulette());
            ResolveServices();
            ValidateReferences();
        }

        private void SetupListeners()
        {
            if (spinButton != null)
                spinButton.onClick.AddListener(StartSpin);
        }

        private void ResolveServices()
        {
            _rewardManager = ServiceLocator.Resolve<IRewardManager>();
        }

        private void ValidateReferences()
        {
            ValidationUtility.NotNull(_rewardManager, this, nameof(_rewardManager));
            ValidationUtility.NotNull(board, this, nameof(board));
            ValidationUtility.NotNull(rouletteUIController, this, nameof(rouletteUIController));
        }

        private IEnumerator InitializeRoulette()
        {
            var handle = variationReference.LoadAssetAsync();
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("[Roulette] Variation failed to load.");
                yield break;
            }

            _variation = handle.Result;

            board.Initialize(_variation);
            rouletteUIController.Initialize(_variation);

            spinButton.gameObject.SetActive(true);

            _isInitialized = true;
        }

        private bool CanSpin()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[Roulette] Not initialized yet.");
                return false;
            }

            return !_isSpinning;
        }

        public void StartSpin()
        {
            if (!CanSpin()) return;

            _isSpinning = true;
            spinButton.gameObject.SetActive(false);

            GameStateManager.SetState(GameState.Spinning);

            StartCoroutine(_variation.PlaySpin(
                board.Slots.ToList(),
                OnSpinComplete));
        }

        private void OnSpinComplete(int index)
        {
            StartCoroutine(HandleSpinEnd(index));
        }

        private IEnumerator HandleSpinEnd(int index)
        {
            var slot = board.Slots[index];

            yield return RewardSequence(slot);

            if (AllSlotsCompleted())
            {
                ResetGame();
            }
        }

        private IEnumerator RewardSequence(SlotView slot)
        {
            slot.SetRewardReceived();

            EventBus.Publish(new UIEvents.RewardWonEvent(slot.Data.rewardData));

            yield return new WaitForSeconds(0.8f);

            _rewardManager.PlayRewardSequence(
                slot.Data.rewardData.icon,
                slot.Data.rewardData.amount
            );

            while (GameStateManager.CurrentState != GameState.Idle)
                yield return null;

            slot.SetCompleted();
            
            spinButton.gameObject.SetActive(true);
            _isSpinning = false;
        }

        private bool AllSlotsCompleted()
        {
            return board.Slots.All(s => s.IsCompleted);
        }

        private void ResetGame()
        {
            StartCoroutine(ResetRoutine());
        }

        private IEnumerator ResetRoutine()
        {
            spinButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            SceneLoader.LoadScene(SceneType.GameScene);
        }
    }
}