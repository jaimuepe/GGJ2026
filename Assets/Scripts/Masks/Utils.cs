#nullable enable

using UnityEngine;

namespace Masks
{
    public static class Utils
    {
        public static void SetLayerRecursive(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform t in obj.transform)
            {
                SetLayerRecursive(t.gameObject, layer);
            }
        }
    }
}