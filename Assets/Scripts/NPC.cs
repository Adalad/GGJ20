using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Animator MeshAnimator;
    private Animator AnimatorComponent;
    private Vector3 LastPos;

    private void Start()
    {
        AnimatorComponent = GetComponent<Animator>();
        LastPos = transform.position;
    }

    private void Update()
    {
        MeshAnimator.SetFloat("Speed", Vector3.Distance(transform.position, LastPos));
        LastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            AnimatorComponent.GetBehaviour<WalkBhaviour>().Triggered();
        }
    }
}
