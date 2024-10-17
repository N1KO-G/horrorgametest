using UnityEngine;

public class ItemsPickedUp : MonoBehaviour
{
     public GameObject item;
   
    public pickupscript pickupscript;
    public WeaponSwitching weaponSwitching;
    public Gun gun;

    public bool ItemPickedUp;

    public GameObject weaponholder;





    public void Awake()
    {
        pickupscript.GetComponent<pickupscript>();
        gun = GetComponent<Gun>();
        weaponSwitching = FindAnyObjectByType<WeaponSwitching>();
    }

    public void Update()
    {   
        if (pickupscript.isPickedUp)
        {
            item.transform.SetParent(weaponholder.transform, false);
            item.transform.localPosition = new Vector3(0.32f, -0.267f, 0.284f);
            weaponSwitching.selectedweapon = 1;
            
            
        }
        else if (pickupscript.isPickedUp && gun.isShotgun)
        {
            item.transform.SetParent(weaponholder.transform, false);
            item.transform.localPosition = new Vector3(0.312f, -0.254f, 0.204f);
            weaponSwitching.selectedweapon = 2;   
        }
}

}
