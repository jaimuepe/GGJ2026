#nullable enable

using DG.Tweening;
using UnityEngine;

namespace Masks
{
    public class BirthdayKidCanvas : MonoBehaviour
    {
        [SerializeField] private GameButton _blowCandlesButton;
        [SerializeField] private GameButton _goBackButton;

        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _titleContent;

        private bool _isVisible;

        public bool IsBlowingTheCandles { get; private set; }

        private void Awake()
        {
            _content.anchoredPosition = new Vector2(0.0f, -1200.0f);
            _titleContent.anchoredPosition = new Vector2(0.0f, -1200.0f);
        }

        private void Start()
        {
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
            seq.Append(_content.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.Insert(0.3f, _titleContent.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
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
            seq.Append(_titleContent.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.Insert(0.3f, _content.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.AppendCallback(() => { _isVisible = false; });
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