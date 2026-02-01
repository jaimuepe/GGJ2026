#nullable enable

using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Masks
{
    public class BubbleMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageLabel;

        private Transform _anchor;

        private Camera _mainCamera;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        public void SetData(Transform worldAnchor, string msg)
        {
            _anchor = worldAnchor;
            _messageLabel.text = msg;
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }

        public void Show(float duration = 1.0f)
        {
            var seq = DOTween.Sequence();
            seq.SetTarget(this);
            seq.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
            seq.AppendInterval(3.0f);
            seq.AppendCallback(Hide);
        }

        private void Hide()
        {
            var seq = DOTween.Sequence();
            seq.SetTarget(this);
            seq.Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.InBack));
            seq.OnComplete(() => Destroy(gameObject));
        }

        private void LateUpdate()
        {
            if (_anchor == null) return;

            _mainCamera ??= Camera.main;
            var screenPos = _mainCamera.WorldToScreenPoint(_anchor.position);
            transform.position = screenPos;
        }
    }
}