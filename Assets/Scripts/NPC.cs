using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public bool Moon = false;
    public AnimationCurve Curve;
    public Animator MeshAnimator;
    private Animator AgentAnimator;
    private PlayerController Player;
    private Vector3 LastPos;
    private bool IsHugged;
    private bool IsHugging;

    private void Start()
    {
        AgentAnimator = GetComponent<Animator>();
        LastPos = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        MeshAnimator.SetFloat("Speed", Vector3.Distance(transform.position, LastPos));
        LastPos = transform.position;
        if (IsHugged && IsHugging)
        {
            Player.AddSanity(Curve.Evaluate(Map(0f, 0f, 5f, 0f, 1f)));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            AgentAnimator.GetBehaviour<WalkBehaviour>().Triggered();
        }
    }

    public void Hug(bool value)
    {
        IsHugging = value;
        AgentAnimator.SetBool("Hug", value);
    }

    public void Hugged(bool value)
    {
        IsHugged = value;
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
