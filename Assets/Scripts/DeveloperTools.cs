using System.Collections;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class DeveloperTools : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Developer Tools in the Debug Panel";

    [Header("Database script dependency")]
    public DB_Initial database;

    [Header("Developer Tools elements")]
    public TextMeshProUGUI Parameter1;
    public TextMeshProUGUI Parameter2;
    public TextMeshProUGUI Parameter3;

    public TextMeshProUGUI Debug1;
    public TextMeshProUGUI Debug2;
    public TextMeshProUGUI Debug3;
    public TextMeshProUGUI Debug4;

    public Image Debug1_img;
    public Image Debug2_img;
    public Image Debug3_img;
    public Image Debug4_img;

    public Sprite img_red;
    public Sprite img_green;
    public Sprite img_yellow;

    [Header("Panels and Buttons")]
    public GameObject btn_SuperUser;
    public GameObject btn_Debug;
    public GameObject panel_Debug;
    public GameObject panel_MainSettings;


    private int clickCount;
    private bool pingLoop;


    // Start is called before the first frame update
    void Start()
    {
        btn_Debug.SetActive(false);
        panel_Debug.SetActive(false);

        clickCount = 0;
        pingLoop = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void clickCounter()
    {

        clickCount++;

        if(clickCount >= 4)
        {
            btn_Debug.SetActive(true);
        }

    }

    public void ResetClickCounter_debug()
    {
        clickCount = 0;
    }


    public void debugPanel_show()
    {
        pingLoop = true;
        getSystemInfo();


        panel_MainSettings.SetActive(false);
        panel_Debug.SetActive(true);

        Debug1.text = "PING to begin testing";
        Debug1_img.sprite = img_red;
        Debug2.text ="- ms";
        Debug2_img.sprite = img_red;
        Debug3.text = "-";
        Debug3_img.sprite = img_red;
        Debug4.text = "-";
        Debug4_img.sprite = img_red;

    }

    public void debugPanel_hide()
    {
        pingLoop = false;

        ResetClickCounter_debug();
        StopCoroutine(PingUpdate());

        panel_MainSettings.SetActive(true);
        panel_Debug.SetActive(false);
        btn_Debug.SetActive(false);

        

    }


    //------------------ Elements for the Debug ------------------

    private void getSystemInfo()
    {
        Parameter1.text = SystemInfo.deviceModel.ToString();
        Parameter2.text = SystemInfo.operatingSystem;
        Parameter3.text = database.autoUserName;
    }


    public void PING()
    {

        StartCoroutine(PingUpdate());

    }


#if !UNITY_WEBGL
    // Code block or method using PING

    System.Collections.IEnumerator PingUpdate()
    {
    RestartLoop:

        database.Check_Login_Status();
        var ping = new Ping("89.42.218.231");

        yield return new WaitForSeconds(1f);

        while (!ping.isDone) yield return null;
        

        Debug2.text = ping.time + " ms";
        Debug1.text = "Online";
        Debug1_img.sprite = img_green;

        if (ping.time <= 50 && ping.time > -1)
        {
            Debug2_img.sprite = img_green;
        }
        else if (ping.time > 50 && ping.time < 150)
        {
            Debug2_img.sprite = img_yellow;
        }
        else if (ping.time >= 150)
        {
            Debug2_img.sprite = img_red;
        }
        else if (ping.time == -1)
        {
            Debug1.text = "Offline";
            Debug1_img.sprite = img_red;
            Debug2_img.sprite = img_red;

        }

        if (database.loggedIN == true)
        {
            Debug3.text = "LOGGED IN!";
            Debug3_img.sprite = img_green;
        } else
        {
            Debug3.text = "NOT logged in!";
            Debug3_img.sprite = img_red;
        }
        
        if(database.DB_Operational == true)
        {
            Debug4.text = "ACTIVE";
            Debug4_img.sprite = img_green;
        }
        else
        {
            Debug4.text = "DOWN";
            Debug4_img.sprite = img_red;
        }


        if (pingLoop == true)
        {
            goto RestartLoop;

        }
        else
        {
            yield return null;
        }


    }

#endif


#if UNITY_WEBGL
    IEnumerator PingUpdate()
    {
        // Create a new WWW object with the target URL
        WWW www = new WWW("https://zalmoxeland.ro/");
        database.Check_Login_Status();

        // Get the start time
        float startTime = Time.time;

        // Wait until the request is complete
        yield return www;

        // Get the end time
        float endTime = Time.time;

        // Calculate the response time
        float responseTime = endTime - startTime;
        int pingTime = (int)Math.Round(responseTime*100.0f);



        // Check if there was an error
        if (string.IsNullOrEmpty(www.error))
        {

            Debug2.text = pingTime + " ms";
            Debug1.text = "Online";
            Debug1_img.sprite = img_green;

            if (pingTime <= 50)
            {
                Debug2_img.sprite = img_green;
            }
            else if (pingTime > 50 && pingTime < 150)
            {
                Debug2_img.sprite = img_yellow;
            }
            else if (pingTime >= 150)
            {
                Debug2_img.sprite = img_red;
            }

            if (database.loggedIN == true)
            {
                Debug3.text = "LOGGED IN!";
                Debug3_img.sprite = img_green;
            }
            else
            {
                Debug3.text = "NOT logged in!";
                Debug3_img.sprite = img_red;
            }

            if (database.DB_Operational == true)
            {
                Debug4.text = "ACTIVE";
                Debug4_img.sprite = img_green;
            }
            else
            {
                Debug4.text = "DOWN";
                Debug4_img.sprite = img_red;
            }

        }
        else
        {
            Debug1.text = "OFFLINE";
            Debug2.text = "- ms";
            Debug3.text = "-";
            Debug4.text = "-";
            Debug1_img.sprite = img_red;
            Debug2_img.sprite = img_red;
            Debug3_img.sprite = img_red;
            Debug4_img.sprite = img_red;


        }


    }

#endif

}
