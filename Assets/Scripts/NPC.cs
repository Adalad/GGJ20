using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public bool Moon = false;
    public AnimationCurve Curve;
    public Animator MeshAnimator;
    public GameObject Shield;
    public float SanityRate = 1f;
    public float HugTime = 10f;
    private Animator AgentAnimator;
    private PlayerController Player;
    private Vector3 LastPos;
    private bool IsHugged;
    private bool IsHugging;
    private float StartTime;

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
            Player.AddSanity(Curve.Evaluate(Map(Time.time - StartTime, 0f, HugTime, 0f, 1f)) * Time.deltaTime * SanityRate);
            Debug.Log(Time.time - StartTime);
            //if ((Time.time - StartTime) > HugTime)
            //{
            //    if (Moon)
            //    {
            //        SceneController.Instance.FadeAndLoadScene("Ending");
            //    }
            //    else
            //    {
            //        IsHugging = false;
            //        Hugged(false);
            //    }
            //}
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
    }

    public void Hugged(bool value)
    {
        IsHugged = value;
        IsHugging = value;
        AgentAnimator.SetBool("Hug", value);
        MeshAnimator.SetBool("Hugging", value);
        StartTime = Time.time;
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
