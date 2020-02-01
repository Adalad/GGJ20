using UnityEngine;

public class NPCBack : MonoBehaviour
{
    public NPC NPCComponent;
    private bool LeftController;
    private bool RightController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.name == "LeftController")
            {
                LeftController = true;
            }
            else if (other.name == "RightController")
            {
                RightController = true;
            }

            if (LeftController && RightController)
            {
                NPCComponent.Hugged(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (other.name == "Leftcontroller")
            {
                LeftController = false;
            }
            else if (other.name == "RightController")
            {
                RightController = false;
            }

            NPCComponent.Hugged(false);
        }
    }
}
