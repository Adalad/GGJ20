using UnityEngine;

public abstract class VRInteractableObject : MonoBehaviour
{
    public bool Interactable;

    public abstract void OnGestureChanged(VRController.ControllerGesture gesture, VRController controller);

    public abstract void OnControllerExit(VRController controller);
}
