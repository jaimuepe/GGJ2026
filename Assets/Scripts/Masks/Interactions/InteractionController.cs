#nullable enable

using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Masks.Interactions
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private BoxCollider _interactableShape;
        [SerializeField] private LayerMask _interactableMask;
        [SerializeField] private GameObject _bubble;
        [SerializeField] private PlayerInput _playerInput;

        [SerializeField] private GameObject _visualsObject;
        [SerializeField] private ThirdPersonController _characterController;

        private IInteractable? _current;

        private Coroutine? _interactCor;

        private void Start()
        {
            HideBubble();
        }

        private readonly Collider[] _cachedResults = new Collider[1];

        private void Update()
        {
            if (_current != null &&
                _current.HasInteractionStarted &&
                !_current.IsInteractionCompleted)
            {
                if (_current!.NeedsInput &&
                    _playerInput.actions["interact"].WasPressedThisFrame())
                {
                    _current.ReceiveInput();
                }

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

            if (_playerInput.actions["interact"].WasPressedThisFrame() && _current != null)
            {
                Interact();
            }
        }

        private void ShowBubble()
        {
            _bubble.SetActive(true);
        }

        private void HideBubble()
        {
            _bubble.SetActive(false);
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