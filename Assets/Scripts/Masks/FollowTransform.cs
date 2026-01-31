#nullable enable

using UnityEngine;

namespace Masks
{
    [ExecuteInEditMode]
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform _follow;
        [SerializeField] private Vector3 _offset;

        private void LateUpdate()
        {
            if (_follow == null) return;
            transform.position = _follow.position + _offset;
        }
    }
}