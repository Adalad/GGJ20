using UnityEngine;

public class VRPadLocomotion : VRLocomotion
{
    private float MoveHorizontal;
    private float MoveVertical;
    private float RotateHorizontal;

    public override void GetMovement(ref float movementX, ref float movementY, ref float rotation)
    {
        movementX = MoveHorizontal;
        movementY = MoveVertical;
        rotation = RotateHorizontal;
    }

    private void Update()
    {
        MoveHorizontal = (Mathf.Abs(Input.GetAxis("HorizontalL")) > 0.2f) ? Input.GetAxis("HorizontalL") : 0;
        MoveVertical = (Mathf.Abs(Input.GetAxis("VerticalL")) > 0.2f) ? -Input.GetAxis("VerticalL") : 0;
        RotateHorizontal = (Mathf.Abs(Input.GetAxis("HorizontalR")) > 0.2f) ? Input.GetAxis("HorizontalR") : 0;
    }
}
