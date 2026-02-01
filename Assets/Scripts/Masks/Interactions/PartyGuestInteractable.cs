#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Masks.Interactions
{
    public class PartyGuestInteractable : MonoBehaviour, IInteractable
    {
        public bool NeedsInput => false;
        
        public bool HasInteractionStarted { get; private set; }

        public bool IsInteractionCompleted { get; private set; }

        private PartyGuest _partyGuest;

        private Coroutine? _interactCor;

        private PartyGuestCanvas _canvas;

        private void Start()
        {
            _partyGuest = GetComponentInParent<PartyGuest>();
            _canvas = FindFirstObjectByType<PartyGuestCanvas>();
            Assert.IsNotNull(_partyGuest);
        }

        public void Interact()
        {
            if (_interactCor != null) return;

            HasInteractionStarted = true;
            IsInteractionCompleted = false;
            
            _interactCor = StartCoroutine(InteractCor());
        }

        public void Focus()
        {
        }

        public void Unfocus()
        {
        }
        
        public void ReceiveInput()
        {
        }

        private IEnumerator InteractCor()
        {
            _partyGuest.ZoomIn();
            
            yield return new WaitUntil(() => _partyGuest.CameraIsBlending());
            yield return new WaitWhile(() => _partyGuest.CameraIsBlending());
            
            _canvas.SetData(_partyGuest.Character);
            _canvas.Show();
            
            yield return new WaitUntil(() => _canvas.IsVisible());
            yield return new WaitWhile(() => _canvas.IsVisible());

            _partyGuest.ZoomOut();
            
            yield return new WaitUntil(() => _partyGuest.CameraIsBlending());
            yield return new WaitWhile(() => _partyGuest.CameraIsBlending());

            IsInteractionCompleted = true;
            
            _interactCor = null;
        }
    }
}