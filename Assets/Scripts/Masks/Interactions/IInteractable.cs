#nullable enable

namespace Masks.Interactions
{
    public interface IInteractable
    {
        bool NeedsInput { get; }
        
        bool HasInteractionStarted { get; }

        bool IsInteractionCompleted { get; }
        
        void Interact();

        void Focus();

        void Unfocus();
        
        void ReceiveInput();
    }
}