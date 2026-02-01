#nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Masks.Catalog
{
    public class PieceUI : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedObj;
        [SerializeField] private TextMeshProUGUI _placeholderText;

        [SerializeField] private Image _icon;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _deelectedColor;

        [SerializeField] private GameButton _button;
        
        private MaskPieceSO _pieceSO;
        private Action<PieceUI>? _callback;

        public MaskPieceSO PieceSO => _pieceSO;

        private void OnEnable()
        {
            _button.onClick += OnClick;
        }

        private void OnDisable()
        {
            _button.onClick -= OnClick;
        }

        public void SetData(MaskPieceSO pieceSO, Action<PieceUI> callback)
        {
            _pieceSO = pieceSO;
            _callback = callback;

            _icon.sprite = pieceSO.sprite;
            
            UpdateColor(false);
        }

        public void SetSelected(bool b)
        {
            _selectedObj.SetActive(b);
            UpdateColor(b);
        }

        private void UpdateColor(bool selected)
        {
            _icon.color = selected ? _selectedColor : _deelectedColor;
        }

        private void OnClick()
        {
            _callback?.Invoke(this);
        }
    }
}