#nullable enable

using UnityEngine;

namespace Masks
{
    public class PartyGuestSlot : MonoBehaviour
    {
        [SerializeField] private GameObject _stub;

        private void Awake()
        {
            if (_stub != null) Destroy(_stub);
        }

        public void Bind(PartyGuest character)
        {
            character.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}