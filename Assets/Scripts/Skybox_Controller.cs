using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox_Controller : MonoBehaviour
{
    public float skyboxRotationSpeed = 1.2f;
    void Update()
    {
        
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotationSpeed);  


    }
}
