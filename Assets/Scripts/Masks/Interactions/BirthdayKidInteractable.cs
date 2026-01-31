#nullable enable

using System.Collections;
using UnityEngine;

namespace Masks.Interactions
{
    public class BirthdayKidInteractable : MonoBehaviour, IInteractable
    {
        public bool NeedsInput => false;

        public bool HasInteractionStarted { get; private set; }

        public bool IsInteractionCompleted { get; private set; }

        private PartyGuest _partyGuest;

        private Coroutine? _interactCor;

        private void Start()
        {
            _partyGuest = GetComponentInParent<PartyGuest>();
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

            yield return null;
            if (_partyGuest.CameraIsBlending())
            {
                yield return new WaitUntil(() => !_partyGuest.CameraIsBlending());
            }

            var canvas = FindFirstObjectByType<BirthdayKidCanvas>();
            canvas.Show();

            yield return new WaitUntil(() => canvas.IsVisible());

            yield return new WaitWhile(() => canvas.IsVisible());

            if (canvas.IsBlowingTheCandles) yield break;

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