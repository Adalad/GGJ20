using UnityEngine;
using UnityEngine.AI;

public class FleeBehaviour : StateMachineBehaviour
{
    private NavMeshAgent Agent;
    private Transform Player;
    private Vector3[] Waypoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent = animator.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(animator.transform.position, Player.position);
        if (distance > 3)
        {
            animator.SetBool("Flee", false);
        }
        else
        {
            Agent.destination = Agent.transform.position + (Agent.transform.position - Player.position).normalized;
        }
    }
}
