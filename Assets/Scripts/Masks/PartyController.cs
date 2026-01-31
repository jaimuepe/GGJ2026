#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Masks
{
    public class PartyController : MonoBehaviour
    {
        [SerializeField] private Character _playableCharacter;
        
        [SerializeField] private PartyGuest _partyGuestPrefab;

        private List<PartyGuestSlot> _partyGuestSlots;
        
        private void Start()
        {
            var persistenceStore = PersistentStoreObject.Instance;
            
            var storedData = persistenceStore.PlayerData;
            if (storedData == null)
            {
                Debug.LogError("Could not find any stored data, randomizing");
                _playableCharacter.RandomizeAllPieces();
            }
            else
            {
                _playableCharacter.Load(storedData);
            }

            _partyGuestSlots = FindObjectsByType<PartyGuestSlot>(FindObjectsSortMode.None).ToList();

            var data = persistenceStore.PartyGuestsData;

            for (var i = 0; i < data.Count && i < _partyGuestSlots.Count; i++)
            {
                var guest = Instantiate(_partyGuestPrefab);
                guest.name = data[i].player_name;
                
                var guestCharacter = guest.GetComponentInChildren<Character>();
                
                guestCharacter.Load(data[i]);
                
                _partyGuestSlots[i].Bind(guest);
            }

            // if we retrieve less characters than slots we will randomize the rest
            for (var i = data.Count; i < _partyGuestSlots.Count; i++)
            {
                var guest = Instantiate(_partyGuestPrefab);
                var guestCharacter = guest.GetComponentInChildren<Character>();
                
                guestCharacter.RandomizeAllPieces();
                
                _partyGuestSlots[i].Bind(guest);
            }
        }
    }
}