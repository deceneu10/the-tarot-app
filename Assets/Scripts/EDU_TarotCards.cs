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

public class EDU_TarotCards : MonoBehaviour
{
    [Header("Script Description")]
    public string ScriptDescription = "Education - Tarot Cards Presentation";

    [Header("UI Elements")]
    public GameObject btn_majorarcana;
    public GameObject btn_wands;
    public GameObject btn_cups;
    public GameObject btn_swords;
    public GameObject btn_pentacles;
    public GameObject img_btn_majorarcana;
    public GameObject img_btn_wands;
    public GameObject img_btn_cups;
    public GameObject img_btn_swords;
    public GameObject img_btn_pentacles;
    public TextMeshProUGUI label_cardGroup;

    [Header("UI Sections")]
    public ScrollRect scrollRect_MajorArcana;
    public ScrollRect scrollRect_MinorArcana;
    public GameObject scroll_MajorArcana;
    public GameObject scroll_MinorArcana;
    public GameObject content_wands;
    public GameObject content_cups;
    public GameObject content_swords;
    public GameObject content_pentacles;


    [Header("Individual Card Assets")]
    public GameObject i_card_img;
    public GameObject i_group_img;
    public TextMeshProUGUI i_group_label;
    public GameObject i_tag1_img;
    public TextMeshProUGUI i_tag1_label;
    public GameObject i_tag2_img;
    public TextMeshProUGUI i_tag2_label;
    public GameObject i_tag3_img;
    public TextMeshProUGUI i_tag3_label;
    public GameObject i_tag4_img;
    public TextMeshProUGUI i_tag4_label;
    public GameObject i_tag5_img;
    public TextMeshProUGUI i_tag5_label;
    public GameObject i_element1_img;
    public TextMeshProUGUI i_element1_label;
    public GameObject i_astrology_img;
    public TextMeshProUGUI i_astrology_label;
    public TextMeshProUGUI i_description_label;


    private Color color_inactiveTG_button;


