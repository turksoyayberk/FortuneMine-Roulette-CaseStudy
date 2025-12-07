using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float ScaleDownValue = 0.9f;
        private const float AnimationDuration = 0.1f;

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(ScaleDownValue, AnimationDuration).SetEase(Ease.OutQuad);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(1f, AnimationDuration).SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}