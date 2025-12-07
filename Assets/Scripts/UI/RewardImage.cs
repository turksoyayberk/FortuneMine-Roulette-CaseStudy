using System;
using Core.Pooling;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RewardImage : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Image _image;

        private Vector3 _localScale;

        private ObjectPool<RewardImage> _pool;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();

            _localScale = transform.localScale;
        }

        public void SetSprite(Sprite sprite)
        {
            if (_image.sprite == sprite) return;
            _image.sprite = sprite;
        }

        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void SetPool(ObjectPool<RewardImage> pool)
        {
            _pool = pool;
        }

        public void Release()
        {
            transform.localScale = _localScale;
            _pool.Release(this);
        }
    }
}