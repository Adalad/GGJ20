using UnityEngine;
using UnityEngine.AI;

public class HugBehaviour : StateMachineBehaviour
{
    private NavMeshAgent Agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent = animator.GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent.isStopped = false;
    }
}
