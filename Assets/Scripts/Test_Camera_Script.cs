using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test_Camera_Script : MonoBehaviour
{
    public float movement_speed = 20f;

    private float i = 9f;
    private bool left = false;
    private bool right = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()

    {
        if ((int)Math.Round(i) == 8)
        {
            left = false;
            right = true;
        }

        if ((int)Math.Round(i) == 80)
        {
            left = true;
            right = false;
        }


        if (left == true && right == false)
        {
            i = i - 0.1f;
            transform.position = new Vector3(i, transform.position.y, transform.position.z);
        }


        if (left == false && right == true)
        {
            i = i + 0.1f;
            transform.position = new Vector3(i, transform.position.y, transform.position.z);
        }

    }
}
