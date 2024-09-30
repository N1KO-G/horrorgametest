using UnityEngine;

public class EnemyScript : MonoBehaviour
{

public float health = 100;
public float maxhealth = 100;

public void Awake ()
{
    health = maxhealth;
}

public void Update()
{
    if (health <= 0)
    {
        Destroy(this.gameObject);
    }

    Debug.Log("Enemy health is " + health);

}

public void TakeDamage(float damage)
{
    health -= damage;
}


}
