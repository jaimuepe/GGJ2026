#nullable enable

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Masks.Catalog
{
    public class DetailsUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private GameButton _confirmButton;

        [SerializeField] private GameObject _blocker;

        [SerializeField] private GameObject _loadingObj;

        [SerializeField] private Character _character;

        [SerializeField] private TMP_InputField _messageField;
        [SerializeField] private TMP_InputField _nameField;

        [SerializeField] private TextMeshProUGUI _messageCounter;
        [SerializeField] private TextMeshProUGUI _nameCounter;

        [SerializeField] private int _minMessageLength = 5;
        [SerializeField] private int _minNameLength = 3;

        public void Show()
        {
            _blocker.SetActive(true);

            var seq = DOTween.Sequence();
            seq.AppendInterval(0.1f);
            seq.Append(_content.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.AppendCallback(() => { _blocker.SetActive(false); });
        }

        private void Awake()
        {
            _content.anchoredPosition = new Vector2(0.0f, -1200.0f);
            _loadingObj.SetActive(false);
        }

        private void Start()
        {
            UpdateButton();
            UpdateMessageCounter();
            UpdateNameCounter();
        }

        private void OnEnable()
        {
            _confirmButton.onClick += Confirm;
            _messageField.onValueChanged.AddListener(OnMessageUpdated);
            _nameField.onValueChanged.AddListener(OnNameUpdated);
        }

        private void OnDisable()
        {
            _confirmButton.onClick -= Confirm;
            _messageField.onValueChanged.RemoveListener(OnMessageUpdated);
            _nameField.onValueChanged.RemoveListener(OnNameUpdated);
        }

        private Coroutine? _confirmCor;

        private void Confirm()
        {
            if (_confirmCor != null) return;
            _confirmCor = StartCoroutine(ConfirmCor());
        }

        private IEnumerator ConfirmCor()
        {
            _loadingObj.SetActive(true);

            var server = FindFirstObjectByType<Server>();

            string? errorMsg = null;
            var completed = false;

            server.Send(
                _character,
                () => completed = true,
                errCode =>
                {
                    errorMsg = errCode;
                    completed = true;
                });

            yield return new WaitUntil(() => completed);

            if (errorMsg != null)
            {
                Debug.LogError($"Error = {errorMsg}");
            }

            PersistentStoreObject.Instance.Store(_character);

            var partyGuests = new List<PlayerData>();

            completed = false;
            errorMsg = null;

            server.RetrieveOtherPlayers(
                playersData =>
                {
                    if (playersData != null)
                    {
                        partyGuests = playersData;
                    }

                    completed = true;
                },
                errCode =>
                {
                    errorMsg = errCode;
                    completed = true;
                });

            yield return new WaitUntil(() => completed);

            if (errorMsg != null)
            {
                Debug.LogError($"Error = {errorMsg}");
            }

            PersistentStoreObject.Instance.StorePartyGuests(partyGuests);

            _loadingObj.SetActive(false);

            yield return SceneManager.LoadSceneAsync("Test_Party");

            _confirmCor = null;
        }

        private void OnMessageUpdated(string text)
        {
            UpdateButton();
            UpdateMessageCounter();
        }

        private void OnNameUpdated(string text)
        {
            UpdateButton();
            UpdateNameCounter();
        }

        private void UpdateButton()
        {
            _confirmButton.Interactable = _messageField.text.Length > _minMessageLength &&
                                          _nameField.text.Length > _minNameLength;
        }

        private void UpdateMessageCounter()
        {
            _messageCounter.text = $"{_messageField.text.Length}/{_messageField.characterLimit}";
        }

        private void UpdateNameCounter()
        {
            _nameCounter.text = $"{_nameField.text.Length}/{_nameField.characterLimit}";
        }
    }
}