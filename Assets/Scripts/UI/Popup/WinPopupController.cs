using Core.Events;
using Core.Services;
using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class WinPopupController : MonoBehaviour
    {
        [SerializeField] private RectTransform winPopup;

        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI rewardText;

        private const int RewardFormatThreshold = 10_000;

        private void Awake()
        {
            winPopup.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<UIEvents.RewardWonEvent>(OnRewardWon);
        }

        private void OnDisable()
        {
            EventBus.UnSubscribe<UIEvents.RewardWonEvent>(OnRewardWon);
        }

        private void OnRewardWon(UIEvents.RewardWonEvent e)
        {
            Show(e.Reward.icon, e.Reward.amount);
        }

        private void Show(Sprite icon, int amount)
        {
            GameStateManager.SetState(GameState.Popup);
            winPopup.gameObject.SetActive(true);

            iconImage.sprite = icon;
            rewardText.SetText($"{amount.FormatNumber(RewardFormatThreshold)}");

            UITweenUtils.PlayRewardPopupAnimation(winPopup, OnComplete);
        }

        private void OnComplete()
        {
            GameStateManager.SetState(GameState.Rewarding);
        }
    }
}