#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Masks.Catalog
{
    public class CatalogUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _titleLabel;

        [SerializeField] private GameObject _blocker;

        [SerializeField] private MaskPiecesCatalogSO _catalogSO;
        [SerializeField] private ColorPaletteSO _colorPaletteSO;

        [SerializeField] private Transform _tabsContainer;
        [SerializeField] private CatalogTabUI _tabTemplate;

        [SerializeField] private Transform _piecesContainer;
        [SerializeField] private PieceUI _pieceTemplate;

        [SerializeField] private Transform _colorsContainer;
        [SerializeField] private ColorUI _colorTemplate;

        [SerializeField] private Character _character;

        [SerializeField] private GameButton _confirmButton;
        [SerializeField] private GameButton _randomizeButton;

        [SerializeField] private DetailsUI _detailsUI;

        [SerializeField] private TextMeshProUGUI _groupLabel;

        private readonly List<CatalogTabUI> _tabs = new();
        private readonly List<PieceUI> _pieces = new();
        private readonly List<ColorUI> _colors = new();

        private CatalogTabUI _activeTab;
        private ColorSO _activeColor;

        private void Awake()
        {
            _tabTemplate.gameObject.SetActive(false);
            _pieceTemplate.gameObject.SetActive(false);
            _colorTemplate.gameObject.SetActive(false);

            _content.anchoredPosition = new Vector2(0.0f, -1200.0f);
            _titleLabel.anchoredPosition = new Vector2(0.0f, -1200.0f);

            _blocker.SetActive(true);
        }

        private void OnEnable()
        {
            _confirmButton.onClick += Confirm;
            _randomizeButton.onClick += Randomize;
        }

        private void OnDisable()
        {
            _confirmButton.onClick -= Confirm;
            _randomizeButton.onClick -= Randomize;
        }

        private void Start()
        {
            foreach (var group in _catalogSO.groups)
            {
                var tab = Instantiate(_tabTemplate, _tabsContainer, false);
                tab.name = "tab_" + group.location;
                tab.SetData(group, OnTabClicked);

                tab.gameObject.SetActive(true);

                _tabs.Add(tab);
            }

            foreach (var color in _colorPaletteSO.colors)
            {
                var colorUI = Instantiate(_colorTemplate, _colorsContainer, false);
                colorUI.SetData(color, OnColorClicked);

                colorUI.gameObject.SetActive(true);
                _colors.Add(colorUI);
            }

            SelectTab(_tabs[0]);
            SelectPiece(_pieces[0].PieceSO);
            SelectColor(_colorPaletteSO.colors[^1]);
            
            UpdateContinueButton();
            
            var seq = DOTween.Sequence();
            seq.AppendInterval(0.1f);
            seq.Append(_content.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.Insert(0.3f, _titleLabel.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.AppendCallback(() => { _blocker.SetActive(false); });
        }

        private void OnTabClicked(CatalogTabUI tab)
        {
            if (_activeTab == tab) return;
            
            SelectTab(tab);
            UpdateContinueButton();
        }

        private void SelectTab(CatalogTabUI tabToSelect)
        {
            _activeTab = tabToSelect;

            var group = _catalogSO.groups.First(g => g.location == tabToSelect.Location);

            _groupLabel.text = group.title;
            
            foreach (var tab in _tabs)
            {
                tab.SetSelected(tab == tabToSelect);
            }

            LoadItems(tabToSelect.Location);

            var currentPiece = _character.GetPieceAtLocation(tabToSelect.Location);
            if (currentPiece != null)
            {
                SelectPiece(currentPiece);
            }
            else
            {
                foreach (var piece in _pieces)
                {
                    piece.SetSelected(false);
                }
            }
        }

        private void LoadItems(eMaskPieceLocation location)
        {
            foreach (var piece in _pieces)
            {
                Destroy(piece.gameObject);
            }

            _pieces.Clear();

            var groupSO = _catalogSO.groups
                .FirstOrDefault(g => g.location == location);

            foreach (var pieceSO in groupSO.pieces)
            {
                var piece = Instantiate(_pieceTemplate, _piecesContainer.transform, false);
                piece.name = "piece_" + pieceSO.name;
                piece.SetData(pieceSO, OnPieceClicked);

                piece.gameObject.SetActive(true);

                _pieces.Add(piece);
            }
        }

        private void OnPieceClicked(PieceUI piece)
        {
            if (_character.GetPieceAtLocation(_activeTab.Location) == piece.PieceSO)
            {
                return;
            }

            SelectPiece(piece.PieceSO, animate: true);
            UpdateContinueButton();
        }

        private void SelectPiece(MaskPieceSO pieceSO, bool animate = false)
        {
            foreach (var piece in _pieces)
            {
                piece.SetSelected(piece.PieceSO == pieceSO);
            }

            _character.Load(_activeTab.Location, pieceSO, animate);

            var appliedColor = _character.GetColorAtLocation(_activeTab.Location);
            if (appliedColor == null)
            {
                _character.SetColor(_activeTab.Location, _activeColor);
            }
            else
            {
                SelectColor(appliedColor);
            }
        }

        private void OnColorClicked(ColorUI color)
        {
            if (_character.GetColorAtLocation(_activeTab.Location) == color.Color)
            {
                return;
            }

            SelectColor(color.Color);
        }

        private void SelectColor(ColorSO colorToSelect)
        {
            foreach (var color in _colors)
            {
                color.SetSelected(color.Color == colorToSelect);
            }

            _activeColor = colorToSelect;

            _character.SetColor(_activeTab.Location, colorToSelect);
        }

        private Coroutine? _confirmCor;

        private void Confirm()
        {
            if (_confirmCor != null) return;
            _confirmCor = StartCoroutine(ConfirmCor());
        }

        private void Randomize()
        {
            _character.RandomizeAllPieces(true);

            SelectPiece(_character.GetPieceAtLocation(_activeTab.Location));
            SelectColor(_character.GetColorAtLocation(_activeTab.Location));
            
            UpdateContinueButton();
        }

        private IEnumerator ConfirmCor()
        {
            _blocker.SetActive(true);

            var seq = DOTween.Sequence();
            seq.Append(_titleLabel.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.Insert(0.3f, _content.DOAnchorPosY(-1200.0f, 0.5f).SetEase(Ease.InBack));
            seq.AppendCallback(() => { _blocker.SetActive(false); });

            yield return seq.WaitForCompletion();

            _blocker.SetActive(false);

            _detailsUI.Show();

            _confirmCor = null;
        }

        private void UpdateContinueButton()
        {
            foreach (eMaskPieceLocation loc in Enum.GetValues(typeof(eMaskPieceLocation)))
            {
                if (loc == eMaskPieceLocation.Unknown) continue;

                if (_character.GetPieceAtLocation(loc) == null)
                {
                    _confirmButton.Interactable = false;
                    return;
                }
            }
            
            _confirmButton.Interactable = true;
        }
    }
}