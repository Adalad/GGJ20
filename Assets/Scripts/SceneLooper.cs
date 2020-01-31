using UnityEngine;

public class SceneLooper : MonoBehaviour
{
    [Range(-1,1)]
    public int Xdelta = 0;
    [Range(-1, 1)]
    public int Zdelta = 0;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) || (other.CompareTag("NPC")))
        {
            Vector3 newPosition = Vector3.zero;
            newPosition.x = other.transform.position.x * Xdelta;
            newPosition.z = other.transform.position.z * Zdelta;
            other.transform.position = newPosition;
        }
    }
}
