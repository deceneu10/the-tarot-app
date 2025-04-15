using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using System;

public class GDPR_v2 : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls the GDPR screen and DB write";

    [Header("Database Dependency")]
    public DB_Initial database;

    [Header("GDPR Elements")]
    public GameObject GDPR_Panel;
    public string GdprDocumentVersion;

    [Header("Retry strategy with exponential backoff")]
    //Variables for Connection Timeout - retry strategy with exponential backoff
    public float initialDelay = 1f;
    public float maxDelay = 4f;
    public float backoffFactor = 1.25f;
    public int maxAttempts = 5;

    private int currentReadAttempt = 0;
    private int currentWriteAttempt = 0;

    [Header("Reset GDPR")]
    public bool GdprFlagReset = false;

    private bool GDPR_Acknoledged;


    // Start is called before the first frame update
    void Start()
    {

        GDPR_Check();

    }


    //--------- GDPR Check --> ALL LOCAL ------------------
    private void GDPR_Check()
    {

        if (PlayerPrefs.GetInt("GDPR", 0) == 0)
        {
            GDPR_Panel.SetActive(true);
            GDPR_Acknoledged = false;

        }
        else
        {
            GDPR_Panel.SetActive(false);
            GDPR_Acknoledged = true;

        }
    }

    public void GDPR_Accept()
    {

        PlayerPrefs.SetInt("GDPR", 1);

        GDPR_Check();
        check_GDPR_DB();

    }

    //-------------------------------------------------------------------------------




    //--------------- Update used to reset the GDPR Player Prefs --------------------
    void Update()
    {
        if(GdprFlagReset == true)
        {

            PlayerPrefs.SetInt("GDPR", 0);
            GdprFlagReset = false;

            Debug.Log("[GDPR](local): GDPR flag is set to 0 --> You should see the GDPR form at next run");

        }
    }

    //-------------------------------------------------------------------------------




    //-------------- Write the GDPR Record to the DB so that we have an account of it --------------------


    public void check_GDPR_DB()
    {
        if (GDPR_Acknoledged == true)
        {

            database.Check_Login_Status();


            if (database.DB_Operational == false)
            {

                retry_DB_read();

            }
            else
            {

                GDPR_DB_Check();

            }

        }
        else
        {
            //Do nothing and await the user to trigger the class by accepting the GDPR terms and conditions
        }

    }


    public void GDPR_DB_Check()
    {
    
            var gdpr_result = UniRESTClient.Async.ReadOne<DB.Tta_gdpr>
                 (API.thetarotapp_gdpr,
                 new DB.Tta_gdpr
                 {

                     username = database.autoUserName

                 }, (DB.Tta_gdpr gdpr_result, bool ok) => {
                     if (ok)
                     {
                         //Do nothing as there is a record in the GDPR table
                         Debug.Log("[GDPR](DB): GDPR Record FOUND in the DB");
                     }
                     else
                     {
                        //Write the GDPR record to the DB table
                        GDPR_DB_Write();
                     }
                 });

    }

    private void GDPR_DB_Write()
    {
        var gdpr_write = UniRESTClient.Async.Write(
            API.thetarotapp_gdpr,
            new DB.Tta_gdpr
            {
                username = database.autoUserName,
                acceptanceDate = DateTime.Now.ToString("dd-MM-yyyy"),
                documentVersion = GdprDocumentVersion

            },
        (bool ok) =>
        {
            if (ok)
            {
                Debug.Log("[GDPR](DB): GDPR Record succesfully written to the DB");
            }
            else
            {
                Debug.LogWarning("[GDPR](DB): GDPR Record was NOT written to the DB --> ... Retrying ...");
                StartCoroutine(retry_DB_write());
            }
        });
    }


    IEnumerator retry_DB_read()
    {
        float delay = Mathf.Min(Mathf.Pow(backoffFactor, currentReadAttempt) * initialDelay, maxDelay);
        yield return new WaitForSeconds(delay);


        if (currentReadAttempt < maxAttempts)
        {
            check_GDPR_DB();
            Debug.LogWarning("[GDPR](DB): READ --> Attempting to READ from the database (Attempt: " + (currentReadAttempt + 1) + ")");
        }
        else
        {
            Debug.LogError("[GDPR](DB): READ --> Failed to READ from the database after " + maxAttempts + " attempts.");
            //You are not online - retry later - logic should be here if testing sais it to be
        }

        currentReadAttempt++;
    }

    IEnumerator retry_DB_write()
    {
        float delay = Mathf.Min(Mathf.Pow(backoffFactor, currentWriteAttempt) * initialDelay, maxDelay);
        yield return new WaitForSeconds(delay);


        if (currentWriteAttempt < maxAttempts)
        {
            GDPR_DB_Write();
            Debug.LogWarning("[GDPR](DB): WRITE --> Attempting to WRITE to the database (Attempt: " + (currentWriteAttempt + 1) + ")");
        }
        else
        {
            Debug.LogError("[GDPR](DB): WRITE --> Failed to WRITE to the database after " + maxAttempts + " attempts.");
            //You are not online - retry later - logic should be here if testing sais it to be
        }

        currentWriteAttempt++;
    }



}
