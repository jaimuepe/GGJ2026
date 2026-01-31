#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Masks
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] public string player_name;

        [SerializeField] public string message;

        [SerializeField] public List<PieceData> mask_data;

        [SerializeField] public DateTime created_at;

        public PlayerData()
        {
        }

        public PlayerData(Character c)
        {
            player_name = c.PlayerName;
            message = c.Message;

            mask_data = c.GetAllPiecesInfo()
                .Select(i => new PieceData
                {
                    prefabName = i.PieceSO.prefab.name,
                    color = i.Color?.name ?? "white",
                    location = i.Location,
                })
                .ToList();
        }
    }

    [Serializable]
    public class PieceData
    {
        [SerializeField] public eMaskPieceLocation location;

        [SerializeField] public string prefabName;

        [SerializeField] public string color;
    }
}