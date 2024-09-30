using System;
using UnityEngine;

public class flashlighton : MonoBehaviour
{
    public GameObject flashlight;
    public bool isFlashlightOn;
    public flashlightscript Flashlightscript;

    public void Awake()
    {
        Flashlightscript.GetComponent<flashlightscript>();
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
        
        if (Flashlightscript.isPickedUp && !isFlashlightOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(true);
        }
        else if (isFlashlightOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(false);    
        }

        Debug.Log(isFlashlightOn);
        
            
        
    } 

}
