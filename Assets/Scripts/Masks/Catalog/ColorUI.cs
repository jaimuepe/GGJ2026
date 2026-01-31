#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Masks.Catalog
{
    public class ColorUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _selectedObj;
        [SerializeField] private Image _colorImage;
        
        private Color _color;
        private Action<ColorUI> _callback;

        public Color Color => _color;
        
        public void SetData(Color color, Action<ColorUI> callback)
        {
            _color = color;
            _callback = callback;

            _colorImage.color = color;
        }
        
        public void SetSelected(bool b)
        {
            _selectedObj.SetActive(b);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _callback.Invoke(this);
        }
    }
}