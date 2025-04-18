using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using System;
using TMPro;


public class AppVersion : MonoBehaviour
{

    [Header("Script Description")]
    public string ScriptDescription = "Controls the App Version check";

    [Header("Elements")]
    public GameObject AppVersion_Panel;

    [Header("App Elements that need app versioning updated")]
    public TextMeshProUGUI settingAppVersionLabel;

    private float currentAppVersion;
    private float DBAppVersion;



    // Start is called before the first frame update
    void Start()
    {

        currentAppVersion = float.Parse(Application.version);

        settingAppVersionLabel.text = "Aplicatia de Tarot v" + Application.version + " <sprite=0> ";

    }


    public void CheckAppVersion()
    {
        var appVer_result = UniRESTClient.Async.ReadOne<DB.Tta_appVersion>
           (API.thetarotapp_appVersion,
           new DB.Tta_appVersion
           {


           }, (DB.Tta_appVersion appVer_result, bool ok) => {
               if (ok)
               {

                   DBAppVersion = float.Parse(appVer_result.appVersion);

                   CompareAppVersions();

               }
               else
               {

               }
           });
    }

    private void CompareAppVersions()
    {
        if(DBAppVersion > currentAppVersion)
        {

            AppVersion_Panel.SetActive(true);
            Debug.Log("[AppVersion](DB): App Version outdated! The version " + DBAppVersion + " is available!");
        } 
        else
        {

            AppVersion_Panel.SetActive(false);
            Debug.Log("[AppVersion](DB): App Version is up to date!");
        }
    }


    public void UpdateApp()
    {

        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZalmoxeLand.TheTarotApp");
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZalmoxeLand.TheTarotApp");
#elif UNITY_IPHONE
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZalmoxeLand.TheTarotApp");
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ZalmoxeLand.TheTarotApp");
#endif


    }

    public void CloseAppVersionPanel()
    {

        AppVersion_Panel.SetActive(false);

    }



}
