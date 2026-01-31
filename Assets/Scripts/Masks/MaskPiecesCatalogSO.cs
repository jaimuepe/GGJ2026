#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Masks
{
    [CreateAssetMenu(fileName = "PiecesCatalog", menuName = "ScriptableObjects/PiecesCatalog", order = 1)]
    public class MaskPiecesCatalogSO : ScriptableObject
    {
        public List<MaskPieceGroupSO> groups = new();
    }
}