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

        [SerializeField] private BubbleMessageUI _bubble;
        
        private void Start()
        {
            _birthdayKid.Character.ChangeFace("Sad");
            _bubble.SetData(null, "I wish... I wish someone comes to my next birthday!");
            
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
            
            _bubble.Show(duration: 2.5f);
            
            yield return new WaitForSeconds(3.0f);
            
            _birthdayKid.PlayState("BlowCandles_Sad", 0.0f);

            yield return new WaitForSeconds(3.0f);

            SceneTransitionUI.Instance.LoadSceneWithTransition("MaskEditor");
        }
    }
}