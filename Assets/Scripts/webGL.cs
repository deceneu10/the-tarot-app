using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BansheeGz.BGDatabase;
using UnityEngine.EventSystems;
using System;
using TigerForge;
using UnityEngine.UI;
using TMPro;


public class webGL : MonoBehaviour
{

    [Header("Script Description")]
    public string ScriptDescription = "Script Used only for WebGL";

    [Header("Dependency")]
    public GameObject webGL_panel;

    private string deviceOS;

    // Start is called before the first frame update
    void Awake()
    {

      //  deviceChecker();

    }

    private void deviceChecker()
    {
        deviceOS = SystemInfo.operatingSystem;

        if (deviceOS.Contains("android", System.StringComparison.CurrentCultureIgnoreCase))
        {

            webGL_panel.SetActive(true);

        } 
        else
        {
            
            //do nothing

        }
    }


    public void redirectAndroid()
    {

        Application.OpenURL("https://zalmoxeland.ro/tta_redirect_to_android");

    }

    



}
