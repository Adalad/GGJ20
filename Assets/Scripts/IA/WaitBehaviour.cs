using UnityEngine;
using UnityEngine.AI;

public class WaitBehaviour : StateMachineBehaviour
{
    private NavMeshAgent Agent;
    private Transform Player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent = animator.GetComponent<NavMeshAgent>();
        Agent.isStopped = true;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Heart.transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(animator.transform.position, Player.position);
        Vector3 pos = Player.transform.position;
        pos.y = Agent.transform.position.y;
        Agent.transform.LookAt(pos);
        if (distance > 2f)
        {
            animator.SetBool("Wait", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent.isStopped = false;
    }
}
