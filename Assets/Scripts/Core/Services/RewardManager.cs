using System.Collections;
using System.Collections.Generic;
using Core.Pooling;
using Core.Utilities;
using DG.Tweening;
using UI;
using UnityEngine;

namespace Core.Services
{
    public class RewardManager : MonoBehaviour, IRewardManager
    {
        [SerializeField] private RectTransform spawnArea;
        [SerializeField] private RectTransform walletTarget;

        private IRewardImagePool _pool;
        private IWalletService _wallet;

        private const float SpawnInterval = 0.05f;
        private const float DelayBeforeFlight = 0.25f;
        private const float FlightInterval = 0.08f;
        private const float SpawnMoveDuration = 0.35f;

        private void Awake()
        {
            ServiceLocator.Register<IRewardManager>(this);
        }

        private void Start()
        {
            ResolveServices();
        }

        private void ResolveServices()
        {
            _pool = ServiceLocator.Resolve<IRewardImagePool>();
            _wallet = ServiceLocator.Resolve<IWalletService>();

            ValidationUtility.NotNull(_pool, this, nameof(_pool));
            ValidationUtility.NotNull(_wallet, this, nameof(_wallet));
        }

        public void PlayRewardSequence(Sprite rewardSprite, int amount, int count = 10)
        {
            if (rewardSprite == null)
            {
                Debug.LogWarning("[RewardManager] Reward sprite is NULL.");
                return;
            }

            StartCoroutine(SpawnAndFlyRewards(rewardSprite, amount, count));
        }

        private IEnumerator SpawnAndFlyRewards(Sprite sprite, int amount, int count)
        {
            while (GameStateManager.CurrentState != GameState.Rewarding)
                yield return null;

            var spawnedImages = new List<RewardImage>(count);

            for (var i = 0; i < count; i++)
            {
                var img = SpawnSingleReward(sprite);
                spawnedImages.Add(img);
                yield return new WaitForSeconds(SpawnInterval);
            }

            yield return new WaitForSeconds(DelayBeforeFlight);

            foreach (var img in spawnedImages)
            {
                FlyToWallet(img);
                yield return new WaitForSeconds(FlightInterval);
            }

            GameStateManager.SetState(GameState.Idle);
            _wallet.Add(amount);
        }

        private RewardImage SpawnSingleReward(Sprite sprite)
        {
            var img = _pool.GetRewardImage(Vector3.zero);
            var rt = img.GetRectTransform();

            img.SetSprite(sprite);

            // Reset transform
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            var randomPos = GetRandomInside(spawnArea);

            rt.DOAnchorPos(randomPos, SpawnMoveDuration)
                .SetEase(Ease.OutQuad);

            return img;
        }

        private void FlyToWallet(RewardImage img)
        {
            var rt = img.GetRectTransform();

            UITweenUtils.AnimateRewardToWallet(
                rt,
                walletTarget,
                () => img.Release()
            );
        }

        private Vector2 GetRandomInside(RectTransform area)
        {
            var halfW = area.rect.width * 2.5f;
            var halfH = area.rect.height * 2.5f;

            return new Vector2(
                Random.Range(-halfW, halfW),
                Random.Range(-halfH, halfH)
            );
        }
    }
}