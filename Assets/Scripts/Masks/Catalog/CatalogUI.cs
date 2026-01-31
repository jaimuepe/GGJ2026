#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Masks.Catalog
{
    public class CatalogUI : MonoBehaviour
    {
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

        [SerializeField] private GameObject _loadingObj;

        private readonly List<CatalogTabUI> _tabs = new();
        private readonly List<PieceUI> _pieces = new();
        private readonly List<ColorUI> _colors = new();

        private CatalogTabUI _activeTab;
        private Color _activeColor;

        private void Awake()
        {
            _tabTemplate.gameObject.SetActive(false);
            _pieceTemplate.gameObject.SetActive(false);
            _colorTemplate.gameObject.SetActive(false);
            _loadingObj.SetActive(false);
        }

        private void OnEnable()
        {
            _confirmButton.onClick += Confirm;
        }

        private void OnDisable()
        {
            _confirmButton.onClick -= Confirm;
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

                _character.Load(tab.Location, group.pieces[0]);
                _character.SetColor(tab.Location, _colorPaletteSO.colors[0]);
            }

            foreach (var color in _colorPaletteSO.colors)
            {
                var colorUI = Instantiate(_colorTemplate, _colorsContainer, false);
                colorUI.SetData(color, OnColorClicked);

                colorUI.gameObject.SetActive(true);
                _colors.Add(colorUI);
            }

            SelectTab(_tabs[0]);
            SelectColor(_colors[0].Color);
        }

        private void OnTabClicked(CatalogTabUI tab)
        {
            SelectTab(tab);
        }

        private void SelectTab(CatalogTabUI tabToSelect)
        {
            _activeTab = tabToSelect;

            foreach (var tab in _tabs)
            {
                tab.SetSelected(tab == tabToSelect);
            }

            LoadItems(tabToSelect.Location);

            var currentPiece = _character.GetPieceAtLocation(tabToSelect.Location);
            SelectPiece(currentPiece ?? _pieces[0].PieceSO);
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
            SelectPiece(piece.PieceSO);
        }

        private void SelectPiece(MaskPieceSO pieceSO)
        {
            foreach (var piece in _pieces)
            {
                piece.SetSelected(piece.PieceSO == pieceSO);
            }

            _character.Load(_activeTab.Location, pieceSO);

            var appliedColor = _character.GetColorAtLocation(_activeTab.Location);
            if (appliedColor == null)
            {
                _character.SetColor(_activeTab.Location, _activeColor);
            }
            else
            {
                SelectColor(appliedColor.Value);
            }
        }

        private void OnColorClicked(ColorUI color)
        {
            SelectColor(color.Color);
        }

        private void SelectColor(Color colorToSelect)
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

        private IEnumerator ConfirmCor()
        {
            _loadingObj.SetActive(true);

            var server = FindFirstObjectByType<Server>();

            string? errorMsg = null;
            var completed = false;

            server.Send(
                _character,
                () => completed = true,
                errCode =>
                {
                    errorMsg = errCode;
                    completed = true;
                });

            yield return new WaitUntil(() => completed);

            if (errorMsg != null)
            {
                Debug.LogError($"Error = {errorMsg}");
            }

            PersistentStoreObject.Instance.Store(_character);

            var partyGuests = new List<PlayerData>();

            completed = false;
            errorMsg = null;
            
            server.RetrieveOtherPlayers(
                playersData =>
                {
                    if (playersData != null)
                    {
                        partyGuests = playersData;
                    }

                    completed = true;
                },
                errCode =>
                {
                    errorMsg = errCode;
                    completed = true;
                });

            yield return new WaitUntil(() => completed);

            if (errorMsg != null)
            {
                Debug.LogError($"Error = {errorMsg}");
            }

            PersistentStoreObject.Instance.StorePartyGuests(partyGuests);

            _loadingObj.SetActive(false);
            
            yield return SceneManager.LoadSceneAsync("Test_Party");
            
            _confirmCor = null;
        }
    }
}