#nullable enable

using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Masks
{
    public class PartyGuestCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _dateLabel;

        [SerializeField] private GameButton _backButton;

        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _titleContent;

        private bool _isVisible;

        private void Awake()
        {
            _content.anchoredPosition = new Vector2(0.0f, -1200.0f);
            _titleContent.anchoredPosition = new Vector2(0.0f, -1200.0f);
        }

        private void Start()
        {
            _backButton.Interactable = false;
        }

        private void OnEnable()
        {
            _backButton.onClick += GoBack;
        }

        private void OnDisable()
        {
            _backButton.onClick -= GoBack;
        }

        public void Show()
        {
            var seq = DOTween.Sequence();
            seq.Append(_content.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.Insert(0.3f, _titleContent.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.AppendCallback(() =>
            {
                _backButton.Interactable = true;
                _isVisible = true;
            });
        }

        public void Hide()
        {
            _backButton.Interactable = false;

            var seq = DOTween.Sequence();
            seq.Append(_titleContent.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.Insert(0.3f, _content.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.AppendCallback(() => { _isVisible = false; });
        }

        public void SetData(Character character)
        {
            _nameLabel.text = character.PlayerName;
            _dateLabel.text = "Attended the party on " + character.AttendanceDate.ToShortDateString();
        }

        public bool IsVisible() => _isVisible;

        private void GoBack()
        {
            Hide();
        }
    }
}