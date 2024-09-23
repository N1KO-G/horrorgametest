using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightscript : MonoBehaviour
{

    public GameObject flashlight;

    public void pickupflashlight()
    {

        flashlight.SetActive(false);


        flashlight.SetActive(true);
    }
}
