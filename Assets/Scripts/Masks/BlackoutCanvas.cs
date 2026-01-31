#nullable enable

using DG.Tweening;
using UnityEngine;

namespace Masks
{
    public class BlackoutCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup.alpha = 0.0f;
        }

        public Sequence Show()
        {
            var seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(1.0f, 1.0f));
            return seq;
        }

        public Sequence Hide()
        {
            var seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(0.0f, 1.0f));
            return seq;
        }
    }
}