using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Rotation : MonoBehaviour
{
    public float rotationSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
