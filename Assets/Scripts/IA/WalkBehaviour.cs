using UnityEngine;
using UnityEngine.AI;

public class WalkBehaviour : StateMachineBehaviour
{
    private NavMeshAgent Agent;
    private Transform Player;
    private Vector3[] Waypoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Agent = animator.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] wps = GameObject.FindGameObjectsWithTag("Waypoints");
        Waypoints = new Vector3[wps.Length];
        for (int i = 0; i < wps.Length; ++i)
        {
            Waypoints[i] = wps[i].transform.position;
        }

        int pos = Random.Range(0, Waypoints.Length);
        Agent.SetDestination(Waypoints[pos]);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(animator.transform.position, Player.position);
        if (distance < 2f)
        {
            animator.SetBool("Wait", true);
        }
    }

    public void Triggered()
    {
        int pos = Random.Range(0, Waypoints.Length);
        Agent.SetDestination(Waypoints[pos]);
    }
}
