using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : VRInteractableObject
{
    private Rigidbody RigidBody;
    private VRController Controller;

    private void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    private void OnJointBreak(float breakForce)
    {
        ClearLink();
    }

    public override void OnGestureChanged(VRController.ControllerGesture gesture, VRController controller)
    {
        if (controller == Controller)
        {
            if (gesture != VRController.ControllerGesture.GRAB)
            {
                ClearLink();
            }
        }
        else if (gesture == VRController.ControllerGesture.GRAB)
        {
            ClearLink();
            RigidBody.isKinematic = false;
            Controller = controller;
            FixedJoint joint = Controller.gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            joint.connectedBody = RigidBody;
        }
    }

    public override void OnControllerExit(VRController controller)
    {
        if (controller == Controller)
        {
            ClearLink();
        }
    }

    private void ClearLink()
    {
        if (Controller != null)
        {
            if (Controller.TryGetComponent<FixedJoint>(out FixedJoint joint))
            {
                joint.connectedBody = null;
                Destroy(joint);
                if (RigidBody != null)
                {
                    RigidBody.isKinematic = false;
                    RigidBody.velocity = Controller.Velocity;
                    RigidBody.angularVelocity = Controller.AngularVelocity;
                }
            }

            Controller.OnControllerChanged(this);
            Controller = null;
        }
    }
}
