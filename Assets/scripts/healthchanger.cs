
using UnityEngine;
using UnityEngine.UI;

public class healthchanger : MonoBehaviour
{
  
  [SerializeField]
  healthmanager healthmanager;
  [SerializeField]
  public Image healthbar;


  void Update()
  {
    healthbar.fillAmount = healthmanager.health / 100;
  }
}
