#nullable enable

using UnityEngine;

namespace Masks
{
    [CreateAssetMenu(fileName = "MaskPiece", menuName = "ScriptableObjects/MaskPiece", order = 1)]
    public class MaskPieceSO : ScriptableObject
    {
        [SerializeField] public GameObject prefab;

        [SerializeField] public Sprite sprite;
    }
}