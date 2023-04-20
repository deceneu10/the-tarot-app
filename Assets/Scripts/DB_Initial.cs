using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class DB_Initial : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls the Autologin or Register/Login";


    private string autoUserName;
    private string autoPassword;

    // Start is called before the first frame update
    void Start()
    {
        autoUserName = "TTA_" + SystemInfo.deviceUniqueIdentifier;
        autoPassword = SystemInfo.deviceUniqueIdentifier;

        AutoLogin();
    }



    // ------------- AUTOMATIC LOGIN --------------------
    void AutoLogin()
    {
        StartCoroutine(AutoLogin_delay());
    }

    private IEnumerator AutoLogin_delay() //Make a tiny delay before attempting to autologin so that internet can be establised
    {
        yield return new WaitForSeconds(1.5f);

        var login_attempt = UniRESTClient.Async.Login(autoUserName, autoPassword, (bool ok) =>
        {
            if (ok)
            {

                Debug.Log("Login done!");


            }
            else
            {
                var result = UniRESTClient.Async.Registration(autoUserName, autoPassword, (bool ok) =>
                {
                    if (ok)
                    {
                        Debug.Log("Registration done!");
                        AutoLogin();
                    }
                    else
                    {
                        Debug.Log("Registration failed!");
                        StartCoroutine(retry_login());
                    }
                });
            }
        });
    }

    private IEnumerator retry_login()
    {
        yield return new WaitForSeconds(2.5f);
        AutoLogin();
    }

    // ------------- END auto-login --------------------



}
