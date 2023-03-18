using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TigerForge;
using System.Text;

/*
 * THIS FILE DOESN'T CONTAIN A WORKING DEMO.
 * 
 * The following code is a collection of commands for study purposes and you can use it for setting up working tests.
 * 
 */

public class Demo : MonoBehaviour
{

    public Text txtUsername;
    public Text txtPassword;
    public Button btRegister;
    public Button btLogin;
    public Text txtResult;

    public Text txtName;
    public Text txtGold;
    public Text txtHealth;
    public Text txtWeaponID;
    public Button btRead;
    public Button btWrite;
    public Button btUpdate;
    public Button btDelete;

    public Text txtWeaponName;
    public Text txtWeaponAttack;

    public GameObject goPlayer;
    public GameObject spr;

    public Text downloadText;
    public Button downloadStart;
    public Button downloadStop;

    public UniRESTClient.Download download = new UniRESTClient.Download();
    public UniRESTClient.Upload upload = new UniRESTClient.Upload();

    [System.Serializable]
    public class UserMeta
    {
        public int user_id = 0;
        public string meta_value = "";
    }

    [System.Serializable]
    class ComplexData
    {
        public ComplexDataItem selectedItem = new ComplexDataItem();
        public List<ComplexDataItem> allItems = new List<ComplexDataItem>();
        public Dictionary<string, ComplexDataItem> equipment = new Dictionary<string, ComplexDataItem>();
        public GameObject player;
        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();
        public string name = "";
        public bool isActive = true;
        public int gold = 1000;
        public float health = 265.6f;
    }

    [System.Serializable]
    class ComplexDataItem
    {
        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();
        public string[] names;
        public List<Color32> colors = new List<Color32>();
        public GameObject target;
        public int value = 0;
    }

    Dictionary<string, ComplexData> CreateComplexData()
    {
        var players = new Dictionary<string, ComplexData>();

        var allItems = new List<ComplexDataItem>();
        allItems.Add(CreateComplexItem());
        allItems.Add(CreateComplexItem());
        allItems.Add(CreateComplexItem());

        var equipment = new Dictionary<string, ComplexDataItem>();
        equipment.Add("one", CreateComplexItem());
        equipment.Add("two", CreateComplexItem());
        equipment.Add("three", CreateComplexItem());
        
        for (var i = 0; i < 10; i++)
        {

            var player = new ComplexData
            {
                selectedItem = CreateComplexItem(),
                player = goPlayer,
                position = new Vector3(1, 1, 1),
                rotation = new Quaternion(1, 1, 1, 1),
                name = "Player" + Random.Range(10, 100),
                isActive = true,
                gold = Random.Range(1000, 5000),
                health = Random.Range(100, 500),
                allItems = allItems,
                equipment = equipment
            };

            players.Add("P_" + i, player);
        }

        return players;
    }

    ComplexDataItem CreateComplexItem()
    {
        var colors = new List<Color32>();
        colors.Add(Color.black);
        colors.Add(Color.blue);
        colors.Add(Color.cyan);

        return new ComplexDataItem
        {
            position = new Vector3(1, 1, 1),
            rotation = new Quaternion(1, 1, 1, 1),
            names = new string[] { "A" + Random.Range(10, 100), "B" + Random.Range(10, 100), "C" + Random.Range(10, 100) },
            colors = colors,
            target = goPlayer,
            value = Random.Range(100, 900)
        };

        
    }

    //void Start()
    //{
    //    UniRESTClient.debugMode = true;

    //    btRegister.onClick.AddListener(OnRegisterButtonClick);
    //    btLogin.onClick.AddListener(OnLoginButtonClick);

    //    btUpdate.onClick.AddListener(OnUpdateButtonClick);
    //    btRead.onClick.AddListener(OnReadButtonClick);
    //    btDelete.onClick.AddListener(OnDeleteButtonClick);
    //}

    //void OnRegisterButtonClick()
    //{
    //    var result = UniRESTClient.Registration(txtUsername.text, txtPassword.text);

    //    if (result)
    //    {
    //        txtResult.text = "Registration done!";
    //    } else
    //    {
    //        txtResult.text = "Registration error: " + UniRESTClient.ServerError;
    //    }
    //}

    //void OnLoginButtonClick()
    //{
    //    var result = UniRESTClient.Login(txtUsername.text, txtPassword.text);

    //    if (result)
    //    {
    //        txtResult.text = "Login done!";
    //    }
    //    else
    //    {
    //        txtResult.text = "Login error: " + UniRESTClient.ServerError;
    //    }
    //}

    //void OnUpdateButtonClick()
    //{
    //    var result = UniRESTClient.Update(API.player_data, new DB.Player
    //    {
    //        user_id = UniRESTClient.UserID,
    //        name = txtName.text,
    //        gold = int.Parse(txtGold.text),
    //        health = float.Parse(txtHealth.text),
    //        weapon_id = int.Parse(txtWeaponID.text)
    //    });

    //    if (result)
    //    {
    //        txtResult.text = "Write/Update done!";
    //    }
    //    else
    //    {
    //        txtResult.text = "Write/Update error: " + UniRESTClient.DBerror;
    //    }
    //}

    //void OnReadButtonClick()
    //{
    //    var result = UniRESTClient.ReadOne<DB.Player>(API.player_data, new DB.Player { user_id = UniRESTClient.UserID });

    //    if (result == null)
    //    {
    //        txtResult.text = "Read error: " + UniRESTClient.DBerror;
    //    } else
    //    {
    //        txtName.GetComponentInParent<InputField>().text = result.name;
    //        txtGold.GetComponentInParent<InputField>().text = result.gold.ToString();
    //        txtHealth.GetComponentInParent<InputField>().text = result.health.ToString();
    //        txtWeaponID.GetComponentInParent<InputField>().text = result.weapon_id.ToString();

    //        var resultWeapon = UniRESTClient.ReadOne<DB.Weapons>(API.player_weapon, new DB.Weapons { id = result.weapon_id });

    //        if (resultWeapon == null)
    //        {
    //            txtResult.text = "Weapon Read error: " + UniRESTClient.DBerror;
    //        }
    //        else
    //        {
    //            txtWeaponName.GetComponentInParent<InputField>().text = resultWeapon.name;
    //            txtWeaponAttack.GetComponentInParent<InputField>().text = resultWeapon.attack.ToString();
    //        }
    //    }
    //}

    //void OnDeleteButtonClick()
    //{
    //    var result = UniRESTClient.Delete(API.player_data, new DB.Player { user_id = UniRESTClient.UserID });

    //    if (result)
    //    {
    //        txtResult.text = "Delete done!";
    //    }
    //    else
    //    {
    //        txtResult.text = "Delete error: " + UniRESTClient.DBerror;
    //    }
    //}
}
