#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Masks
{
    public class GameButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action onClick;

        [SerializeField] private GameObject _enabledVisuals;
        [SerializeField] private GameObject _disabledVisuals;

        private bool _interactable = true;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                UpdateVisuals();
            }
        }

        private void Start()
        {
            UpdateVisuals();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;
            onClick?.Invoke();
        }

        private void UpdateVisuals()
        {
            if (_enabledVisuals != null)
            {
                _enabledVisuals.SetActive(_interactable);
            }

            if (_disabledVisuals != null)
            {
                _disabledVisuals.SetActive(!_interactable);
            }
        }
    }
}