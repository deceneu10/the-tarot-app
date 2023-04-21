using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFab_Manager : MonoBehaviour
{

    [SerializeField] public string pID;

    // Start is called before the first frame update
    void Start()
    {
       

        checkInternet();
    }


    void checkInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");

            StartCoroutine(retry_internet());

        }
        else
        {
            Login();
        }
    }


    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "TTA_" + SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);

    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Account succesfully logged in or created!");

        pID = result.InfoResultPayload.AccountInfo.PlayFabId;

    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while loggin in or receiving data");
        Debug.Log(error.GenerateErrorReport());

    }


    void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Data received!");


    }

    private IEnumerator retry_internet()
    {
        yield return new WaitForSeconds(3f);
        checkInternet();

    }
}