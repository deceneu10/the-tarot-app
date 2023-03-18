using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class NEW : MonoBehaviour
{

    private string newPost;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(horoscop());    
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator horoscop()
    {
        Debug.Log("In 5 seconds it will trigger");

        yield return new WaitForSeconds(5f);
        Debug.Log("Should trigger now!");

        new2();



    }
    void new2()
    {



        RestClient.Post("https://aztro.sameerkumar.website?sign=leo&day=today", newPost).Then(res => {
            EditorUtility.DisplayDialog("Status", res.StatusCode.ToString(), "Ok");
            Debug.Log("Headers---------------------");
            Debug.Log(res.Headers);
            Debug.Log("Data---------------------");
            Debug.Log(res.Data);
            Debug.Log("Request---------------------");
            Debug.Log(res.Request);
            Debug.Log("Text---------------------");
            Debug.Log(res.Text);

        });

    }


}
