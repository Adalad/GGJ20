using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRLocomotionManager : MonoBehaviour
{
    public enum LocomotionMode
    {
        PAD, SWING, TELEPORT
    }

    public Transform Head;
    public Renderer Blur;
    public float BlurDuration = 1f;
    public float MoveSpeed = 1f;
    public float RotateSpeed = 1f;
    public LocomotionMode Mode;
    private CharacterController CharacterController;
    private Vector3 MoveDirection;
    private bool IsBlurred = false;
    private Coroutine BlurCoroutine;
    private VRLocomotion LocomotionComponent;
    private float MoveHorizontal;
    private float MoveVertical;
    private float Rotation;

    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        switch (Mode)
        {
            case LocomotionMode.PAD:
                LocomotionComponent = gameObject.AddComponent<VRPadLocomotion>();
                break;
            case LocomotionMode.SWING:
                LocomotionComponent = gameObject.AddComponent<VRSwingLocomotion>();
                break;
        }
    }

    private void Update()
    {
        HandleCenter();
        CalculateMovement();
    }

    public IEnumerator DoBlur(float finalAlpha)
    {
        if (finalAlpha > 0)
        {
            IsBlurred = true;
        }

        // Calculate how fast should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(Blur.material.GetFloat("_Alpha") - finalAlpha) / BlurDuration;

        // While it hasn't reached the final alpha yet...
        while (Mathf.Abs(Blur.material.GetFloat("_Alpha") - finalAlpha) > 0.05f)
        {
            // ... move the alpha towards it's target alpha.
            Blur.material.SetFloat("_Alpha", Mathf.MoveTowards(Blur.material.GetFloat("_Alpha"), finalAlpha, fadeSpeed * Time.deltaTime));

            // Wait for a frame then continue.
            yield return null;
        }

        Blur.material.SetFloat("Cutoff", finalAlpha);
        if (finalAlpha == 0)
        {
            IsBlurred = false;
        }
    }

    private void CalculateMovement()
    {
        if (LocomotionComponent)
        {
            LocomotionComponent.GetMovement(ref MoveHorizontal, ref MoveVertical, ref Rotation);
            MoveDirection = (Head.forward * MoveVertical + Head.right * MoveHorizontal) * MoveSpeed * Time.deltaTime;
            if (!CharacterController.isGrounded)
            {
                MoveDirection.y = Physics.gravity.y * Time.deltaTime;
            }

            CharacterController.Move(MoveDirection);
            transform.RotateAround(Head.transform.position, Vector3.up, Rotation * RotateSpeed * Time.deltaTime);
        }
        else
        {
            MoveHorizontal = 0f;
            MoveVertical = 0f;
            Rotation = 0f;
        }

        if ((MoveHorizontal != 0) || (MoveVertical != 0) || (Rotation != 0))
        {
            if (!IsBlurred)
            {
                if (BlurCoroutine != null)
                {
                    StopCoroutine(BlurCoroutine);
                }

                BlurCoroutine = StartCoroutine(DoBlur(1));
            }
        }
        else if (IsBlurred && (MoveHorizontal == 0) && (MoveVertical == 0) && (Rotation == 0))
        {
            if (BlurCoroutine != null)
            {
                StopCoroutine(BlurCoroutine);
            }

            BlurCoroutine = StartCoroutine(DoBlur(0));
        }
    }

    private void HandleCenter()
    {
        // Get head in local space
        float headHeight = Mathf.Clamp(Head.localPosition.y, 1, 2);
        CharacterController.height = headHeight;
        // Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = CharacterController.height / 2;
        // Move capsule in local space
        newCenter.x = Head.localPosition.x;
        newCenter.z = Head.localPosition.z;
        // Rotate
        //newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;
        // Apply
        CharacterController.center = newCenter;
    }
}
