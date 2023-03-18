using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TigerForge;

/* 
 
  ============================
   HOW TO MAKE THIS DEMO WORK
  ============================
  
  In the UniREST Server application, create:
  
  - a SyncTable named 'synctest'
    
  - be sure to have your account registered and another account who will be your opponent

  - in the Start() function below, type your username and password in the Login() method

  - type your user ID and the other user ID in the Player 1 and Player 2 input fields

  ****************************************************************
   Once everything has been set, uncomment the code below.
  ****************************************************************

 */

public class SyncTableDemo : MonoBehaviour
{
    public GameObject canvas;
    UniRESTClient.SyncTable mySync;

    void Start()
    {
        UniRESTClient.debugMode = true;
        mySync = new UniRESTClient.SyncTable();

        _ = UniRESTClient.Async.Login("MyUser", "MyPass", (bool ok) => { 
        
            if (ok) GetInput("P1ID").text = UniRESTClient.UserID.ToString(); else GetInput("P1ID").text = "ERROR!";

            GetInput("P2ID").text = "9";
            SetButtonClick("Start");
            SetButtonClick("Stop");

            GetText("Status").text = "Press START";

        });

        for (var i = 1; i <= 9; i++)
        {
            SetButtonClick("P1_b" + i);
            SetButtonClick("P2_b" + i);
            SetButtonText("P1_b" + i, " ");
            SetButtonText("P2_b" + i, " ");
        }

    }

    InputField GetInput(string name)
    {
        var go = GameObject.Find("TXT_" + name).GetComponent<InputField>();
        if (go == null) Debug.LogError("Not found: TXT_" + name);
        return go;
    }

    Text GetText(string name)
    {
        return GameObject.Find("TXT_" + name).GetComponent<Text>();
    }

    void SetButtonClick(string name)
    {
        var go = GameObject.Find("BT_" + name);
        if (go == null) Debug.LogError("Not found: BT_" + name);
        var bt = go.GetComponent<Button>();
        if (bt != null) bt.onClick.AddListener(() => ButtonClicked(name)); else Debug.LogError("Not found: BT_" + name);
    }

    void SetButtonText(string name, string text)
    {
        var go = GameObject.Find("BT_" + name);
        if (go == null) Debug.LogError("Not found: BT_" + name);
        var bt = go.GetComponent<Button>();
        var txt = bt.GetComponentInChildren<Text>();
        if (txt != null) txt.text = text; else Debug.LogError("Not found Text in: BT_" + name);
    }

    void ButtonClicked(string name)
    {
        if (name == "Start") BTStart();
        if (name == "Stop") BTStop();
        if (name.StartsWith("P1_b")) P1Play(name);
        if (name.StartsWith("P2_b")) P2Play(name);
        Debug.Log("Clicked: " + name);
    }

