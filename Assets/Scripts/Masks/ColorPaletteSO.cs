#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Masks
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
    public class ColorPaletteSO : ScriptableObject
    {
        public List<Color> colors = new();
    }
}