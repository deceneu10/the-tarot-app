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
  
  - a database table named 'test' with the following columns:
    name    (TEXT)
    value   (FLOAT)
    json    (TEXT)
    
  - an API Group named 'test' with an API named 'test', so as to create the API 'test/test'.
    Then, select the 'test' table and enable Read, Write, Update and Delete operations, with the following settings:
    Read: enable 'ID as Key' and 'One record only'
    Update: enable 'ID as Key'
    Delete: enable 'ID as Key'
    
  - another API named 'php', so as to create the API 'test/php'.
    Then, enable the Custom PHP feature and paste, into the editor, this PHP code:

    <?php 
        $message = $URInput["message"];
        $reply = array();
        $reply["message"] = "RECEIVED: $message";
        $UROutput["result"] = "OK";
        $UROutput["data"] = $reply;
    ?>

  NOTE: remember to generate and update your "UniREST Client Config"!

  ****************************************************************
   Once everything has been set, remove uncomment the code below.
  ****************************************************************

 */

public class AsyncDemo : MonoBehaviour
{

    public GameObject canvas;

    //[System.Serializable]
    //class MyData
    //{
    //    public string message;
    //}

    //void Start()
    //{
    //    UniRESTClient.debugMode = true;

    //    GetInput("Username").text = "MyUser";
    //    GetInput("Password").text = "MyPass";

    //    GetInput("ReadID").text = "1";
    //    GetInput("UpdateID").text = "1";
    //    GetInput("DeleteID").text = "1";
    //    GetInput("ExistsID").text = "1";
    //    GetInput("UpdateText").text = "HI!";
        
    //    SetButtonClick("Login");
    //    SetButtonClick("Register");

    //    SetButtonClick("Write");
    //    SetButtonClick("Read");
    //    SetButtonClick("Update");
    //    SetButtonClick("Delete");
    //    SetButtonClick("Exists");
    //    SetButtonClick("PHP");
    //    SetButtonClick("NewPass");
    //    SetButtonClick("Tokens");
    //    SetButtonClick("FileSave");
    //    SetButtonClick("FileLoad");
    //    SetButtonClick("Math");
    //    SetButtonClick("Json");

    //}

    //InputField GetInput(string name)
    //{
    //    return GameObject.Find("TXT_" + name).GetComponent<InputField>();
    //}

    //Text GetText(string name)
    //{
    //    return GameObject.Find("TXT_" + name).GetComponent<Text>();
    //}

    //void SetButtonClick(string name)
    //{
    //    var bt = GameObject.Find("BT_" + name).GetComponent<Button>();
    //    if (bt != null) bt.onClick.AddListener(() => ButtonClicked(name)); else Debug.LogError("Not found: BT_" + name);
    //}

    //void ButtonClicked(string name)
    //{
    //    if (name == "Login") UserLogin();
    //    if (name == "Register") UserRegistration();
    //    if (name == "Write") DBWrite();
    //    if (name == "Read") DBRead();
    //    if (name == "Update") DBUpdate();
    //    if (name == "Delete") DBDelete();
    //    if (name == "PHP") PHPCall();
    //    if (name == "Exists") DBExists();
    //    if (name == "NewPass") DBNewPass();
    //    if (name == "Tokens") DBTokens();
    //    if (name == "FileSave") FileSave();
    //    if (name == "FileLoad") FileLoad();
    //    if (name == "Math") DBMath();
    //    if (name == "Json") DBJson();
    //    Debug.Log("Clicked: " + name);
    //}

    //void UserLogin()
    //{
    //    _ = UniRESTClient.Async.Login(GetInput("Username").text, GetInput("Password").text,(bool ok) => 
    //    {
    //        if (ok) Log(UniRESTClient.userAccount.username + " LOGGED IN!"); else Log("ERROR: " + UniRESTClient.ServerError);
    //    });
    //}

    //void UserRegistration()
    //{
    //    _ = UniRESTClient.Async.Registration(GetInput("Username").text, GetInput("Password").text, (bool ok) =>
    //    {
    //        if (ok) Log("User registered! Now you can login!!!"); else Log("ERROR: " + UniRESTClient.ServerError);
    //    });
    //}

    //void DBWrite()
    //{
    //    _ = UniRESTClient.Async.Write(API.test_test, new DB.Test { name = "HELLO! " + Random.Range(1, 1000)}, (bool ok) => 
    //    {
    //        if (ok) Log("New record written. ID: " + UniRESTClient.DBresponse); else Log("ERROR: " + UniRESTClient.DBerror);
    //    });
    //}

