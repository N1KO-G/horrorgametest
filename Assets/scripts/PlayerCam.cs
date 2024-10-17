using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCam : MonoBehaviour
{
    

    public Transform orientation;   
    float yRotation; 

    private void Update()
    {
        
        
        orientation.rotation =  Quaternion.Euler(0, yRotation, 0);
        
    }


}
