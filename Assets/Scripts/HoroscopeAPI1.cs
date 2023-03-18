using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.ComponentModel;
using UnityEngine.UIElements;
using Models;
using Proyecto26;


public class HoroscopeAPI1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }




    private IEnumerator horoscop()
    {
        Debug.Log("In 5 seconds it will trigger");

        yield return new WaitForSeconds(5f);
        Debug.Log("Should trigger now!");

        horoscopeAPI1Async();



    }




        async Task horoscopeAPI1Async()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://sameer-kumar-aztro-v1.p.rapidapi.com/?sign=aquarius&day=today"),
            Headers =
    {
        { "X-RapidAPI-Key", "aa3d5666c4msh55ca8dbc81d5816p14b005jsn968906497f51" },
        { "X-RapidAPI-Host", "sameer-kumar-aztro-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(body);

            Debug.Log("The status is...");
            Debug.Log(body);
        }


    }

    void test()
    {



    }

}
