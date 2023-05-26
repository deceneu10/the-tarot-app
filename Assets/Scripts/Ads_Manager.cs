using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using System;
using TMPro;
using GoogleMobileAds;
using GoogleMobileAds.Api;


public class Ads_Manager : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Controls Ads within the app";

    [Header("ID's used in the script")]
    [Header("TEST ID's")]
    public string test_interstitialAd1_android;
    public string test_interstitialAd2_android;
    public string test_interstitialAd3_android;
    public string test_interstitialAd1_ios;
    public string test_interstitialAd2_ios;
    public string test_interstitialAd3_ios;
    [Header("PRODUCTION ID's")]
    public string production_interstitialAd1_android;
    public string production_interstitialAd2_android;
    public string production_interstitialAd3_android;
    public string production_interstitialAd1_ios;
    public string production_interstitialAd2_ios;
    public string production_interstitialAd3_ios;


    private string ad1_id;
    private string ad2_id;
    private string ad3_id;
    public bool useTestIds; //this should be true besides production release

    private string interstitialAd1_Android;
    private string interstitialAd2_Android;
    private string interstitialAd3_Android;
    private string interstitialAd1_iOS;
    private string interstitialAd2_iOS;
    private string interstitialAd3_iOS;

    private InterstitialAd interstitialAd1;
    private InterstitialAd interstitialAd2;
    private InterstitialAd interstitialAd3;


    //-----------------------
    private bool NoAds;     //Flag that will be set at the begging. Will look up on the server to see if the user has NoAds purchased or set and will populate here
                            //it will come from another script that will handle this
    //-----------------------

    private int clickCounterA; //Method or Algorithm A - 2 clicks on element to show Ad
    private int clickCounterB; //Method or Algorithm B - 3 clicks on element to show Ad




    void Start()
    {
        clickCounterA = 0;
        NoAds = false;

        if(useTestIds == true)
        {
            interstitialAd1_Android = test_interstitialAd1_android;
            interstitialAd2_Android = test_interstitialAd2_android;
            interstitialAd3_Android = test_interstitialAd3_android;
            interstitialAd1_iOS = test_interstitialAd1_ios;
            interstitialAd2_iOS = test_interstitialAd2_ios;
            interstitialAd3_iOS = test_interstitialAd3_ios;
        } 
        else
        {
            interstitialAd1_Android = production_interstitialAd1_android;
            interstitialAd2_Android = production_interstitialAd2_android;
            interstitialAd3_Android = production_interstitialAd3_android;
            interstitialAd1_iOS = production_interstitialAd1_ios;
            interstitialAd2_iOS = production_interstitialAd2_ios;
            interstitialAd3_iOS = production_interstitialAd3_ios;

        }


        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        ad1_id = interstitialAd1_Android;
        ad2_id = interstitialAd2_Android;
        ad3_id = interstitialAd3_Android;
#elif UNITY_IPHONE
        ad1_id = interstitialAd1_iOS;
#else
        ad1_id = "unused";
#endif



        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log("Mobile Ads initialization succesful!");
        });

        StartCoroutine(Check_MasterAdsController());

    }



    private IEnumerator Check_MasterAdsController()
    {

        yield return new WaitForSeconds(3.5f);

        var mac = UniRESTClient.Async.ReadOne<DB.Tta_MasterAdsController>
             (API.thetarotapp_masterAdsController,
             new DB.Tta_MasterAdsController
             {

                 Date = DateTime.Now.ToString("dd-MM-yyyy"),

             }, (DB.Tta_MasterAdsController mac, bool ok) => {
                 if (ok)
                 {

                     if(mac.NoAdsFlag == "Yes")
                     {

                         NoAds = true;
                         Debug.Log("NO Ads Should Show!");

                     } else
                     {

                         NoAds = false;
                         Debug.Log("Ads Should Show!");
                         LoadAds();
                     }


                 }
                 else
                 {

                     //need to do nothing as if the master table is not containing a record it will automatically show ads
                     LoadAds();

                 }
             });
    }

    private void LoadAds()
    {

        LoadInterstitialAd1();
        LoadInterstitialAd2();
        LoadInterstitialAd3();

    }




    public void LoadInterstitialAd1()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd1 != null)
        {
            interstitialAd1.Destroy();
        }


        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        InterstitialAd.Load(ad1_id, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd1 = ad;
            });
    }

    public void LoadInterstitialAd2()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd2 != null)
        {
            interstitialAd2.Destroy();
        }


        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        InterstitialAd.Load(ad2_id, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd2 = ad;
            });
    }

    public void LoadInterstitialAd3()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd3 != null)
        {
            interstitialAd3.Destroy();
        }


        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        InterstitialAd.Load(ad3_id, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd3 = ad;
            });
    }


    private void RegisterReloadHandler(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd1();
            LoadInterstitialAd2();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd1();
            LoadInterstitialAd2();
        };
    }


    public void ShowInterstitialAd1_A() //A. The click on the specific element should reach 2 before showing an Ad
    {
        if (NoAds == false)
        {
            clickCounterA++;

            if (clickCounterA >= 2)
            {
               
                Debug.Log("Ad should show");
                //Here we need to show the Ad


                if (interstitialAd1.IsLoaded())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd1.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }

                RegisterReloadHandler(interstitialAd1);

                clickCounterA = 0;
            }


        }
        else
        {
            //User enjoys an ads free app
        }
    }



    public void ShowInterstitialAd1_B() //B. The click on the specific element should reach 3 before showing an Ad
    {
        if (NoAds == false)
        {
            clickCounterB++;

            if (clickCounterB >= 3)
            {

                Debug.Log("Ad should show");
                //Here we need to show the Ad


                if (interstitialAd1.IsLoaded())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd1.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }

                RegisterReloadHandler(interstitialAd1);

                clickCounterB = 0;
            }


        }
        else
        {
            //User enjoys an ads free app
        }
    }


    public void ShowInterstitialAd2_B() //B. The click on the specific element should reach 3 before showing an Ad
    {
        if (NoAds == false)
        {
            clickCounterB++;

            if (clickCounterB >= 3)
            {

                Debug.Log("Ad should show");
                //Here we need to show the Ad


                if (interstitialAd2.IsLoaded())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd2.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                }

                RegisterReloadHandler(interstitialAd2);

                clickCounterB = 0;
            }

        }
        else
        {
            //User enjoys an ads free app
        }
    }


    public void ShowInterstitialAd3_NOW() //NOW. The click on the specific element triggers the Ads
    {
        if (NoAds == false)
        {

            Debug.Log("Ad should show");
            //Here we need to show the Ad


            if (interstitialAd3.IsLoaded())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd3.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }

            RegisterReloadHandler(interstitialAd3);

        }
        else
        {
            //User enjoys an ads free app
        }
    }


    private IEnumerator example_delay()
    {

        yield return new WaitForSeconds(0.5f);

    }



}
