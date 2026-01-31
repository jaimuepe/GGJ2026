#nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Masks.Catalog
{
    public class CatalogTabUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _selectedObj;
        [SerializeField] private TextMeshProUGUI _placeholderText;

        private MaskPieceGroupSO _groupDataSO;

        private Action<CatalogTabUI> _onTabClicked;

        public eMaskPieceLocation Location => _groupDataSO.location;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onTabClicked.Invoke(this);
        }

        public void SetData(MaskPieceGroupSO groupDataSO, Action<CatalogTabUI> onTabClicked)
        {
            _groupDataSO = groupDataSO;
            _onTabClicked = onTabClicked;

            if (_placeholderText != null)
            {
                _placeholderText.text = _groupDataSO.location.ToString();
            }
        }

        public void SetSelected(bool b)
        {
            _selectedObj.SetActive(b);
        }
    }
}