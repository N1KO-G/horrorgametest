using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class enemyscripttest : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Animator animator;
    public Transform Player;
    public float moveSpeed = 3f;
    public float Maxdistance = 10f;
    public float Mindistance = 1f;

public void Awake()
{
  animator = GetComponent<Animator>();
  navMeshAgent = GetComponent<NavMeshAgent>();
}

public void Update()
{

  transform.LookAt(Player);
  transform.localEulerAngles = new Vector3(0f,transform.localEulerAngles.y, transform.localEulerAngles.z);

  
  if (navMeshAgent.destination == Player.transform.position)
  {

  }

}
}
