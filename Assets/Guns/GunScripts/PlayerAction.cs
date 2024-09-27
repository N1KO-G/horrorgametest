using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && GunSelector.ActiveGun != null)
    {
        GunSelector.ActiveGun.Shoot();
    }
  }
}
