using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class VRController : MonoBehaviour
{
    /*  https://docs.unity3d.com/Manual/OpenVRControllers.html
        TRACKED AXES (4)
        0 - Horizontal pad
        1 - Vertical pad
        2 - Trigger 1 squeeze
        3 - Trigger 2 squeeze

        TRACKED BUTTONS (9)
        0 - Menu
        1 - Extra button
        2 - Pad pess
        3 - Pad touch
        4 - Trigger touch
        5 - A
        6 - B
        7 - X
        8 - Y
     */
    public enum ControllerGesture
    {
        NONE, POINT, FIST, GRIP, GRAB, MENU, A, B, X, Y
    }

    [Header("Tracking Settings")]
    [Tooltip("0 - Horizontal pad, 1 - Vertical pad, 2 - Trigger 1 squeeze, 3 - Trigger 2 squeeze")]
    public string[] AxesTracked = new string[4];
    [Tooltip("0 - Menu, 1 - Extra button, 2 - Pad pess, 3 - Pad touch, 4 - Trigger touch, 5 - A, 6 - B, 7 - X, 8 - Y")]
    public string[] ButtonsTracked = new string[9];
    public ControllerGesture CurrentGesture
    {
        get;
        private set;
    }
    public XRNode Node;
    public Vector3 Velocity;
    public Vector3 AngularVelocity;

    private ControllerGesture PreviousGesture;
    private VRInteractableObject LockedInteractable;
    private readonly List<XRNodeState> NodeStatesCache = new List<XRNodeState>();

    private void Update()
    {
        TryGetNodeStateTransform();

        if ((Input.GetAxis(AxesTracked[3]) == 1) && (Input.GetButton(ButtonsTracked[3])) && (!Input.GetButton(ButtonsTracked[4]))) // POINT
        {
            CurrentGesture = ControllerGesture.POINT;
        }
        else if ((Input.GetAxis(AxesTracked[2]) == 1) && (Input.GetAxis(AxesTracked[3]) == 1) && (Input.GetButton(ButtonsTracked[3]))) // FIST
        {
            CurrentGesture = ControllerGesture.FIST;
        }
        else if ((Input.GetAxis(AxesTracked[3]) == 1) && (!Input.GetButton(ButtonsTracked[4]))) // GRIP
        {
            CurrentGesture = ControllerGesture.GRIP;
        }
        else if ((Input.GetAxis(AxesTracked[2]) == 1) && (Input.GetAxis(AxesTracked[3]) == 1) && (!Input.GetButton(ButtonsTracked[3]))) // GRAB
        {
            CurrentGesture = ControllerGesture.GRAB;
        }
        else // NONE
        {
            CurrentGesture = ControllerGesture.NONE;
        }

        if (PreviousGesture != CurrentGesture)
        {
            PreviousGesture = CurrentGesture;
            if (LockedInteractable != null)
            {
                LockedInteractable.OnGestureChanged(CurrentGesture, this);
            }
        }
    }

    private bool TryGetNodeStateTransform()
    {
        InputTracking.GetNodeStates(NodeStatesCache);
        for (int i = 0; i < NodeStatesCache.Count; i++)
        {
            XRNodeState nodeState = NodeStatesCache[i];
            if (nodeState.nodeType == Node)
            {
                if (nodeState.TryGetAngularVelocity(out AngularVelocity) && nodeState.TryGetVelocity(out Velocity))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LockedInteractable == null) && other.TryGetComponent<VRInteractableObject>(out LockedInteractable))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((LockedInteractable != null) && (other.gameObject == LockedInteractable.gameObject))
        {
            LockedInteractable.OnControllerExit(this);
            LockedInteractable = null;
        }
    }

    public void OnControllerChanged(VRInteractableObject interactable)
    {
        if (interactable == LockedInteractable.gameObject)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            LockedInteractable = null;
        }
    }
}
