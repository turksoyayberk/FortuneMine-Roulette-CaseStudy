using Core.Utilities;
using GameData.Appearance;
using GameData.Slot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private Image backGroundImg;
        [SerializeField] private Image completedImg;
        [SerializeField] private Image iconImg;
        [SerializeField] private Image glowImg;
        [SerializeField] private Image doneImg;
        [SerializeField] private TextMeshProUGUI amountText;

        private CanvasGroup _glowCanvasGroup;

        private SlotData _data;
        private SlotAppearanceConfig _config;
        public SlotData Data => _data;
        public bool IsCompleted { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            ValidationUtility.NotNull(backGroundImg, this, nameof(backGroundImg));
            ValidationUtility.NotNull(iconImg, this, nameof(iconImg));
            ValidationUtility.NotNull(glowImg, this, nameof(glowImg));
            ValidationUtility.NotNull(amountText, this, nameof(amountText));
            ValidationUtility.NotNull(doneImg, this, nameof(doneImg));
            ValidationUtility.NotNull(completedImg, this, nameof(completedImg));
            
            _glowCanvasGroup = glowImg.GetComponent<CanvasGroup>();
            if (_glowCanvasGroup == null)
                _glowCanvasGroup = glowImg.gameObject.AddComponent<CanvasGroup>();
        }

        public void SetData(SlotData data)
        {
            _data = data;

            iconImg.sprite = data.rewardData.icon;
            amountText.SetText($"{data.rewardData.amount.FormatNumber()}");
        }

        public void SetAppearance(SlotAppearanceConfig config)
        {
            _config = config;

            backGroundImg.sprite = _config.normalBackground;
            completedImg.sprite = _config.completedState;
            glowImg.enabled = false;
        }

        public void SetNormal()
        {
            backGroundImg.sprite = _config.normalBackground;
            glowImg.enabled = false;
            _glowCanvasGroup.alpha = 1f;
        }

        public void SetSelectedState()
        {
            backGroundImg.sprite = _config.glowSprite;
            glowImg.enabled = true;
            _glowCanvasGroup.alpha = 1f;

            UITweenUtils.PlaySelectSlotBounce(transform);
        }

        public void SetTrailState()
        {
            backGroundImg.sprite = _config.normalBackground;
            glowImg.enabled = true;
            _glowCanvasGroup.alpha = 0.5f;
        }

        public void SetRewardReceived()
        {
            doneImg.gameObject.SetActive(true);
            backGroundImg.sprite = _config.rewardReceived;
            glowImg.enabled = false;
        }

        public void SetCompleted()
        {
            iconImg.enabled = false;
            backGroundImg.enabled = false;
            amountText.gameObject.SetActive(false);
            glowImg.enabled = false;
            completedImg.enabled = true;
            IsCompleted = true;
        }
        
        public void TrySetNormal()
        {
            glowImg.enabled = false;
            if (IsCompleted) return;
            SetNormal();
        }
    }
}