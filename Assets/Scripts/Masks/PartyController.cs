#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masks.Catalog;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Masks
{
    public class PartyController : MonoBehaviour
    {
        [SerializeField] private Character _playableCharacter;

        [SerializeField] private PartyGuest _partyGuestPrefab;

        [SerializeField] private PartyGuest _birthdayBoy;

        [SerializeField] private CinemachineCamera _cinematicStartCamera;
        [SerializeField] private CinemachineCamera _cinematicEndCamera;

        [SerializeField] private GameObject _playerVisualsObject;

        [SerializeField] private RenderTexture _polaroidRt;
        
        private readonly List<PartyGuest> _guests = new();

        private List<PartyGuestSlot> _partyGuestSlots;

        private CinemachineBrain _brain;
        
        private void Start()
        {
            _brain = FindFirstObjectByType<CinemachineBrain>();
            
            var persistenceStore = PersistentStoreObject.Instance;

            var storedData = persistenceStore.PlayerData;
            if (storedData == null)
            {
                Debug.LogError("Could not find any stored data, randomizing");
                _playableCharacter.RandomizeAllPieces(false);
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
                _guests.Add(guest);

                _partyGuestSlots[i].Bind(guest);
            }

            // if we retrieve less characters than slots we will randomize the rest
            for (var i = data.Count; i < _partyGuestSlots.Count; i++)
            {
                var guest = Instantiate(_partyGuestPrefab);
                var guestCharacter = guest.GetComponentInChildren<Character>();

                guestCharacter.RandomizeAllPieces(false);
                _guests.Add(guest);

                _partyGuestSlots[i].Bind(guest);
            }

            _birthdayBoy.Character.RandomizeAllPieces(false);
        }

        private Coroutine? _blowCandlesCor;

        public void BlowCandles()
        {
            if (_blowCandlesCor != null) return;
            _blowCandlesCor = StartCoroutine(BlowCandlesCor());
        }

        private IEnumerator BlowCandlesCor()
        {
            yield return SceneTransitionUI.Instance.ShowCor();

            yield return new WaitForSeconds(1.0f);

            Utils.SetLayerRecursive(_playerVisualsObject, LayerMask.NameToLayer("Default"));
            
            _birthdayBoy.PlayState("Idle", Random.value);
            _playableCharacter.PlayState("Idle", Random.value);

            foreach (var guest in _guests)
            {
                guest.PlayState("Idle", Random.value);
            }

            var slots = FindObjectsByType<BlowCandleSlot>(FindObjectsSortMode.None);
            var playerSlot = slots.FirstOrDefault(s => s.reservedForPlayer);

            var restOfSlots = slots
                .Where(s => !s.reservedForPlayer)
                .OrderBy(s => Random.value)
                .ToList();

            if (playerSlot != null)
            {
                _playableCharacter.transform.SetPositionAndRotation(
                    playerSlot.transform.position,
                    playerSlot.transform.rotation);
            }
            else
            {
                var slot = restOfSlots[0];
                restOfSlots.RemoveAt(0);

                _playableCharacter.transform.SetPositionAndRotation(
                    slot.transform.position,
                    slot.transform.rotation);
            }

            var n = Mathf.Min(_guests.Count, restOfSlots.Count);

            for (var i = 0; i < n; i++)
            {
                var guest = _guests[i];
                var slot = restOfSlots[i];

                guest.transform.SetPositionAndRotation(
                    slot.transform.position,
                    slot.transform.rotation);
            }

            for (var i = _guests.Count - 1; i >= n; i--)
            {
                Destroy(_guests[i].gameObject);
                _guests.RemoveAt(0);
            }

            _cinematicStartCamera.Priority = new PrioritySettings
            {
                Value = 1000,
            };

            yield return new WaitForSeconds(1.0f);
            
            _cinematicEndCamera.Priority = new PrioritySettings
            {
                Value = 1001,
            };
            
            yield return SceneTransitionUI.Instance.HideCor();

            if (_brain.IsBlending)
            {
                yield return new WaitWhile(() => _brain.IsBlending);
            }
            
            yield return new WaitForSeconds(1.0f);

            _playableCharacter.PlayState("Clap",  Random.value);

            for (var i = 0; i < _guests.Count; i++)
            {
                _guests[i].PlayState("Clap",  Random.value);
            }

            yield return new WaitForSeconds(5.0f);

            DontDestroyOnLoad(gameObject);

            var mainCamera = Camera.main;
            var oldTexture = mainCamera.targetTexture;
            mainCamera.targetTexture = _polaroidRt;
            mainCamera.Render();
            mainCamera.targetTexture = oldTexture;
            
            yield return FlashCanvasUI.Instance.ShowCor();
            
            yield return SceneManager.LoadSceneAsync("Polaroid");
            
            yield return FlashCanvasUI.Instance.HideCor();

            var polaroidUI = FindFirstObjectByType<PolaroidUI>();
            polaroidUI.PlayAnimations();
            
            Destroy(gameObject);
        }
    }
}