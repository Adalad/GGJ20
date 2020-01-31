using System.Collections.Generic;
using UnityEngine;

public class VRSwingLocomotion : VRLocomotion
{
    // Controllers
    private GameObject LeftControllerGameObject;
    private GameObject RightControllerGameObject;
    private VRController LeftController;
    private VRController RightController;

    // Controller positions
    private Vector3 leftControllerLocalPosition;
    private Vector3 rightControllerLocalPosition;
    private Vector3 leftControllerPreviousLocalPosition;
    private Vector3 rightControllerPreviousLocalPosition;

    // Saved movement
    private float LastMovement;
    private Vector3 LastDirection;

    // Controller gestures
    private VRController.ControllerGesture armGesture = VRController.ControllerGesture.GRIP;
    private bool leftGestureEnabled = false;
    private bool rightGestureEnabled = false;

    private void Awake()
    {
        LeftControllerGameObject = VRControllerManager.Instance.LeftController;
        RightControllerGameObject = VRControllerManager.Instance.RightController;
        LeftController = LeftControllerGameObject.GetComponent<VRController>();
        RightController = RightControllerGameObject.GetComponent<VRController>();
        leftControllerPreviousLocalPosition = LeftControllerGameObject.transform.localPosition;
        rightControllerPreviousLocalPosition = RightControllerGameObject.transform.localPosition;
    }

    private void FixedUpdate()
    {
        GetControllerGestures();
        leftControllerLocalPosition = LeftControllerGameObject.transform.localPosition;
        rightControllerLocalPosition = RightControllerGameObject.transform.localPosition;
        CalculateMovement();
        leftControllerPreviousLocalPosition = LeftControllerGameObject.transform.localPosition;
        rightControllerPreviousLocalPosition = RightControllerGameObject.transform.localPosition;
    }

    public override void GetMovement(ref float movementX, ref float movementY, ref float rotation)
    {
        movementX = 0f;
        movementY = LastMovement;
        rotation = 0f;
    }

    // Variable Arm Swing locomotion
    private void CalculateMovement()
    {
        if (leftGestureEnabled && rightGestureEnabled)
        {
            //Vector3 leftVector = Vector3.ProjectOnPlane(leftControllerLocalPosition - leftControllerPreviousLocalPosition, Vector3.up).normalized;
            //Vector3 rightVector = Vector3.ProjectOnPlane(rightControllerLocalPosition - rightControllerPreviousLocalPosition, Vector3.up).normalized;
            float leftControllerChange = Vector3.Distance(leftControllerPreviousLocalPosition, leftControllerLocalPosition);
            float rightControllerChange = Vector3.Distance(rightControllerPreviousLocalPosition, rightControllerLocalPosition);
            LastMovement = Map(leftControllerChange + rightControllerChange / 2, 0f, 0.05f, 0f, 1f);
            LastDirection = Vector3.zero;
            return;
        }

        LastDirection = Vector3.zero;
        LastMovement = 0f;
    }

    private void GetControllerGestures()
    {
        // Left
        leftGestureEnabled = (LeftController.CurrentGesture == armGesture) ? true : false;
        //Right
        rightGestureEnabled = (RightController.CurrentGesture == armGesture) ? true : false;
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
