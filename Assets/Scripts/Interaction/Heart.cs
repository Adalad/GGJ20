using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Heart : VRInteractableObject
{
    public GameObject FullHeart;
    public GameObject FracturedHeart;
    public float ExplodeStrength = 1f;
    private Transform DefaultParent;
    private Vector3 DefaultLocalPosition;
    private VRController Controller;
    private Coroutine Routine;

    private void Start()
    {
        DefaultParent = transform.parent;
        DefaultLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Controller != null)
        {
            transform.position = Controller.transform.position;
        }
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
            if (Routine != null)
            {
                StopCoroutine(Routine);
                Routine = null;
            }

            transform.parent = null;
            transform.rotation = Quaternion.identity;
            Controller = controller;
        }
    }

    public override void OnControllerEnter(VRController controller)
    {
        if (Interactable && (controller.CurrentGesture == VRController.ControllerGesture.FIST) && (controller.Velocity.magnitude > 0.3f))
        {
            ClearLink();
            Interactable = false;
            FullHeart.SetActive(false);
            FracturedHeart.SetActive(true);
            foreach (Rigidbody child in FracturedHeart.transform.GetComponentsInChildren<Rigidbody>())
            {
                child.AddForce((child.transform.position - transform.position).normalized * ExplodeStrength);
            }
            StartCoroutine(QuitRoutine());
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
            }

            Controller.OnControllerChanged(this);
            Controller = null;
            Routine = StartCoroutine(RespawnHeart());
        }
    }

    private IEnumerator RespawnHeart()
    {
        yield return new WaitForSeconds(5);
        transform.parent = DefaultParent;
        transform.localPosition = DefaultLocalPosition;
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
