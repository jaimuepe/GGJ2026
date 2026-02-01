#nullable enable

using System.Collections;
using Masks.Catalog;
using Unity.Cinemachine;
using UnityEngine;

namespace Masks
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField] private BubbleMessageUI _bubbleMessagePrefab;
        
        [SerializeField] private PartyGuest _birthdayKid;
        [SerializeField] private CinemachineCamera _endCamera;

        [SerializeField] private CinemachineBrain _brain;

        [SerializeField] private Canvas _overlayCanvas;
        
        private void Start()
        {
            _birthdayKid.Character.ChangeFace("Sad");
            StartCoroutine(Play());
        }

        private IEnumerator Play()
        {
            _endCamera.Priority = new PrioritySettings
            {
                Value = 100,
            };

            yield return new WaitUntil(() => _brain.IsBlending);
            yield return new WaitWhile(() => _brain.IsBlending);

            yield return new WaitForSeconds(0.5f);
            
            var bubble = Instantiate(_bubbleMessagePrefab, _overlayCanvas.transform, false);
            bubble.SetData(_birthdayKid.bubbleAnchor, "I wish... for everyone to come on my next birthday!");
            bubble.gameObject.SetActive(true);
            
            bubble.Show(duration: 2.0f);
            
            yield return new WaitForSeconds(3.0f);
            
            _birthdayKid.PlayState("BlowCandles", 0.0f);

            yield return new WaitForSeconds(3.0f);

            SceneTransitionUI.Instance.LoadSceneWithTransition("MaskEditor");
        }
    }
}