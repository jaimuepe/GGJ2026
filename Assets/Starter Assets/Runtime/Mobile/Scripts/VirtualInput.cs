using StarterAssets;
using UnityEngine;

public class VirtualInput : MonoBehaviour
{
    [Header("Output")]
    public StarterAssetsInputs StarterAssetsInputs;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        StarterAssetsInputs.MoveInput(virtualMoveDirection);
    }

    public void VirtualInteractInput(bool virtualInteractState)
    {
        StarterAssetsInputs.InteractInput(virtualInteractState);
    }
}
