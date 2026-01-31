#nullable enable

using UnityEngine;

namespace Masks
{
    [CreateAssetMenu(fileName = "Color", menuName = "ScriptableObjects/Color", order = 1)]
    public class ColorSO : ScriptableObject
    {
        [SerializeField]
        public Color color;

        [SerializeField]
        public Sprite preview_active;
        
        [SerializeField]
        public Sprite preview_inactive;
    }
}