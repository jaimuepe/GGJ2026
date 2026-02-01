#nullable enable

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Masks.Catalog
{
    public class SceneTransitionUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _circle;

        private static SceneTransitionUI? _instance;

        public static SceneTransitionUI Instance => _instance;

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
            _circle.sizeDelta = Vector2.one * 5000.0f;
        }

        public IEnumerator ShowCor()
        {
            yield return _circle.DOSizeDelta(Vector2.zero, 1.0f).WaitForCompletion();
        }

        public IEnumerator HideCor()
        {
            yield return _circle.DOSizeDelta(Vector2.one * 5000.0f, 1.0f).WaitForCompletion();
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