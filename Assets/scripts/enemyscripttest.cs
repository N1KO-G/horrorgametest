using System;
using UnityEngine;
using UnityEngine.AI;

public class enemyscripttest : MonoBehaviour
{

public Transform player;
public NavMeshAgent navMeshAgent;

public void Awake()
{
    navMeshAgent = GetComponent<NavMeshAgent>();
}

public void Update()
{
    navMeshAgent.SetDestination(player.transform.position);
}
}
