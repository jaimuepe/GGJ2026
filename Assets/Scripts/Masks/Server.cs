#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Masks
{
    public class Server : MonoBehaviour
    {
        private static Server _instance;

        public static Server Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("Server");
                    _instance = go.AddComponent<Server>();
                }

                return _instance!;
            }
        }

        [SerializeField] public bool _dbgTargetLocal = false;

        private const string prod_url = "https://ggj2026-server-sbre.onrender.com/users";

        private Coroutine? _sendCor;
        private Coroutine? _retrieveCor;

        public int TotalPlayers { get; private set; }

        public event Action<int> OnTotalPlayersUpdated;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(FetchNumberOfPlayersCor());
        }

        public void Send(Character character, Action successCallback, Action<string> failureCallback)
        {
            if (_sendCor != null) return;

            _sendCor = StartCoroutine(SendCor(character, successCallback, failureCallback));
        }

        private IEnumerator SendCor(
            Character character,
            Action successCallback,
            Action<string> failureCallback)
        {
            var playerData = new PlayerData(character);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ColorJsonConverter());

            var json = JsonConvert.SerializeObject(playerData, settings);

            var url = GetUrl();

            using (var www = UnityWebRequest.Post(
                       url,
                       json,
                       "application/json"))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Server Error = {www.error}");
                    failureCallback.Invoke(www.error);
                }
                else
                {
                    successCallback.Invoke();
                }
            }

            _sendCor = null;
        }

        public void RetrieveOtherPlayers(Action<List<PlayerData>> successCallback, Action<string> failureCallback)
        {
            if (_retrieveCor != null) return;
            _retrieveCor = StartCoroutine(RetrieverOtherPlayersCor(successCallback, failureCallback));
        }

        private IEnumerator RetrieverOtherPlayersCor(
            Action<List<PlayerData>> successCallback,
            Action<string> failureCallback)
        {
            string url = GetUrl();

            using (var www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Server Error = {www.error}");
                    failureCallback.Invoke(www.error);
                }
                else
                {
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new ColorJsonConverter());

                    var playersData =
                        JsonConvert.DeserializeObject<List<PlayerData>>(www.downloadHandler.text, settings);

                    successCallback.Invoke(playersData);
                }
            }

            _retrieveCor = null;
        }

        private string GetUrl() => prod_url;

        private IEnumerator FetchNumberOfPlayersCor()
        {
            const string url = prod_url + "/count";

            while (true)
            {
                using (var www = UnityWebRequest.Get(url))
                {
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        // silently fail
                    }
                    else
                    {
                        var settings = new JsonSerializerSettings();
                        settings.Converters.Add(new ColorJsonConverter());

                        if (int.TryParse(www.downloadHandler.text, out var number))
                        {
                            TotalPlayers = number;
                            OnTotalPlayersUpdated?.Invoke(TotalPlayers);
                        }
                    }
                }

                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}