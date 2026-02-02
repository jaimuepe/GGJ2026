#nullable enable

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Masks
{
    public class PolaroidUI : MonoBehaviour
    {
        [SerializeField] private GameButton _closeButton;
        [SerializeField] private RawImage _polaroidView;

        [SerializeField] private TextMeshProUGUI _totalFriendsLabel;
        [SerializeField] private TextMeshProUGUI _message;

        public void PlayAnimations()
        {
            _polaroidView.DOFade(1.0f, 5.0f);
        }

        private void Awake()
        {
            var c = _polaroidView.color;
            _polaroidView.color = new Color(c.r, c.g, c.b, 0.0f);

#if UNITY_WEBGL
            _closeButton.gameObject.SetActive(false);
#endif
        }

        private void Start()
        {
            RefreshPlayersCount();
            _message.text = PersistentStoreObject.Instance.PlayerData?.message ?? "";
        }

        private void OnEnable()
        {
            _closeButton.onClick += CloseGame;
            Server.Instance.OnTotalPlayersUpdated += OnPlayersUpdated;
        }

        private void OnDisable()
        {
            _closeButton.onClick -= CloseGame;
            Server.Instance.OnTotalPlayersUpdated -= OnPlayersUpdated;
        }

        private void CloseGame()
        {
            Application.Quit();
        }

        private void OnPlayersUpdated(int _)
        {
            RefreshPlayersCount();
        }

        private void RefreshPlayersCount()
        {
            var players = Server.Instance.TotalPlayers + 1;

            _totalFriendsLabel.text =
                $@"You and {players} more friends attended the party.\n Thank you for coming!\n-Manolito";
        }
    }
}