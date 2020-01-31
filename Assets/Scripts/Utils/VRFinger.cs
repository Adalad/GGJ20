using UnityEngine;

public class VRFinger : MonoBehaviour
{
    // mapping bones (drag and drop your prefered bones here)
    public Transform indexProximal;
    public Transform indexIntermediate;
    public Transform indexDistal;

    // array to store bones
    private Transform[] bones;

    // arrays to store the angles of the bones
    private Quaternion[] initialDirections; // for reset purposes
    private Quaternion[] boneDirections;
    private int ARRAYLENGTH = 3;

    void Start()
    {
        bones = new Transform[ARRAYLENGTH];
        initialDirections = new Quaternion[ARRAYLENGTH];
        boneDirections = new Quaternion[ARRAYLENGTH];

        // I map the bones into the array
        MapBones();
        // after which I store the initial directions
        GetInitialDirections();
    }

    // Map bones if they are available
    void MapBones()
    {
        if (indexProximal != null) { bones[0] = indexProximal; }
        if (indexIntermediate != null) { bones[1] = indexIntermediate; }
        if (indexDistal != null) { bones[2] = indexDistal; }
    }

    // get initial angles of bones in case the rig needs to be reset
    void GetInitialDirections()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                initialDirections[i] = bones[i].localRotation;
                boneDirections[i] = bones[i].localRotation;
            }
        }
    }

    // reset rig (this demonstrates the use of the 'initialDirections' array
    void ResetPositions()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localRotation = initialDirections[i];
        }
    }

    // Rotate joint to given angle
    void RotateJoint(int bone, float angle)
    {
        if (angle > 0)
        {
            return;
        }

        if (bones[bone] != null)
        {
            Quaternion target = Quaternion.Euler(0, 0, angle); // define target angle, in this case a rotation around Z axis
            bones[bone].localRotation = target * boneDirections[bone]; // rotate bone to new position
        }
    }

    public void FlexFinger(float angle)
    {
        if (bones == null)
        {
            return;
        }

        for (int i = 0; i < bones.Length; i++)
        {
            RotateJoint(i, angle);
        }
    }
}
