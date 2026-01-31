#nullable enable

using System;
using UnityEngine;

namespace Masks
{
    public class BlowCandleSlot : MonoBehaviour
    {
        [SerializeField] public bool reservedForPlayer;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
        }
#endif
    }
}