#nullable enable

using System;
using UnityEngine;

namespace Masks
{
    public class BlowCandleSlot : MonoBehaviour
    {
        [SerializeField] public bool reservedForPlayer;
        [SerializeField] public bool reservedForBirthdayKid;
        [SerializeField] public string cheerAnimation = "Clap";

        private void Awake()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
        }
#endif
    }
}