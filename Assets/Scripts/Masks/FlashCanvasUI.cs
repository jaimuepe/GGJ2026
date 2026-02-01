#nullable enable

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Masks
{
    public class FlashCanvasUI : MonoBehaviour
    {
        private static FlashCanvasUI? _instance;

        public static FlashCanvasUI Instance => _instance;

        [SerializeField] private Image _flashImage;

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
            var c = _flashImage.color;
            _flashImage.color = new Color(c.r, c.g, c.b, 0.0f);
        }

        public IEnumerator ShowCor()
        {
            yield return _flashImage.DOFade(1.0f, 0.5f).WaitForCompletion();
        }

        public IEnumerator HideCor()
        {
            yield return _flashImage.DOFade(0.0f, 3.0f).WaitForCompletion();
        }

        public void LoadSceneWithTransition(string sceneName)
        {
            StartCoroutine(LoadSceneWithTransitionCor(sceneName));
        }

        private IEnumerator LoadSceneWithTransitionCor(string sceneName)
        {
            yield return ShowCor();

            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;

            yield return HideCor();
        }
    }
}