#nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Masks.Catalog
{
    public class PieceUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _selectedObj;
        [SerializeField] private TextMeshProUGUI _placeholderText;
        
        private MaskPieceSO _pieceSO;
        private Action<PieceUI> _callback;

        public MaskPieceSO PieceSO => _pieceSO;

        public void SetData(MaskPieceSO pieceSO, Action<PieceUI> callback)
        {
            _pieceSO = pieceSO;
            _callback = callback;

            _placeholderText.text = pieceSO.name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _callback.Invoke(this);
        }

        public void SetSelected(bool b)
        {
            _selectedObj.SetActive(b);
        }
    }
}