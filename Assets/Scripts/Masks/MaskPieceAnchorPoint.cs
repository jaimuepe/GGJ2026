#nullable enable

using UnityEngine;

namespace Masks
{
    public class MaskPieceAnchorPoint : MonoBehaviour
    {
        [SerializeField] private eMaskPieceLocation _location;

        public eMaskPieceLocation Location => _location;
    }
}