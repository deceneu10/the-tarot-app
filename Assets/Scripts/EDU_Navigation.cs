using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BansheeGz.BGDatabase;
using UnityEngine.EventSystems;
using System;
using TigerForge;
using UnityEngine.UI;
using TMPro;

public class EDU_Navigation : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Education - Navigation within section";


    [Header("Panels")]
    public GameObject panel_MainMenu;
    public GameObject panel_EDU;
    public GameObject panel_EDU_MainMenu;
    public GameObject panel_EDU_TarotCards;
    public GameObject panel_EDU_IndividualTarotCard;

    [Header("Dependencies")]
    public EDU_TarotCards edu_tarotcards;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void goToEducationMenu()
    {
        panel_MainMenu.SetActive(false);
        panel_EDU.SetActive(true);
        panel_EDU_MainMenu.SetActive(true);
        panel_EDU_TarotCards.SetActive(false);

    }

    public void goToMainMenu()
    {
        panel_MainMenu.SetActive(true);
        panel_EDU.SetActive(false);
        panel_EDU_MainMenu.SetActive(false);

    }

    public void goToTarotCardsMenu()
    {
        panel_EDU_MainMenu.SetActive(false);
        panel_EDU_TarotCards.SetActive(true);
        panel_EDU_IndividualTarotCard.SetActive(false);

    }


    public void goToIndividualCard()
    {


        panel_EDU_IndividualTarotCard.SetActive(true);
        panel_EDU_MainMenu.SetActive(false);
        panel_EDU_TarotCards.SetActive(false);

        edu_tarotcards.enterIndividualCards();


    }



}
