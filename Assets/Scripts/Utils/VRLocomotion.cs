using UnityEngine;

public abstract class VRLocomotion : MonoBehaviour
{
    public abstract void GetMovement(ref float movementX, ref float movementY, ref float rotation);
}
