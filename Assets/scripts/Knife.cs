using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [SerializeField]
     public Transform stabpoint;
    [SerializeField]
     private Knife knife;

     [SerializeField]
     public float knifecooldown = 0.5f;
     public float knifetimebeforelasthit = 0f;
    Animator animator;


private void Update()
  {
    if (Input.GetMouseButtonDown(0) && knife.isActiveAndEnabled)
    {
      knife.Stab();
    }

    knifetimebeforelasthit += Time.deltaTime;

  }

 void Awake()
 {
    animator = GetComponent<Animator>();
 }




 public void Stab()
 {
   

   
   if(knifetimebeforelasthit > knifecooldown)
   {
    animator.Play("stab");
    animator.SetBool("isStabbing",true);
    knifetimebeforelasthit = 0;

   Collider[] DamageColliders = Physics.OverlapSphere(stabpoint.position, 0.125f/2);      
    foreach(Collider coll in DamageColliders)
    {
        if(coll.TryGetComponent<EnemyScript>(out var enemyscript))
        {
           enemyscript.TakeDamage(10); 
        }        
    }



    knifetimebeforelasthit = 0;

    animator.Play("stab", 0,0);
    animator.SetBool("isStabbing",false);
   }

  
 }
}
