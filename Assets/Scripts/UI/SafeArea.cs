using UnityEngine;

namespace UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private Vector2 padding = Vector2.zero;

        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            var safe = Screen.safeArea;

            var anchorMin = safe.position;
            var anchorMax = safe.position + safe.size;

            var screenSize = new Vector2(Screen.width, Screen.height);

            anchorMin.x /= screenSize.x;
            anchorMin.y /= screenSize.y;
            anchorMax.x /= screenSize.x;
            anchorMax.y /= screenSize.y;

            _rect.anchorMin = anchorMin;
            _rect.anchorMax = anchorMax;

            _rect.offsetMin = padding;
            _rect.offsetMax = -padding;
        }
    }
}