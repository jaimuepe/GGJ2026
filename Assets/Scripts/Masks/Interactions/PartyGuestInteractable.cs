#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Masks.Interactions
{
    public class PartyGuestInteractable : MonoBehaviour, IInteractable
    {
        public bool NeedsInput => true;
        
        public bool HasInteractionStarted { get; private set; }

        public bool IsInteractionCompleted { get; private set; }

        private PartyGuest _partyGuest;

        private Coroutine? _interactCor;

        private PartyGuestCanvas _canvas;

        private bool _canCaptureInput;
        private bool _hasReceivedInput;
        
        private void Start()
        {
            _partyGuest = GetComponentInParent<PartyGuest>();
            _canvas = FindFirstObjectByType<PartyGuestCanvas>();
            Assert.IsNotNull(_partyGuest);
        }

        public void Interact()
        {
            if (_interactCor != null) return;

            _canCaptureInput = false;
            _hasReceivedInput = false;
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
            if (!_canCaptureInput) return;
            _hasReceivedInput = true;
        }

        private IEnumerator InteractCor()
        {
            _canCaptureInput = false;
            
            _partyGuest.ZoomIn();

            yield return null;
            if (_partyGuest.CameraIsBlending())
            {
                yield return new WaitUntil(() => !_partyGuest.CameraIsBlending());
            }
            
            _canvas.SetData(_partyGuest.Character);
            _canvas.Show();
            
            yield return new WaitForSeconds(1.0f);

            _canCaptureInput = true;

            yield return new WaitUntil(() => _hasReceivedInput);
            
            _canvas.Hide();
            
            _partyGuest.ZoomOut();

            yield return null;
            if (_partyGuest.CameraIsBlending())
            {
                yield return new WaitUntil(() => !_partyGuest.CameraIsBlending());
            }

            IsInteractionCompleted = true;
            
            _interactCor = null;
        }
    }
}