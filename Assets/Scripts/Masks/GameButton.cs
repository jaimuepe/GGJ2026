#nullable enable

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Masks
{
    public class GameButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action onClick;

        [SerializeField] private Transform _content;
        
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
            if (_content == null)
            {
                Debug.LogError($"GameButton = {name} does not have _content");
            }
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) return;

            if (_content != null)
            {
                _content.transform.DOScale(1.1f, 0.15f);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable) return;

            if (_content != null)
            {
                _content.transform.DOScale(1.0f, 0.15f);
            }
            
            onClick?.Invoke();
        }
    }
}