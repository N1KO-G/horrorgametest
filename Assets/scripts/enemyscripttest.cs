using System;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class enemyscripttest : MonoBehaviour
{
    public Transform Player;
    public float moveSpeed = 3f;
    public float Maxdistance = 10f;
    public float Mindistance = 10f;

public void Awake()
{
 
}

public void Update()
{

  transform.LookAt(Player);
  transform.localEulerAngles = new Vector3(0f,transform.localEulerAngles.y, transform.localEulerAngles.z);


  if(Vector3.Distance(transform.position, Player.position) >= Mindistance)
  {
    transform.position += transform.forward * moveSpeed * Time.deltaTime;


    if(Vector3.Distance(transform.position, Player.position) >= Maxdistance)
    {
       
    }
  }
}
}
