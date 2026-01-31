#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Masks
{
    public class PersistentStoreObject : MonoBehaviour
    {
        public PlayerData? PlayerData { get; set; }

        public List<PlayerData> PartyGuestsData { get; set; } = new();
        
        private static PersistentStoreObject _instance;

        public static PersistentStoreObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("PlayerDataStoreObject");
                    _instance = go.AddComponent<PersistentStoreObject>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Store(Character c)
        {
            PlayerData = new PlayerData(c);
        }

        public void StorePartyGuests(List<PlayerData> guestsData)
        {
            PartyGuestsData = guestsData.ToList();
        }
    }
}