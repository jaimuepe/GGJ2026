#nullable enable

using UnityEngine;

namespace Masks
{
    public class PartyGuestSlot : MonoBehaviour
    {
        [SerializeField] private PartyGuest _stub;
        [SerializeField] private string? _animationState;

        [Range(0.0f, 1.0f)] [SerializeField] private float _normalizedTime;

        public PartyGuest Guest => _stub;

        public void PlayDefaultState()
        {
            if (_animationState != null)
            {
                _stub.PlayState(_animationState, _normalizedTime);
            }
        }
    }
}