    // Start is called before the first frame update
    void Start()
    {

        ColorUtility.TryParseHtmlString("#6F6F6F", out color_inactiveTG_button);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CardGroupSelection()
    {
        EducationTarotCards EDU_TC_MA = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "MA");
        EducationTarotCards EDU_TC_C = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "C");
        EducationTarotCards EDU_TC_P = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "P");
        EducationTarotCards EDU_TC_S = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "S");
        EducationTarotCards EDU_TC_W = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "W");

        EDU_TarotCardsGroup EDU_Group_MA = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_MA.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_C = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_C.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_P = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_P.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_S = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_S.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_W = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_W.GroupIndex);



        if (EventSystem.current.currentSelectedGameObject.tag == "MajorArcana")
        {
            label_cardGroup.text = EDU_TC_MA.GroupName;

            img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgActive;
            img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgInactive;
            img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgInactive;
            img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgInactive;
            img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgInactive;

            btn_majorarcana.GetComponent<Image>().color = Color.white;
            btn_wands.GetComponent<Image>().color = color_inactiveTG_button;
            btn_cups.GetComponent<Image>().color = color_inactiveTG_button;
            btn_swords.GetComponent<Image>().color = color_inactiveTG_button;
            btn_pentacles.GetComponent<Image>().color = color_inactiveTG_button;

            scroll_MajorArcana.SetActive(true);
            scroll_MinorArcana.SetActive(false);



        } 
        else if (EventSystem.current.currentSelectedGameObject.tag == "Wands")
        {
            label_cardGroup.text = EDU_TC_W.GroupName;

            img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgInactive;
            img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgActive;
            img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgInactive;
            img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgInactive;
            img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgInactive;

            btn_majorarcana.GetComponent<Image>().color = color_inactiveTG_button;
            btn_wands.GetComponent<Image>().color = Color.white;
            btn_cups.GetComponent<Image>().color = color_inactiveTG_button;
            btn_swords.GetComponent<Image>().color = color_inactiveTG_button;
            btn_pentacles.GetComponent<Image>().color = color_inactiveTG_button;

            scroll_MajorArcana.SetActive(false);
            scroll_MinorArcana.SetActive(true);
            content_wands.SetActive(true);
            content_cups.SetActive(false);
            content_swords.SetActive(false);
            content_pentacles.SetActive(false);


        }
        else if (EventSystem.current.currentSelectedGameObject.tag == "Cups")
        {
            label_cardGroup.text = EDU_TC_C.GroupName;

            img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgInactive;
            img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgInactive;
            img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgActive;
            img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgInactive;
            img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgInactive;

            btn_majorarcana.GetComponent<Image>().color = color_inactiveTG_button;
            btn_wands.GetComponent<Image>().color = color_inactiveTG_button;
            btn_cups.GetComponent<Image>().color = Color.white;
            btn_swords.GetComponent<Image>().color = color_inactiveTG_button;
            btn_pentacles.GetComponent<Image>().color = color_inactiveTG_button;

            scroll_MajorArcana.SetActive(false);
            scroll_MinorArcana.SetActive(true);
            content_wands.SetActive(false);
            content_cups.SetActive(true);
            content_swords.SetActive(false);
            content_pentacles.SetActive(false);


        }
        else if (EventSystem.current.currentSelectedGameObject.tag == "Swords")
        {
            label_cardGroup.text = EDU_TC_S.GroupName;

            img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgInactive;
            img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgInactive;
            img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgInactive;
            img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgActive;
            img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgInactive;

            btn_majorarcana.GetComponent<Image>().color = color_inactiveTG_button;
            btn_wands.GetComponent<Image>().color = color_inactiveTG_button;
            btn_cups.GetComponent<Image>().color = color_inactiveTG_button;
            btn_swords.GetComponent<Image>().color = Color.white;
            btn_pentacles.GetComponent<Image>().color = color_inactiveTG_button;


            scroll_MajorArcana.SetActive(false);
            scroll_MinorArcana.SetActive(true);
            content_wands.SetActive(false);
            content_cups.SetActive(false);
            content_swords.SetActive(true);
            content_pentacles.SetActive(false);



        }
        else if (EventSystem.current.currentSelectedGameObject.tag == "Pentacles")
        {
            label_cardGroup.text = EDU_TC_P.GroupName;

            img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgInactive;
            img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgInactive;
            img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgInactive;
            img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgInactive;
            img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgActive;

            btn_majorarcana.GetComponent<Image>().color = color_inactiveTG_button;
            btn_wands.GetComponent<Image>().color = color_inactiveTG_button;
            btn_cups.GetComponent<Image>().color = color_inactiveTG_button;
            btn_swords.GetComponent<Image>().color = color_inactiveTG_button;
            btn_pentacles.GetComponent<Image>().color = Color.white;


            scroll_MajorArcana.SetActive(false);
            scroll_MinorArcana.SetActive(true);
            content_wands.SetActive(false);
            content_cups.SetActive(false);
            content_swords.SetActive(false);
            content_pentacles.SetActive(true);


        }

        scrollRect_MajorArcana.verticalNormalizedPosition = 1f;
        scrollRect_MinorArcana.verticalNormalizedPosition = 1f;
    }


    public void setExitScreen()
    {
        EducationTarotCards EDU_TC_MA = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "MA");
        EducationTarotCards EDU_TC_C = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "C");
        EducationTarotCards EDU_TC_P = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "P");
        EducationTarotCards EDU_TC_S = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "S");
        EducationTarotCards EDU_TC_W = EducationTarotCards.FindEntity(entity => entity.GroupIndex == "W");

        EDU_TarotCardsGroup EDU_Group_MA = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_MA.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_C = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_C.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_P = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_P.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_S = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_S.GroupIndex);
        EDU_TarotCardsGroup EDU_Group_W = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_TC_W.GroupIndex);

        //Sets the Cards screen to major arcana when the BACK button is pressed!
        img_btn_majorarcana.GetComponent<Image>().sprite = EDU_Group_MA.GroupImgActive;
        img_btn_wands.GetComponent<Image>().sprite = EDU_Group_W.GroupImgInactive;
        img_btn_cups.GetComponent<Image>().sprite = EDU_Group_C.GroupImgInactive;
        img_btn_swords.GetComponent<Image>().sprite = EDU_Group_S.GroupImgInactive;
        img_btn_pentacles.GetComponent<Image>().sprite = EDU_Group_P.GroupImgInactive;

        btn_majorarcana.GetComponent<Image>().color = Color.white;
        btn_wands.GetComponent<Image>().color = color_inactiveTG_button;
        btn_cups.GetComponent<Image>().color = color_inactiveTG_button;
        btn_swords.GetComponent<Image>().color = color_inactiveTG_button;
        btn_pentacles.GetComponent<Image>().color = color_inactiveTG_button;

        scroll_MajorArcana.SetActive(true);
        scroll_MinorArcana.SetActive(false);

    }


    public void enterIndividualCards()
    {
        EducationTarotCards EDU_table = EducationTarotCards.FindEntity(entity => entity.CardIndex == EventSystem.current.currentSelectedGameObject.name);


        //Group Info
        EDU_TarotCardsGroup EDU_Group = EDU_TarotCardsGroup.FindEntity(entity => entity.GroupIndex == EDU_table.GroupIndex);

        i_group_img.GetComponent<Image>().sprite = EDU_Group.GroupImgActive;
        i_group_label.text = EDU_Group.GroupName;


        //Tags info - All about TAGS
        EDU_Feel_Colors EDU_Feel = EDU_Feel_Colors.FindEntity(entity => entity.Feel == EDU_table.Feel);

        i_tag1_img.GetComponent<Image>().color = EDU_Feel.FeelColor;
        i_tag2_img.GetComponent<Image>().color = EDU_Feel.FeelColor;
        i_tag3_img.GetComponent<Image>().color = EDU_Feel.FeelColor;
        i_tag4_img.GetComponent<Image>().color = EDU_Feel.FeelColor;
        i_tag5_img.GetComponent<Image>().color = EDU_Feel.FeelColor;

        i_tag1_label.text = EDU_table.Tag1;
        i_tag2_label.text = EDU_table.Tag2;
        i_tag3_label.text = EDU_table.Tag3;
        i_tag4_label.text = EDU_table.Tag4;
        i_tag5_label.text = EDU_table.Tag5;


        //Element1 info
        EDU_Elements_Symbols EDU_Elements = EDU_Elements_Symbols.FindEntity(entity => entity.Element == EDU_table.Element1);

        i_element1_img.GetComponent<Image>().sprite = EDU_Elements.ElementImg;
        i_element1_img.GetComponent<Image>().color = Color.black;
        i_element1_label.text = EDU_table.Element1;


        //Astrology info
        EDU_Astro_Symbols EDU_AstroSymbol = EDU_Astro_Symbols.FindEntity(entity => entity.Astro == EDU_table.Astro);

        i_astrology_img.GetComponent<Image>().sprite = EDU_AstroSymbol.AstroImg;
        i_astrology_img.GetComponent<Image>().color = Color.black;
        i_astrology_label.text = EDU_table.Astro;


        //Description Full of the card
        i_description_label.text = EDU_table.Description;


        //Card Image info
        i_card_img.GetComponent<Image>().sprite = EDU_table.CardImg;


    }


}
