#nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Masks.Catalog
{
    public class CatalogTabUI : MonoBehaviour
    {
        [SerializeField] private GameObject _selectedObj;

        [SerializeField] private GameButton _button;

        [SerializeField] private Image[] _images;

        private MaskPieceGroupSO _groupDataSO;

        private Action<CatalogTabUI>? _onTabClicked;

        public eMaskPieceLocation Location => _groupDataSO.location;

        private void OnEnable()
        {
            _button.onClick += OnClick;
        }

        private void OnDisable()
        {
            _button.onClick -= OnClick;
        }

        public void SetData(MaskPieceGroupSO groupDataSO, Action<CatalogTabUI> onTabClicked)
        {
            _groupDataSO = groupDataSO;
            _onTabClicked = onTabClicked;

            foreach (var img in _images)
            {
                img.sprite = groupDataSO.icon;
            }
        }

        public void SetSelected(bool b)
        {
            _selectedObj.SetActive(b);
        }

        private void OnClick()
        {
            _onTabClicked?.Invoke(this);
        }
    }
}