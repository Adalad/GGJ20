using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource HeartBeat;
    public Renderer LeftHand;
    public Renderer RightHand;
    public Renderer Heart;

    public float Sanity = 100f;

    // Update is called once per frame
    private void Update()
    {
        HeartBeat.pitch = 1f;
        LeftHand.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        LeftHand.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        LeftHand.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        LeftHand.material.SetFloat("_TimeScale", Map(100 - Sanity, 0f, 100f, 1f, 10f));
        RightHand.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        RightHand.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        RightHand.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        Heart.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        Heart.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        Heart.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
    }

    public void AddSanity(float ammount)
    {
        Sanity = Mathf.Clamp(Sanity + ammount, 0f, 100f);
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
