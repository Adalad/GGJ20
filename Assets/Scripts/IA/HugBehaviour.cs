using UnityEngine;
using UnityEngine.AI;

public class HugBehaviour : StateMachineBehaviour
{
    private NavMeshAgent Agent;
    private Transform Player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent = animator.GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(animator.transform.position, Player.position);
        if (distance > 0.5f)
        {
            animator.SetBool("Hug", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent.isStopped = false;
    }
}
