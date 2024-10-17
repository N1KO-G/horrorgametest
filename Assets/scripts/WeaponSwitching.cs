using System.Globalization;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedweapon = 0;
    public pickupscript pickupscript;
    // Start is called before the first frame update
    void Awake()
    {
        SelectWeapon();
        pickupscript = GetComponent<pickupscript>();
    }


    // Update is called once per frame
    void Update()
    {

        int previousSelectedWeapon = selectedweapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedweapon >= transform.childCount - 1)
            {selectedweapon = 0;}
            else 
            {selectedweapon ++;}
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedweapon <= 0)
                selectedweapon = transform.childCount -1;
                else
                selectedweapon-- ;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)&& transform.childCount >= 1)
        {
            selectedweapon = 0;
        }
         if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedweapon = 1;
        }
         if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedweapon = 2;
        }
        if (previousSelectedWeapon != selectedweapon)
        {
            SelectWeapon();
        }
   
      
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedweapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else{
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

}
