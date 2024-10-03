using UnityEngine;
using UnityEngine.SceneManagement;

public class healthmanager : MonoBehaviour
{

[SerializeField]
public float health = 100;
[SerializeField]
public float maxhealth = 100;


public void Awake()
{
    health = maxhealth;
}

public void Update()
{
    if (Input.GetKeyDown(KeyCode.T))
    {
        TakeDamage(10);
    }

    if(health <= 0)
    {
        SceneManager.LoadScene(0);
    }
}

public void TakeDamage(float damage)
{
    health -= damage;
}

}
