#nullable enable

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Masks.Catalog
{
    public class ColorUI : MonoBehaviour
    {
        [SerializeField] private Image _colorImage;

        [SerializeField] private GameButton _button;

        private ColorSO _color;
        private Action<ColorUI>? _callback;

        public ColorSO Color => _color;

        private void OnEnable()
        {
            _button.onClick += OnClick;
        }

        private void OnDisable()
        {
            _button.onClick -= OnClick;
        }

        public void SetData(ColorSO color, Action<ColorUI> callback)
        {
            _color = color;
            _callback = callback;

            _colorImage.sprite = color.preview_inactive;
        }

        public void SetSelected(bool b)
        {
            _colorImage.sprite = b ? _color.preview_active : _color.preview_inactive;
        }

        private void OnClick()
        {
            _callback?.Invoke(this);
        }
    }
}