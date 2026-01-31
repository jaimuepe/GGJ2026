#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Masks
{
    public class PieceInfo
    {
        public eMaskPieceLocation Location { get; set; }

        public MaskPieceSO PieceSO { get; set; }

        public GameObject Instance { get; set; }

        public Color? Color { get; set; }
    }

    public class Character : MonoBehaviour
    {
        [SerializeField] private MaskPiecesCatalogSO _catalogSO;
        [SerializeField] private ColorPaletteSO _colorPaletteSO;
        
        private readonly Dictionary<eMaskPieceLocation, PieceInfo> _currentPieces = new();

        private Dictionary<eMaskPieceLocation, MaskPieceAnchorPoint> _anchorsByLocation = new();

        public string PlayerName { get; private set; } = "Unknown";

        public string Message { get; private set; }
        
        public DateTime AttendanceDate { get; private set; }

        private void Awake()
        {
            var anchors = GetComponentsInChildren<MaskPieceAnchorPoint>();
            _anchorsByLocation = anchors.ToDictionary(a => a.Location, a => a);
        }

        public void Load(eMaskPieceLocation location, MaskPieceSO pieceSO)
        {
            if (_currentPieces.TryGetValue(location, out var currentPiece))
            {
                Destroy(currentPiece.Instance);
            }
            else
            {
                _currentPieces[location] = new PieceInfo { PieceSO = pieceSO, Location = location };
            }

            var anchor = _anchorsByLocation[location];

            var visual = Instantiate(pieceSO.prefab, anchor.transform);

            _currentPieces[location].Instance = visual;
        }

        public void Load(PlayerData playerData)
        {
            foreach (var (_, piece) in _currentPieces)
            {
                Destroy(piece.Instance);
            }

            _currentPieces.Clear();

            PlayerName = playerData.player_name;
            Message = playerData.message;
            AttendanceDate = playerData.created_at;

            foreach (var piece in playerData.mask_data)
            {
                var location = piece.location;
                var prefabName = piece.prefabName;
                var color = piece.color;

                var groupSO = _catalogSO.groups.FirstOrDefault(g => g.location == location);
                var pieceSO = groupSO?.pieces.FirstOrDefault(p => p.prefab.name == prefabName);

                if (pieceSO == null)
                {
                    RandomizePiece(location, color);
                }
                else
                {
                    Load(location, pieceSO);
                    SetColor(location, color);
                }
            }
        }

        public void RandomizeAllPieces() 
        {
            foreach (eMaskPieceLocation location in Enum.GetValues(typeof(eMaskPieceLocation)))
            {
                if (location == eMaskPieceLocation.Unknown) continue;
                RandomizePiece(location, null);
            }    
        }
        
        public void RandomizePiece(eMaskPieceLocation location, Color? color)
        {
            var groupSO = _catalogSO.groups
                .First(g => g.location == location);
            
            var randomPieceSO = groupSO.pieces[Random.Range(0, groupSO.pieces.Count)];
            
            Load(location, randomPieceSO);

            if (color == null)
            {
                var randomColor = _colorPaletteSO.colors[Random.Range(0, _colorPaletteSO.colors.Count)];
                SetColor(location, randomColor);
            }
            else
            {
                SetColor(location, color.Value);
            }
        }
        
        public void SetColor(eMaskPieceLocation location, Color color)
        {
            if (!_currentPieces.TryGetValue(location, out var piece))
            {
                return;
            }

            piece.Color = color;

            foreach (var r in piece.Instance.GetComponentsInChildren<Renderer>())
            {
                r.material.SetColor("_BaseColor", color);
            }
        }

        public MaskPieceSO? GetPieceAtLocation(eMaskPieceLocation location)
        {
            return _currentPieces.GetValueOrDefault(location)?.PieceSO;
        }

        public Color? GetColorAtLocation(eMaskPieceLocation location)
        {
            return _currentPieces.GetValueOrDefault(location)?.Color;
        }

        public IEnumerable<PieceInfo> GetAllPiecesInfo()
        {
            foreach (var (_, value) in _currentPieces)
            {
                if (value.Instance != null)
                {
                    yield return value;
                }
            }
        }
    }
}