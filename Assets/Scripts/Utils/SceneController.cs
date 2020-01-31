using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This script exists in the Persistent scene and manages the content
// based scene's loading.  It works on a principle that the
// Persistent scene will be loaded first, then it loads the scenes that
// contain the player and other visual elements when they are needed.
// At the same time it will unload the scenes that are not needed when
// the player leaves them.
public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public event Action<string> BeforeSceneUnload;          // Event delegate that is called just before a scene is unloaded.
    public event Action<string> AfterSceneLoad;             // Event delegate that is called just after a scene is loaded.

    public string StartingSceneName = "InitScene";  // The name of the scene that should be loaded first.
    public string InitialStartingPositionName = "InitSpawn";

    public Transform CameraRig;
    public VRScreenFade ScreenFade;             // The quad renderer used for fading to black.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  //if not, set instance to this
        }
        else if (Instance != this)  //If instance already exists and it's not this:
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
    }

    private IEnumerator Start()
    {
        // Set the initial alpha to start off with a black screen.
        ScreenFade.Clear();

        // Start the first scene loading and wait for it to finish.
        yield return StartCoroutine(FadeAndSwitchScenes(StartingSceneName));

        // Once the scene is finished loading, start fading in.
        StartCoroutine(ScreenFade.Fade(0f));
    }


    // This is the main external point of contact and influence from the rest of the project.
    public void FadeAndLoadScene(string sceneName)
    {
        // If a fade isn't happening then start fading and switching scenes.
        if (!ScreenFade.IsFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }


    // This is the coroutine where the 'building blocks' of the script are put together.
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Start fading to black and wait for it to finish before continuing.
        yield return StartCoroutine(ScreenFade.Fade(1f));

        // If this event has any subscribers, call it.
        BeforeSceneUnload?.Invoke(sceneName);

        // Unload the current active scene.
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        // Start loading the given scene and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // If this event has any subscribers, call it.
        AfterSceneLoad?.Invoke(sceneName);

        // Start fading back in and wait for it to finish before exiting the function.
        yield return StartCoroutine(ScreenFade.Fade(0f));
    }


    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Allow the given scene to load over several frames and add it to the already loaded scenes (just the Persistent scene at this point).
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Find the scene that was most recently loaded (the one at the last index of the loaded scenes).
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene (this marks it as the one to be unloaded next).
        SceneManager.SetActiveScene(newlyLoadedScene);

        // Move to spawn
        GameObject spawn = GameObject.Find(InitialStartingPositionName);
        CameraRig.transform.position = (spawn != null) ? spawn.transform.position : Vector3.zero;
    }
}
