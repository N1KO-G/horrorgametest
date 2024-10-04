
using UnityEngine;
using UnityEngine.UI;

public class staminamanager : MonoBehaviour
{
   [SerializeField]
   movement movement;
   [SerializeField]
   public Image image;

   public void Update()
   {
        image.fillAmount = movement.stamina / 100;
   }
}
