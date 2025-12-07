using DG.Tweening;
using System;
using UnityEngine;

namespace Core.Utilities
{
    public static class UITweenUtils
    {
        public static void PlaySelectSlotBounce(Transform target)
        {
            if (target == null) return;

            var downScale = 0.9f;
            var upScale = 1.05f;

            target.localScale = Vector3.one * downScale;

            target.DOScale(upScale, 0.12f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    target.DOScale(1f, 0.10f)
                        .SetEase(Ease.OutQuad);
                });
        }

        public static void AnimateRewardToWallet(
            RectTransform reward,
            RectTransform walletTarget,
            System.Action onComplete)
        {
            if (reward == null || walletTarget == null)
            {
                Debug.LogWarning("[UITweenUtils] RewardToWallet: null reference");
                onComplete?.Invoke();
                return;
            }

            reward.DOMove(walletTarget.position, 0.6f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    PlayWalletBounce(walletTarget);
                    onComplete?.Invoke();
                });
        }

        private static void PlayWalletBounce(RectTransform wallet)
        {
            if (wallet == null) return;

            var seq = DOTween.Sequence();

            seq.Append(wallet.DOScale(0.9f, 0.10f).SetEase(Ease.OutQuad));
            seq.Append(wallet.DOScale(1.15f, 0.15f).SetEase(Ease.OutBack));
            seq.Append(wallet.DOScale(1f, 0.12f).SetEase(Ease.OutQuad));

            seq.Play();
        }

        public static void PlayEntranceAnimation(
            RectTransform target,
            float duration = 0.4f,
            float delay = 0f)
        {
            if (target == null) return;

            var enterOffset = Screen.width * 1.5f;

            var originalPos = target.anchoredPosition;
            
            target.anchoredPosition = new Vector2(
                originalPos.x + enterOffset,
                originalPos.y
            );

            target.DOAnchorPosX(originalPos.x, duration)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);
        }

        private static Sequence _popupSequence;

        private static readonly Vector2 PopupStartPosition = new Vector2(1100f, 0f);
        private static readonly Vector2 PopupCenterPosition = Vector2.zero;
        private static readonly Vector2 PopupExitPosition = new Vector2(-1100f, 0f);

        private const float EnterDuration = 0.5f;
        private const float ExitDuration = 0.5f;
        private const float HoldDuration = 1.75f;

        public static void PlayRewardPopupAnimation(RectTransform rt,Action onComplete)
        {
            _popupSequence?.Kill();

            _popupSequence = DOTween.Sequence();

            _popupSequence.Append(
                rt.DOAnchorPos(PopupCenterPosition, EnterDuration)
                    .SetEase(Ease.OutBack)
            );

            _popupSequence.AppendInterval(HoldDuration);

            _popupSequence.Append(
                rt.DOAnchorPos(PopupExitPosition, ExitDuration)
                    .SetEase(Ease.InBack)
            );

            _popupSequence.OnComplete(() =>
            {
                onComplete?.Invoke();
                rt.gameObject.SetActive(false);
                rt.anchoredPosition = PopupStartPosition;
            });
        }
    }
}