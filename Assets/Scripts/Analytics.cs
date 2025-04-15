using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BansheeGz.BGDatabase;
using UnityEngine.EventSystems;
using System;
using TigerForge;

public class Analytics : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "All my custom Analytics are here!";

    [Header("Database Dependency")]
    public DB_Initial database;

    private int dateIntToday;


    // Start is called before the first frame update
    void Start()
    {

        dateIntToday = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

        Load();

    }




    private static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "save4.dat"); }
    }


    //this method saves current database state to a file save.dat
    public static void Save()
    {
        var filePath = FilePath;
        File.WriteAllBytes(filePath, BGRepo.I.Addons.Get<BGAddonSaveLoad>().Save());

    }

    //this method loads database state from a file save.dat
    public static void Load()
    {
        var filePath = FilePath;
        if (File.Exists(filePath))
        {
            BGRepo.I.Addons.Get<BGAddonSaveLoad>().Load(File.ReadAllBytes(filePath));

        }
    }


    // --------------- Button Click Analytics --------------- 
    public void registerClicks() //Class that can be put on buttons to register clicks count
    {

        AnalyticsButtons analyticsButton = AnalyticsButtons.FindEntity(entity => entity.buttonTag == EventSystem.current.currentSelectedGameObject.tag);

        if (analyticsButton != null)
        {

            analyticsButton.clicks++;
            analyticsButton.dateInt = dateIntToday;


        }
        else
        {
            AnalyticsButtons newRow = AnalyticsButtons.NewEntity();

            newRow.buttonTag = EventSystem.current.currentSelectedGameObject.tag;
            newRow.dateInt = dateIntToday;
            newRow.clicks = +1;

        }

        Save();


    }
    public IEnumerator sendAnalyticsChecker()
    {
        yield return new WaitForSeconds(2.75f);

        AnalyticsButtons analyticsButtonDateCheck = AnalyticsButtons.FindEntity(entity => entity.dateInt < dateIntToday);

        if (analyticsButtonDateCheck != null)
        {
            List<AnalyticsButtons> result = AnalyticsButtons.FindEntities(entity => entity.dateInt < dateIntToday);

            //Write these to the db and reset the clicks to 0
            for (int i = 0; i < result.Count; i++)
            {
                AnalyticsButtons firstRow = AnalyticsButtons.GetEntity(i);

                var analyticsDB = UniRESTClient.Async.Update(
                    API.thetarotapp_analyticsButtons,
                    new DB.Tta_AnalyticsButtons
                    {
                        username = database.autoUserName,
                        buttonTag = firstRow.buttonTag,
                        clicks = firstRow.clicks,
                        dateInt = firstRow.dateInt

                    },
                (bool ok) =>
                        {

                            firstRow.clicks = 0;
                            firstRow.dateInt = dateIntToday;

                            Save();

                        });
            }
        }
        else
        {
            //Do nothing as the buttons are up-to-date
        }

    }


    // --------------- Device Analytics --------------- 


    public IEnumerator deviceAnalytics()
    {
        yield return new WaitForSeconds(3.75f);

        var analiticsCheck = UniRESTClient.Async.ReadOne<DB.Tta_AnalyticsDevice>
                (API.thetarotapp_analyticsDevice,
                    new DB.Tta_AnalyticsDevice
                    {

                        username = database.autoUserName

                    }, (DB.Tta_AnalyticsDevice analiticsCheck, bool ok) => {
                    if (ok)
                    {
                        //Do nothing as there is a record in the table
                    }
                     else
                    {
                            //Write the code
                            writeDeviceAnalytics();

                    }
                    });
    }


    private void writeDeviceAnalytics()
    {
        var result = UniRESTClient.Async.Write(
            API.thetarotapp_analyticsDevice,
            new DB.Tta_AnalyticsDevice
                {
                    username = database.autoUserName,
                    deviceType = SystemInfo.deviceType.ToString(),
                    deviceModel = SystemInfo.deviceModel,
                    deviceOS = SystemInfo.operatingSystem,
                    systemLanguage = Application.systemLanguage.ToString(),
                    appVersion = Application.version,
                    date = DateTime.Now.ToString("dd-MM-yyyy")

            },
                (bool ok) =>
                    {
                        if (ok)
                            {
                                
                            }
                        else
                            {
                
                            }});

    }



}