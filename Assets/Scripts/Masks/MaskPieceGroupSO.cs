#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Masks
{
    [CreateAssetMenu(fileName = "PieceGroup", menuName = "ScriptableObjects/PieceGroup", order = 1)]
    public class MaskPieceGroupSO : ScriptableObject
    {
        [SerializeField] public eMaskPieceLocation location;

        [SerializeField] public Vector3 offset;
        
        [SerializeField] public List<MaskPieceSO> pieces = new();

        [SerializeField] public Sprite icon;

        [SerializeField] public string title;
    }
}