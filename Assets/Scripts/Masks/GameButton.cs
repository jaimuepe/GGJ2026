#nullable enable

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Masks
{
    public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        public event Action onClick;

        [SerializeField] private Transform _content;

        [SerializeField] private GameObject _enabledVisuals;
        [SerializeField] private GameObject _disabledVisuals;

        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip[] _clips;

        private bool _interactable = true;

        private bool _isClickInProgress;
        private bool _isClickValid;

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
                DOTween.Kill(_content.transform);
                _content.transform.DOScale(0.9f, 0.1f);
            }

            if (_clips != null)
            {
                var clip = _clips[Random.Range(0, _clips.Length)];
                if (_audioSource == null)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                }

                _audioSource.PlayOneShot(clip);
            }

            _isClickInProgress = true;
            _isClickValid = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable) return;

            if (_content != null)
            {
                DOTween.Kill(_content.transform);
                _content.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutBounce);
            }

            _isClickInProgress = false;

            if (_isClickValid)
            {
                onClick?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isClickValid = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isClickValid = false;
        }
    }
}