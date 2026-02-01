#nullable enable

using UnityEngine;

namespace Masks
{
    public class ChangeFacesReceiver : MonoBehaviour
    {
        public void ChangeFace(string face)
        {
            var character = GetComponentInParent<Character>();
            character.ChangeFace(face);
        }
    }
}