#nullable enable

using DG.Tweening;
using UnityEngine;

namespace Masks
{
    public class BirthdayKidCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private GameButton _blowCandlesButton;
        [SerializeField] private GameButton _goBackButton;

        private bool _isVisible;

        public bool IsBlowingTheCandles { get; private set; }
        
        private void Start()
        {
            _canvasGroup.alpha = 0.0f;
            _goBackButton.Interactable = false;
            _blowCandlesButton.Interactable = false;
        }

        private void OnEnable()
        {
            _blowCandlesButton.onClick += BlowCandles;
            _goBackButton.onClick += GoBack;
        }

        private void OnDisable()
        {
            _blowCandlesButton.onClick -= BlowCandles;
            _goBackButton.onClick -= GoBack;
        }

        public void Show()
        {
            var seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(1.0f, 0.3f));
            seq.AppendCallback(() =>
            {
                _goBackButton.Interactable = true;
                _blowCandlesButton.Interactable = true;
                _isVisible = true;
            });
        }

        public void Hide()
        {
            _goBackButton.Interactable = false;
            _blowCandlesButton.Interactable = false;
            
            var seq = DOTween.Sequence();
            seq.Append(_canvasGroup.DOFade(0.0f, 0.3f));
            seq.AppendCallback(() =>
            {
                _isVisible = false;
            });
        }

        private void BlowCandles()
        {
            IsBlowingTheCandles = true;

            Hide();
            
            var partyController = FindFirstObjectByType<PartyController>();
            partyController.BlowCandles();
        }

        private void GoBack()
        {
            Hide();
        }

        public bool IsVisible() => _isVisible;
    }
}