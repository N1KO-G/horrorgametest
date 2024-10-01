using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private Gun Gun;

    private Animator animator;
  

  public void Awake()
  {
    animator = GetComponent<Animator>();
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
        Gun.Shoot();
    }
  }
}
