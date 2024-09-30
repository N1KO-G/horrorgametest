using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private Gun Gun;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
        Gun.Shoot();
    }
  }
}
