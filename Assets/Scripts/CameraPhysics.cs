using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using kha2dev.DatePicker;
using System;
using System.Globalization;
using UnityEngine.EventSystems;

public class CameraPhysics : MonoBehaviour
{
    public string hitCardName;
    public string hitCardTag;
    public GameObject selectedCard;
    // Start is called before the first frame update

    void Start()
    {
        hitCardName = "";
        hitCardTag = "";
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                hitCardName = hit.transform.name;
                hitCardTag = hit.transform.tag;

                selectedCard = hit.transform.gameObject;
            }
        }


    }

    public void hitting()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Name of object: " + name);
    }


}
