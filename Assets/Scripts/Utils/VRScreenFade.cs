using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(MeshCollider))]
public class VRScreenFade : MonoBehaviour
{
    private Renderer ScreenFadeRenderer;
    private MeshCollider ScreenFadeCollider;
    public float FadeDuration = 1f;               // How long it should take to fade to and from black.
    public bool IsFading                          // Flag used to determine if the Image is currently fading to or from black.
    {
        get;
        private set;
    }

    private void Start()
    {
        ScreenFadeRenderer = GetComponent<Renderer>();
        ScreenFadeCollider = GetComponent<MeshCollider>();
    }

    public void Clear()
    {
        ScreenFadeRenderer = GetComponent<Renderer>();
        ScreenFadeCollider = GetComponent<MeshCollider>();
        ScreenFadeRenderer.material.SetFloat("_Alpha", 1);
        ScreenFadeCollider.enabled = false;
    }

    public IEnumerator Fade(float finalAlpha)
    {
        // Set the fading flag to true.
        IsFading = true;

        // Make sure it blocks raycasts into the scene so no more input can be accepted.
        ScreenFadeCollider.enabled = true;

        // Calculate how fast should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(ScreenFadeRenderer.material.GetFloat("_Alpha") - finalAlpha) / FadeDuration;

        // While it hasn't reached the final alpha yet...
        while (!Mathf.Approximately(ScreenFadeRenderer.material.GetFloat("_Alpha"), finalAlpha))
        {
            // ... move the alpha towards it's target alpha.
            ScreenFadeRenderer.material.SetFloat("_Alpha", Mathf.MoveTowards(ScreenFadeRenderer.material.GetFloat("_Alpha"), finalAlpha, fadeSpeed * Time.deltaTime));

            // Wait for a frame then continue.
            yield return null;
        }

        // Set the flag to false since the fade has finished.
        IsFading = false;

        // Stop from blocking raycasts so input is no longer ignored.
        ScreenFadeCollider.enabled = false;
    }
}
