#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Masks.Catalog
{
    public class ColorUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _colorImage;
        
        private ColorSO _color;
        private Action<ColorUI> _callback;

        public ColorSO Color => _color;
        
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

        public void OnPointerClick(PointerEventData eventData)
        {
            _callback.Invoke(this);
        }
    }
}