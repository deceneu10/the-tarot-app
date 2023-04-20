using System;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using Unity.Notifications.Android;
using UnityEngine;
using Random = UnityEngine.Random;

public class Notification_Manager : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls Notifications within the app";


    private string notificationTitle_dth24;
    private string notificationContent_dth24;
    private string notificationSmallIcon_dth24;
    private string notificationLargeIcon_dth24;
    private int notificationDelay_dth24;

    private bool dth24_setFlag;
    private bool dth24_contentFlag;



    private string notificationTitle_general1;
    private string notificationContent_general1;
    private string notificationSmallIcon_general1;
    private string notificationLargeIcon_general1;
    private int notificationDelay_general1;

    private bool general1_setFlag;
    private bool general1_contentFlag;
    private bool general2_setFlag;
    private bool general2_contentFlag;

    private string notificationTitle_general2;
    private string notificationContent_general2;
    private string notificationSmallIcon_general2;
    private string notificationLargeIcon_general2;
    private int notificationDelay_general2;

    private int analyticsId;
    private string analyticsChannel;
    private string analyticsNotification;

    private string todayDate;

    // Start is called before the first frame update
    void Start()
    {
        todayDate = DateTime.Now.ToString("dd-MM-yyyy");

        StartCoroutine(PushNotificationFlow());

    }




    //Android
    //DTH
    private void android_createNotificationChannel_DTH()
    {
        //DTH
        var channel1 = new AndroidNotificationChannel()
        {
            Id = "DTH24",
            Name = "DailyTarotHoroscope24",
            Importance = Importance.High,
            Description = "Notification that will prompt the user to use the app and see their daily tarot horoscope - set at 24h",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel1);

        var notification_DTH24 = new AndroidNotification();
        notification_DTH24.Title = notificationTitle_dth24;
        notification_DTH24.Text = notificationContent_dth24;
        notification_DTH24.SmallIcon = notificationSmallIcon_dth24;
        notification_DTH24.LargeIcon = notificationLargeIcon_dth24;
        notification_DTH24.FireTime = System.DateTime.Now.AddHours(notificationDelay_dth24);

        notification_DTH24.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";

        AndroidNotificationCenter.SendNotification(notification_DTH24, "DTH24");

        writeNotificationsSchedule_DTH_DB();
    }

    //GENERAL1
    private void android_createNotificationChannel_General1()
    {
        var channel2 = new AndroidNotificationChannel()
        {
            Id = "GENERAL1",
            Name = "GENERAL1",
            Importance = Importance.Default,
            Description = "Notification that will prompt the user to use the app and ask questions 1",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel2);

        var notification_general1 = new AndroidNotification();
        notification_general1.Title = notificationTitle_general1;
        notification_general1.Text = notificationContent_general1;
        notification_general1.SmallIcon = notificationSmallIcon_general1;
        notification_general1.LargeIcon = notificationLargeIcon_general1;
        notification_general1.FireTime = System.DateTime.Now.AddHours(notificationDelay_general1);

        notification_general1.IntentData = "{\"title2\": \"Notification 2\", \"data\": \"200\"}";

        AndroidNotificationCenter.SendNotification(notification_general1, "GENERAL1");

        writeNotificationsSchedule_General1_DB();
    }

    //General2
    private void android_createNotificationChannel_General2()
    {
        var channel3 = new AndroidNotificationChannel()
        {
            Id = "GENERAL2",
            Name = "GENERAL2",
            Importance = Importance.Default,
            Description = "Notification that will prompt the user to use the app and ask questions 2",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel3);

        var notification_general2 = new AndroidNotification();
        notification_general2.Title = notificationTitle_general2;
        notification_general2.Text = notificationContent_general2;
        notification_general2.SmallIcon = notificationSmallIcon_general2;
        notification_general2.LargeIcon = notificationLargeIcon_general2;
        notification_general2.FireTime = System.DateTime.Now.AddHours(notificationDelay_general2);

        notification_general2.IntentData = "{\"title3\": \"Notification 3\", \"data\": \"200\"}";

        AndroidNotificationCenter.SendNotification(notification_general2, "GENERAL2");

        writeNotificationsSchedule_General2_DB();
    }




    void checkNotificationsSchedule_DTH_DB()
    {
        //DTH
        var result_dth24 = UniRESTClient.Async.ReadOne<DB.Tta_notifications_controller>
               (API.thetarotapp_notificationsController,
               new DB.Tta_notifications_controller
               {
                   username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                   notification_id = "DTH24",
                   date = todayDate

               }, (DB.Tta_notifications_controller result_dth24, bool ok) => {
                   if (ok)
                   {
                       Debug.Log("Notification for Daily Tarot Horoscope(24) - is already set(1)");
                       dth24_setFlag = true;

                   }
                   else
                   {

                       Debug.Log("NO-Notification for Daily Tarot Horoscope(24) - Attempting to set it");
                       dth24_setFlag = false;

                   }
               });
    }

    void checkNotificationsSchedule_General1_DB()
    {
        //General 1
        var result_general1 = UniRESTClient.Async.ReadOne<DB.Tta_notifications_controller>
       (API.thetarotapp_notificationsController,
       new DB.Tta_notifications_controller
       {
           username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
           notification_id = "GENERAL1",
           date = todayDate

       }, (DB.Tta_notifications_controller result_general1, bool ok) => {
           if (ok)
           {

               general1_setFlag = true;

           }
           else
           {

               general1_setFlag = false;

           }
       });
    }


    void checkNotificationsSchedule_General2_DB()
    {
        //General 2
        var result_general2 = UniRESTClient.Async.ReadOne<DB.Tta_notifications_controller>
       (API.thetarotapp_notificationsController,
       new DB.Tta_notifications_controller
       {
           username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
           notification_id = "GENERAL2",
           date = todayDate

       }, (DB.Tta_notifications_controller result_general2, bool ok) => {
           if (ok)
           {

               general2_setFlag = true;

           }
           else
           {

               general2_setFlag = false;

           }
       });
    }


    //DTH
    void writeNotificationsSchedule_DTH_DB()
    {
        var result_dth24w = UniRESTClient.Async.Update(
            API.thetarotapp_notificationsController,
            new DB.Tta_notifications_controller
            {
                username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                notification_id = "DTH24",
                date= todayDate

            },
     (bool ok) =>
     {
         Debug.Log("SUCCESS --> Scheduled Notification for Daily Tarot Horoscope(24) in the DB");
     });
    }

    //General1
    void writeNotificationsSchedule_General1_DB()
    {
        var result_General1w = UniRESTClient.Async.Update(
            API.thetarotapp_notificationsController,
            new DB.Tta_notifications_controller
            {
                username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                notification_id = "GENERAL1",
                date = todayDate

            },
(bool ok) =>
{
    Debug.Log("SUCCESS --> Scheduled Notification for General1 in the DB");
});
    }

    //General2
    void writeNotificationsSchedule_General2_DB()
    {
        var result_General2w = UniRESTClient.Async.Update(
           API.thetarotapp_notificationsController,
           new DB.Tta_notifications_controller
           {
               username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
               notification_id = "GENERAL2",
               date = todayDate

           },
(bool ok) =>
{
    Debug.Log("SUCCESS --> Scheduled Notification for General2 in the DB");
});
    }



    //DTH
    void getNotificationsContent_DTH_DB()
    {

        var result_dth24_content = UniRESTClient.Async.ReadOne<DB.Tta_notifications_content>
               (API.thetarotapp_notificationsContent,
               new DB.Tta_notifications_content
               {
                   notification_id= "DTH24",
                   notificationIndex = 1

               }, (DB.Tta_notifications_content result_dth24_content, bool ok) => {
                   if (ok)
                   {
                       Debug.Log("Getting Notification Content ... SUCCESS --> Daily Tarot Horoscope(24) data retrieved!");

                       notificationTitle_dth24 = result_dth24_content.notificationTitle;
                       notificationContent_dth24 = result_dth24_content.notificationContent;
                       notificationSmallIcon_dth24 = result_dth24_content.iconSmall;
                       notificationLargeIcon_dth24 = result_dth24_content.iconLarge;
                       notificationDelay_dth24 = result_dth24_content.delayHours;

                       dth24_contentFlag = true;

                   }
                   else
                   {

                       Debug.Log("ERROR(1) --> CANNOT FIND NOTIFICATION MESSAGE!!!");
                       dth24_contentFlag = false;

                   }
               });
    }

    //General 1
    void getNotificationsContent_General1_DB()
    {
        int random1 = Random.Range(1, 9);

        var result_general1_content = UniRESTClient.Async.ReadOne<DB.Tta_notifications_content>
       (API.thetarotapp_notificationsContent,
       new DB.Tta_notifications_content
       {
           notification_id = "GENERAL1",
           notificationIndex = random1

       }, (DB.Tta_notifications_content result_general1_content, bool ok) => {
           if (ok)
           {

               notificationTitle_general1 = result_general1_content.notificationTitle;
               notificationContent_general1 = result_general1_content.notificationContent;
               notificationSmallIcon_general1 = result_general1_content.iconSmall;
               notificationLargeIcon_general1 = result_general1_content.iconLarge;
               notificationDelay_general1 = result_general1_content.delayHours;

               general1_contentFlag = true;

           }
           else
           {

               general1_contentFlag = false;

           }
       });
    }

    //General 2
    void getNotificationsContent_General2_DB()
    {
        int random2 = Random.Range(1, 9);

        var result_general2_content = UniRESTClient.Async.ReadOne<DB.Tta_notifications_content>
       (API.thetarotapp_notificationsContent,
       new DB.Tta_notifications_content
       {
           notification_id = "GENERAL2",
           notificationIndex = random2

       }, (DB.Tta_notifications_content result_general2_content, bool ok) => {
           if (ok)
           {

               notificationTitle_general2 = result_general2_content.notificationTitle;
               notificationContent_general2 = result_general2_content.notificationContent;
               notificationSmallIcon_general2 = result_general2_content.iconSmall;
               notificationLargeIcon_general2 = result_general2_content.iconLarge;
               notificationDelay_general2 = result_general2_content.delayHours;

               general2_contentFlag = true;

           }
           else
           {

               general2_contentFlag = false;

           }
       });
    }


    //Intent - track notifications clicked

    void NotificationAnalytics()
    {
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            analyticsId = notificationIntentData.Id;  //will take but it is not currenty used - it gives a long int id
            analyticsChannel = notificationIntentData.Channel;
            analyticsNotification = notificationIntentData.Notification.ToString(); //will take but it is not currenty used - it gives a generic Android.Notification, not something unique

            //Write to notification analytics
            Write_NotificationAnalytics();
        }
    }

    void Write_NotificationAnalytics()
    {
        var result = UniRESTClient.Async.Write(
            API.thetarotapp_notificationsAnalytics,
            new DB.Tta_notifications_analytics
                {
                username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                analyticsChannel = analyticsChannel,
                date = todayDate,
                time = DateTime.Now.ToString("HH:mm"),
                deviceModel = SystemInfo.deviceModel,
                deviceOS = SystemInfo.operatingSystem,
                appVersion = Application.version

    },
     (bool ok) =>
     {
         if (ok)
         {
             Debug.Log("Notification Analytics Written!");
         }
         else
         {
             Debug.Log("Notification AnalyticsWriting failed");
         }
     }
);
    }


    //Push Android Notification Flow
    private IEnumerator PushNotificationFlow()
    {
        yield return new WaitForSeconds(3f);

        NotificationAnalytics();
        yield return new WaitForSeconds(0.3f);

        checkNotificationsSchedule_DTH_DB();
        yield return new WaitForSeconds(0.25f);
        checkNotificationsSchedule_General1_DB();
        yield return new WaitForSeconds(0.25f);
        checkNotificationsSchedule_General2_DB();

        yield return new WaitForSeconds(0.3f);

        getNotificationsContent_DTH_DB();
        yield return new WaitForSeconds(0.25f);
        getNotificationsContent_General1_DB();
        yield return new WaitForSeconds(0.25f);
        getNotificationsContent_General2_DB();

        yield return new WaitForSeconds(0.3f);

        //DTH
        if(dth24_setFlag == false && dth24_contentFlag == true)
        {

            android_createNotificationChannel_DTH();

        } 
        else if (dth24_setFlag == true && dth24_contentFlag == true)
        {

            Debug.Log("Notification for Daily Tarot Horoscope(24) - is already set(2)");

        } else
        {

            Debug.Log("ERROR(2) --> CANNOT FIND NOTIFICATION MESSAGE!!!");

        }


        //General 1
        if (general1_setFlag == false && general1_contentFlag == true)
        {

            android_createNotificationChannel_General1();

        }
        else if (general1_setFlag == true && general1_contentFlag == true)
        {

            Debug.Log("Notification for General 1 - Already Set");

        }
        else
        {

            Debug.Log("ERROR(3) --> CANNOT FIND GENERAL 1 NOTIFICATION MESSAGE!!!");

        }


        //General 2
        if (general2_setFlag == false && general2_contentFlag == true)
        {

            android_createNotificationChannel_General2();

        }
        else if (general2_setFlag == true && general2_contentFlag == true)
        {

            Debug.Log("Notification for General 2 - Already Set");

        }
        else
        {

            Debug.Log("ERROR(3) --> CANNOT FIND GENERAL 2 NOTIFICATION MESSAGE!!!");

        }
    }


}
