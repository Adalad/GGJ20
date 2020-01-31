using UnityEngine;

public class VRControllerManager : MonoBehaviour
{
    public static VRControllerManager Instance { get; private set; }

    public GameObject LeftController;
    public GameObject RightController;

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
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
