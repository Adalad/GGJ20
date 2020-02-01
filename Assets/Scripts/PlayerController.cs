using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource HeartBeat;
    public Renderer LeftHand;
    public Renderer RightHand;
    public Renderer Heart;
    public float SanityDownRate = 1;
    public float Sanity = 100f;

    private void Update()
    {
        Sanity = Mathf.Clamp(Sanity - SanityDownRate * Time.deltaTime, 0f, 100f);
        HeartBeat.pitch = Map(100 - Sanity, 0f, 100f, 1f, 2f);
        LeftHand.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        LeftHand.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .002f));
        LeftHand.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        LeftHand.material.SetFloat("_TimeScale", Map(100 - Sanity, 0f, 100f, 1f, 11.25f));
        RightHand.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        RightHand.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .002f));
        RightHand.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        RightHand.material.SetFloat("_TimeScale", Map(100 - Sanity, 0f, 100f, 1f, 11.25f));
        Heart.material.SetFloat("_Scale", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        Heart.material.SetFloat("_Effect", Map(100 - Sanity, 0f, 100f, 0f, .01f));
        Heart.material.SetFloat("_Glow", Map(100 - Sanity, 0f, 100f, 0f, .05f));
        Heart.material.SetFloat("_TimeScale", Map(100 - Sanity, 0f, 100f, 5.5f, 11.25f));
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