    //void DBRead()
    //{
    //    var ID = int.Parse(GetInput("ReadID").text);
    //    _ = UniRESTClient.Async.ReadOne<DB.Test>(API.test_test, new DB.Test { id = ID }, (DB.Test record, bool hasData) =>
    //    {
    //        if (hasData) Log("Record with ID " + ID + " read: " + record.name); else Log("NO DATA!");
    //    });
    //}

    //void DBUpdate()
    //{
    //    var ID = int.Parse(GetInput("UpdateID").text);
    //    _ = UniRESTClient.Async.Update(API.test_test, new DB.Test { id = ID, name = GetInput("UpdateText").text }, (bool ok) =>
    //    {
    //        if (ok) Log("Record with ID " + ID + " updated!"); else Log("ERROR!");
    //    });
    //}

    //void DBDelete()
    //{
    //    var ID = int.Parse(GetInput("DeleteID").text);
    //    _ = UniRESTClient.Async.Delete(API.test_test, new DB.Test { id = ID }, (bool hasData) =>
    //    {
    //        if (hasData) Log("Record with ID " + ID + " deleted!"); else Log("ERROR!");
    //    });
    //}

    //void PHPCall()
    //{
    //    _ = UniRESTClient.Async.CallOne<MyData>(API.test_php, new MyData { message = "HELLO!!!" }, (MyData response) => 
    //    {
    //        Log("PHP response: " + response.message);
    //    });
    //}

    //void DBExists()
    //{
    //    var ID = int.Parse(GetInput("ExistsID").text);
    //    _ = UniRESTClient.Async.Exists(API.test_test, new DB.Test { id = ID }, "id", (bool exists) =>
    //    {
    //        if (exists) Log(" Record with ID " + ID + " exists!"); else Log(" Record with ID " + ID + " doesn't exist!");
    //    });
    //    _ = UniRESTClient.Async.Count(API.test_test, new DB.Test { id = ID }, "id", (int count) =>
    //    {
    //        Log(" Records with ID " + ID + ": " + count, true);
    //    });
    //}

    //void DBNewPass()
    //{
    //    _ = UniRESTClient.Async.ChangePassword(UniRESTClient.userAccount.username, UniRESTClient.userAccount.password, GetInput("Password").text, (bool changed) => 
    //    {
    //        if (changed) Log("Password changed from '" + UniRESTClient.userAccount.password + "' to '" + GetInput("Password").text + "'"); else Log("ERROR!");
    //    });
    //}

    //void DBTokens()
    //{
    //    Debug.Log("OLD TR: " + RestAsync.HTTPTokens.tokenR);
    //    Debug.Log("OLD TW: " + RestAsync.HTTPTokens.tokenW);
    //    _ = UniRESTClient.Async.TokenUpdate(true, true, (bool changed) => {
    //        if (changed) Log(" TR: " + RestAsync.HTTPTokens.tokenR + " TW: " + RestAsync.HTTPTokens.tokenW); else Log("ERROR TR!");
    //    });
        
    //}

    //void DBMath()
    //{
    //    var ID = int.Parse(GetInput("ExistsID").text);
    //    _ = UniRESTClient.Async.UpdateMath(API.test_test, new DB.Test { id = ID }, "value", "(X + 1) * 2", (float result) => 
    //    {
    //        Log("Result is: " + result);
    //    });
    //}

    //void DBJson()
    //{
    //    var myJData = new Json();
    //    myJData.Add("health", Random.Range(1000, 9999));
    //    myJData.Add("gold", Random.Range(1000, 9999));
    //    var ID = int.Parse(GetInput("ExistsID").text);
    //    _ = UniRESTClient.Async.UpdateJSON(API.test_test, new DB.Test { id = ID }, "json", myJData, (bool ok) =>
    //    {
    //        if (ok) Log("Record with ID " + ID + " updated!"); else Log("ERROR!");
    //    });
    //}

    //void FileSave()
    //{
    //    var myFile = new UniRESTClient.Async.File("test.txt", UniRESTClient.Target.UserFolder);
    //    _ = myFile.Save("HELLO!", (bool ok) => 
    //    {
    //        if (ok) Log("Saved the string 'HELLO!' in a remote file."); else Log("ERROR");
    //    });
    //}

    //void FileLoad()
    //{
    //    var myFile = new UniRESTClient.Async.File("test.txt", UniRESTClient.Target.UserFolder);
    //    _ = myFile.Load((string data, bool hasData) =>
    //    {
    //        if (hasData) Log("Online Data: " + data); else Log("ERROR!");
    //    });
    //}


    //void Log(string message, bool append = false)
    //{
    //    if (append) GetText("Log").text += message; else GetText("Log").text = message;
    //}

}
