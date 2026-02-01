#nullable enable

using System;
using DG.Tweening;
using UnityEngine;

namespace Masks
{
    [ExecuteInEditMode]
    public class InteractionBubbleUI : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;

        private Camera _mainCamera;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        private void LateUpdate()
        {
            if (_anchor == null) return;

            _mainCamera ??= Camera.main;
            var screenPos = _mainCamera.WorldToScreenPoint(_anchor.position);
            transform.position = screenPos;
        }

        public void Show()
        {
            transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        }
    }
}