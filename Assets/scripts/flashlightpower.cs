using System;
using UnityEngine;

public class flashlighton : MonoBehaviour
{
    public GameObject flashlight;
    public bool isFlashlightOn;
    public pickupscript pickupscript;

    public void Awake()
    {
        pickupscript.GetComponent<pickupscript>();
    }

    public void Update()
    {
        if (flashlight.activeInHierarchy)
        {
            isFlashlightOn = true;
        }
        else if (!flashlight.activeInHierarchy)
        {
            isFlashlightOn = false;
        }
        
        if (pickupscript.isPickedUp && !isFlashlightOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(true);
        }
        else if (isFlashlightOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(false);    
        }

        //Debug.Log(isFlashlightOn);
        
            
        
    } 

}
