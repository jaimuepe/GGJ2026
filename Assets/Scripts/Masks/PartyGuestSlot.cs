#nullable enable

using UnityEngine;

namespace Masks
{
    public class PartyGuestSlot : MonoBehaviour
    {
        [SerializeField] private GameObject _stub;
        [SerializeField] private string? _animationState;

        [Range(0.0f, 1.0f)] [SerializeField] private float _normalizedTime;

        private void Awake()
        {
            if (_stub != null) Destroy(_stub);
        }

        public void Bind(PartyGuest character)
        {
            character.transform.SetPositionAndRotation(transform.position, transform.rotation);

            if (_animationState != null)
            {
                character.PlayState(_animationState, _normalizedTime);
            }
        }
    }
}