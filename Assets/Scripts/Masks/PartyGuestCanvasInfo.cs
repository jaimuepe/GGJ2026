#nullable enable

using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Masks
{
    public class PartyGuestCanvasInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameLabel;
        [SerializeField] private TextMeshProUGUI _dateLabel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup.alpha = 0.0f;
        }

        public void Show()
        {
            _canvasGroup.DOFade(1.0f, 0.5f);
        }

        public void Hide()
        {
            _canvasGroup.DOFade(0.0f, 0.5f);
        }

        public void SetData(Character character)
        {
            _nameLabel.text = character.PlayerName;
            _dateLabel.text = "Attended the party on " + character.AttendanceDate.ToLocalTime();
        }
    }
}