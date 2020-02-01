using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip EndClip;
    public float FadeDuration = 1;
    [Range(0, 1)]
    public float MaxVolume = 1;

    private AudioSource SourceComponent;

    void Start()
    {
        SourceComponent = GetComponent<AudioSource>();
        SceneController.Instance.BeforeSceneUnload += FadeOutMusic;
        SceneController.Instance.AfterSceneLoad += FadeInMusic;
    }

    private void FadeOutMusic(string scene)
    {
        if (scene == "Ending")
        {
            StartCoroutine(Fade(0));
        }
    }

    private void FadeInMusic(string scene)
    {
        if (scene == "Ending")
        {
            SourceComponent.clip = EndClip;
            StartCoroutine(Fade(MaxVolume));
        }
    }

    private IEnumerator Fade(float final)
    {
        float fadeSpeed = Mathf.Abs(SourceComponent.volume - final) / FadeDuration;

        while (!Mathf.Approximately(SourceComponent.volume, final))
        {
            SourceComponent.volume = Mathf.MoveTowards(SourceComponent.volume, final, fadeSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
