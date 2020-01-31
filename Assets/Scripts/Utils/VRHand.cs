using UnityEngine;

public class VRHand : MonoBehaviour
{
    public string Side = "Left";
    public VRFinger[] fingers = new VRFinger[5];

    void Update()
    {
        // Thumb
        if (Input.GetButton(Side + "ThumbTouch"))
        {
            fingers[0].FlexFinger(-VRHand.Map(0.5f, 0, 1, 90, 0));
        }
        else
        {
            fingers[0].FlexFinger(-VRHand.Map(1, 0, 1, 90, 0));
        }
        // Index
        if (Input.GetAxis(Side + "Trigger") > 0)
        {
            fingers[1].FlexFinger(-VRHand.Map(1 - Input.GetAxis(Side + "Trigger"), 0, 1, 90, 30));
        }
        else if (Input.GetButton(Side + "TriggerTouch"))
        {
            fingers[1].FlexFinger(-30f);
        }
        else
        {
            fingers[1].FlexFinger(0f);
        }
        // Rest
        for (int i = 2; i < fingers.Length; ++i)
        {
            if (Input.GetAxis(Side + "Grab") > 0)
            {
                fingers[i].FlexFinger(-VRHand.Map(1 - Input.GetAxis(Side + "Grab"), 0, 1, 90, 45));
            }
            else
            {
                fingers[i].FlexFinger(-45f);
            }
        }
    }

    public static float Map(float value, float originalStart, float originalEnd, float newStart, float newEnd)
    {
        float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
        return (newStart + ((value - originalStart) * scale));
    }
}