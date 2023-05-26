using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using System;

public class GDPR : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls the GDPR screen and DB write";

    [Header("GDPR Elements")]
    public GameObject GDPR_Panel;
    public string GdprDocumentVersion;

    [Header("Reset GDPR")]
    public bool GdprFlagReset = false;


    // Start is called before the first frame update
    void Start()
    {
        if(GdprFlagReset == true)
        {
            PlayerPrefs.SetInt("GDPR", 0);
        }


        StartCoroutine(GDPR_Flow());
    }


    private IEnumerator GDPR_Flow()
    {

        yield return new WaitForSeconds(2f);

        GDPR_Check();

    }


    private void GDPR_Check()
    {
       var gdpr_result = UniRESTClient.Async.ReadOne<DB.Tta_gdpr>
            (API.thetarotapp_gdpr,
            new DB.Tta_gdpr
                {

                    username = "TTA_" + SystemInfo.deviceUniqueIdentifier

                }, (DB.Tta_gdpr gdpr_result, bool ok) => {
                    if (ok)
                        {
                            GDPR_Panel.SetActive(false);
                            Debug.Log("record found in GDPR table");
                            PlayerPrefs.SetInt("GDPR", 1);
                    }
                    else
                        {
                            if(PlayerPrefs.GetInt("GDPR", 0) == 0)
                                {
                                    GDPR_Panel.SetActive(true);
                                    StartCoroutine(retry_GDPR());
                                }
                            else
                                {
                                    GDPR_Panel.SetActive(false);
                                    Debug.Log("GDPR was signed and it is acknoledged locally");
                                }
                            
                    }
                });
    }

    private IEnumerator retry_GDPR()
    {

        yield return new WaitForSeconds(1f);
        GDPR_Check();

    }


    public void GDPR_Accept() {
        var gdpr_write = UniRESTClient.Async.Write(
            API.thetarotapp_gdpr,
            new DB.Tta_gdpr
                {
                    username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                    acceptanceDate = DateTime.Now.ToString("dd-MM-yyyy"),
                    documentVersion = GdprDocumentVersion

            },
        (bool ok) =>
            {
            if (ok)
                {
                    GDPR_Check();
                }
            else
                {
                    StartCoroutine(retry_GDPR());
                }});
    }
        



}