    void BTStart()
    {
        GetText("Status").text = "PLAY!!!";

        for (var i = 1; i <= 9; i++)
        {
            SetButtonText("P1_b" + i, " ");
            SetButtonText("P2_b" + i, " ");
        }

        mySync.Add("player1", "synctest", int.Parse(GetInput("P1ID").text), 500);
        mySync.Add("player2", "synctest", int.Parse(GetInput("P2ID").text), 500);

        _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s1 = " ", s2 = " ", s3 = " ", s4 = " ", s5 = " ", s6 = " ", s7 = " ", s8 = " ", s9 = " " }, (bool ok) => {
            _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s1 = " ", s2 = " ", s3 = " ", s4 = " ", s5 = " ", s6 = " ", s7 = " ", s8 = " ", s9 = " " }, (bool ok2) => {

                mySync.Read("player1", (UniRESTClient.SyncTable.Data data) =>
                {
                    ShowResult(data);
                });

                mySync.Read("player2", (UniRESTClient.SyncTable.Data data) =>
                {
                    ShowResult(data);
                });

                mySync.ListenToAll();

            });
        });

    }

    void ShowResult(UniRESTClient.SyncTable.Data data)
    {
        if (data.s1 == "X") SetButtonText("P1_b1", "X");
        if (data.s2 == "X") SetButtonText("P1_b2", "X");
        if (data.s3 == "X") SetButtonText("P1_b3", "X");
        if (data.s4 == "X") SetButtonText("P1_b4", "X");
        if (data.s5 == "X") SetButtonText("P1_b5", "X");
        if (data.s6 == "X") SetButtonText("P1_b6", "X");
        if (data.s7 == "X") SetButtonText("P1_b7", "X");
        if (data.s8 == "X") SetButtonText("P1_b8", "X");
        if (data.s9 == "X") SetButtonText("P1_b9", "X");

        if (data.s1 == "O") SetButtonText("P2_b1", "O");
        if (data.s2 == "O") SetButtonText("P2_b2", "O");
        if (data.s3 == "O") SetButtonText("P2_b3", "O");
        if (data.s4 == "O") SetButtonText("P2_b4", "O");
        if (data.s5 == "O") SetButtonText("P2_b5", "O");
        if (data.s6 == "O") SetButtonText("P2_b6", "O");
        if (data.s7 == "O") SetButtonText("P2_b7", "O");
        if (data.s8 == "O") SetButtonText("P2_b8", "O");
        if (data.s9 == "O") SetButtonText("P2_b9", "O");

        if (data.s1 == "X") SetButtonText("P2_b1", "X");
        if (data.s2 == "X") SetButtonText("P2_b2", "X");
        if (data.s3 == "X") SetButtonText("P2_b3", "X");
        if (data.s4 == "X") SetButtonText("P2_b4", "X");
        if (data.s5 == "X") SetButtonText("P2_b5", "X");
        if (data.s6 == "X") SetButtonText("P2_b6", "X");
        if (data.s7 == "X") SetButtonText("P2_b7", "X");
        if (data.s8 == "X") SetButtonText("P2_b8", "X");
        if (data.s9 == "X") SetButtonText("P2_b9", "X");

        if (data.s1 == "O") SetButtonText("P1_b1", "O");
        if (data.s2 == "O") SetButtonText("P1_b2", "O");
        if (data.s3 == "O") SetButtonText("P1_b3", "O");
        if (data.s4 == "O") SetButtonText("P1_b4", "O");
        if (data.s5 == "O") SetButtonText("P1_b5", "O");
        if (data.s6 == "O") SetButtonText("P1_b6", "O");
        if (data.s7 == "O") SetButtonText("P1_b7", "O");
        if (data.s8 == "O") SetButtonText("P1_b8", "O");
        if (data.s9 == "O") SetButtonText("P1_b9", "O");
    }

    void BTStop()
    {
        mySync.StopAllListening();
        GetText("Status").text = "Press START";
    }

    void P1Play(string name)
    {
        if (name == "P1_b1") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s1 = "X" }, null);
        if (name == "P1_b2") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s2 = "X" }, null);
        if (name == "P1_b3") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s3 = "X" }, null);
        if (name == "P1_b4") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s4 = "X" }, null);
        if (name == "P1_b5") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s5 = "X" }, null);
        if (name == "P1_b6") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s6 = "X" }, null);
        if (name == "P1_b7") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s7 = "X" }, null);
        if (name == "P1_b8") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s8 = "X" }, null);
        if (name == "P1_b9") _ = mySync.Write("player1", new UniRESTClient.SyncTable.Data { s9 = "X" }, null);
    }

    void P2Play(string name)
    {
        if (name == "P2_b1") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s1 = "O" }, null);
        if (name == "P2_b2") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s2 = "O" }, null);
        if (name == "P2_b3") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s3 = "O" }, null);
        if (name == "P2_b4") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s4 = "O" }, null);
        if (name == "P2_b5") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s5 = "O" }, null);
        if (name == "P2_b6") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s6 = "O" }, null);
        if (name == "P2_b7") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s7 = "O" }, null);
        if (name == "P2_b8") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s8 = "O" }, null);
        if (name == "P2_b9") _ = mySync.Write("player2", new UniRESTClient.SyncTable.Data { s9 = "O" }, null);
    }
}
