#nullable enable

using System.Collections;
using StarterAssets;
using UnityEngine;

namespace Masks.Interactions
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private BoxCollider _interactableShape;
        [SerializeField] private LayerMask _interactableMask;
        [SerializeField] private InteractionBubbleUI _bubble;

        [SerializeField] private GameObject _visualsObject;
        [SerializeField] private ThirdPersonController _characterController;

        [SerializeField] private StarterAssetsInputs _inputs;

        private IInteractable? _current;

        private Coroutine? _interactCor;

        private void Start()
        {
            HideBubble();
        }

        private void OnEnable()
        {
            _inputs.onInteractionPressed += TryInteract;
        }

        private void OnDisable()
        {
            _inputs.onInteractionPressed -= TryInteract;
        }

        private readonly Collider[] _cachedResults = new Collider[1];

        private void Update()
        {
            if (_current != null &&
                _current.HasInteractionStarted &&
                !_current.IsInteractionCompleted)
            {
                return;
            }

            var center = _interactableShape.transform.TransformPoint(_interactableShape.center);
            var halfExtents = Vector3.Scale(_interactableShape.size * 0.5f, _interactableShape.transform.lossyScale);
            var orientation = _interactableShape.transform.rotation;

            var size = Physics.OverlapBoxNonAlloc(center, halfExtents, _cachedResults, orientation, _interactableMask,
                QueryTriggerInteraction.Collide);
            if (size > 0)
            {
                var interactable = _cachedResults[0].gameObject.GetComponent<IInteractable>();

                if (interactable != _current)
                {
                    _current?.Unfocus();
                    _current = interactable;
                    _current.Focus();

                    if (interactable == null)
                    {
                        HideBubble();
                    }
                    else
                    {
                        ShowBubble();
                    }
                }
            }

            else
            {
                if (_current != null)
                {
                    _current.Unfocus();
                    HideBubble();

                    _current = null;
                }
            }
        }

        private void ShowBubble()
        {
            _bubble.Show();
        }

        private void HideBubble()
        {
            _bubble.Hide();
        }

        private void TryInteract()
        {
            if (_current != null)
            {
                Interact();
            }
        }

        private void Interact()
        {
            if (_interactCor != null) return;
            _interactCor = StartCoroutine(InteractCor());
        }

        private IEnumerator InteractCor()
        {
            _characterController.enabled = false;

            Utils.SetLayerRecursive(_visualsObject, LayerMask.NameToLayer("Invisible"));

            _current!.Interact();
            HideBubble();

            yield return new WaitUntil(() => _current.IsInteractionCompleted);

            Utils.SetLayerRecursive(_visualsObject, LayerMask.NameToLayer("Default"));

            _current = null;
            _interactCor = null;

            _characterController.enabled = true;
        }
    }
}