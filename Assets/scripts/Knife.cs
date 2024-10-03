using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [SerializeField]
     public Transform stabpoint;
    [SerializeField]
    EnemyScript enemyScript;




    Animator animator;


 void Awake()
 {
    animator = GetComponent<Animator>();
    enemyScript = GetComponent<EnemyScript>();
 }




 public void Stab()
 {
    animator.Play("stab");
    animator.SetBool("isStabbing",true);

    Ray ray = new Ray(stabpoint.position, stabpoint.forward);
    RaycastHit hit;
    
    if (Physics.Raycast(stabpoint.transform.position,stabpoint.transform.forward, out hit, 5))
    {

        if (hit.transform.TryGetComponent<EnemyScript> ( out var enemyScript))
        {
            enemyScript.TakeDamage(10);
        }

        Debug.DrawLine(stabpoint.position, hit.point, Color.green);
    }

    



    animator.Play("stab", 0,0);
    animator.SetBool("isStabbing",false);

 }
}
