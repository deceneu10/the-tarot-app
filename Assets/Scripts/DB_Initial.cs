using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class DB_Initial : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls the Autologin or Register/Login";

    [Header("Other DB Scripts that need to run after succesful Login")]
    public GDPR_v2 gdpr;
    public AppVersion appVersion;
    public Notification_Manager notificationManager;
    public Ads_Manager adsManager;
    public Analytics analytics;

    [Header("Variables used by system - DO NOT CHANGE!")]
    public string autoUserName;
    public string autoPassword;
    private string tempUser;

    public bool loggedIN = false;
    public bool DB_Operational = false;

    [Header("Retry strategy with exponential backoff")]
    //Variables for Connection Timeout - retry strategy with exponential backoff
    public float initialDelay = 1f;
    public float maxDelay = 6f;
    public float backoffFactor = 1.4f;
    public int maxAttempts = 5;
    public float timoutRetryTime = 60f;

    private int currentLoginAttempt = 0;
    private int currentRelogAttempt = 0;


    void Awake()
    {

        UUID_generator();

    }


    //
    void UUID_generator(){

        tempUser = SystemInfo.deviceUniqueIdentifier;

        if (tempUser.Length < 5)
        {
            autoUserName = "TTA_" + System.Guid.NewGuid().ToString();
            autoPassword = System.Guid.NewGuid().ToString();
        } else
        {
            autoUserName = "TTA_" + SystemInfo.deviceUniqueIdentifier;
            autoPassword = SystemInfo.deviceUniqueIdentifier;
        }

        AutoLogin();
    }


    // ------------- AUTOMATIC LOGIN --------------------
    void AutoLogin()
    {
        var login_attempt = UniRESTClient.Async.Login(autoUserName, autoPassword, (bool ok) =>
        {
            if (ok)
            {

                Debug.Log("[Authentification](DB): Login SUCCESSFUL!");
                loggedIN = true;
                gdpr.check_GDPR_DB();
                appVersion.CheckAppVersion();

#if !UNITY_WEBGL
                StartCoroutine(notificationManager.PushNotificationFlow());
#endif

                StartCoroutine(adsManager.Check_MasterAdsController());
                StartCoroutine(analytics.sendAnalyticsChecker());
                StartCoroutine(analytics.deviceAnalytics());

            }
            else
            {
                var result = UniRESTClient.Async.Registration(autoUserName, autoPassword, (bool ok) =>
                {
                    if (ok)
                    {
                        Debug.Log("[Authentification](DB): Registration SUCCESSFUL!");
                        AutoLogin();
                    }
                    else
                    {
                        Debug.LogWarning("[Authentification](DB): Registration FAILED!");
                        StartCoroutine(retry_login());
                    }
                });
            }
        });
    }



    IEnumerator retry_login() 
    {
        float delay = Mathf.Min(Mathf.Pow(backoffFactor, currentLoginAttempt) * initialDelay, maxDelay);
        yield return new WaitForSeconds(delay);


        if (currentLoginAttempt < maxAttempts)
        {
            AutoLogin();
            Debug.LogWarning("[Authentification](DB): Attempting to connect to the database (Attempt: " + (currentLoginAttempt + 1) + ")");
        } else
        {
            Debug.LogError("[Authentification](DB): Failed to connect to the database via Autologin after " + maxAttempts + " attempts.");
            StartCoroutine(retry_login_late());
        }

        currentLoginAttempt++;
    }
    IEnumerator retry_login_late()
    {
        currentLoginAttempt = 0;
        yield return new WaitForSeconds(timoutRetryTime);
        StartCoroutine(retry_login());
    }




    // ------------- END auto-login --------------------



    // ------------- Check session login state --------------------
    public void Check_Login_Status()
    {
        if(loggedIN == true)
        {

            if (UniRESTClient.isLoggedIn)
            {

                DB_Operational = true;
                Debug.Log("[Authentification](DB): DB ready for use!");

            }
            else
            {

                DB_Operational = false;
                Debug.LogWarning("[Authentification](DB): Session disconnected! Attempting to Relog ...");

                var relogin_attempt = UniRESTClient.Async.Relogin((bool ok) =>
                    {
                        if (ok)
                        {

                            DB_Operational = true;
                            Debug.Log("[Authentification](DB): Session succesfuly restored!");

                        }
                        else
                        {

                            Debug.LogWarning("[Authentification](DB): Relog attempt FAILED! Attempting to Relog ...");
                            StartCoroutine(retry_RElogin());

                        }
                    });

            }
        } 
        else
        {
            //If the user is not logged in the Autologin will be triggered
            AutoLogin();

        }

    }


    IEnumerator retry_RElogin() 
    {
        float delay = Mathf.Min(Mathf.Pow(backoffFactor, currentRelogAttempt) * initialDelay, maxDelay);
        yield return new WaitForSeconds(delay);


        if (currentRelogAttempt < maxAttempts)
        {
            Check_Login_Status(); 
            Debug.LogWarning("[Authentification](DB): Attempting to re-connect to the database (Attempt: " + (currentRelogAttempt + 1) + ")");
        }
        else
        {
            Debug.LogError("[Authentification](DB): Failed to re-connect to the database via Relogin after " + maxAttempts + " attempts.");
            StartCoroutine(retry_RElogin_late());
        }

        currentRelogAttempt++;
    }

    IEnumerator retry_RElogin_late()
    {
        currentRelogAttempt = 0;
        yield return new WaitForSeconds(timoutRetryTime);
        StartCoroutine(retry_RElogin());
    }


}
