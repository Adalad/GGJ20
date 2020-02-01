using UnityEngine;

public class Logo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneController.Instance.FadeAndLoadScene("MainScene");
    }
}
