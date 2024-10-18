using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class flashlightscript : MonoBehaviour
{

    public GameObject flashlight;

    public GameObject player;

    public bool isPickedUp = false;

    public bool isInZone = false;
    
    public void Update()
    {
        if (isInZone && Input.GetKey(KeyCode.E))
        {
            flashlight.SetActive(flashlight);
            isPickedUp = true;
        }
        
        
        
       
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           isInZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        isInZone = false;
    }
}
