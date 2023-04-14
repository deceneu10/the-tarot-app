using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using kha2dev.DatePicker;
using System;
using System.Globalization;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Horoscope_Controller : MonoBehaviour
{
    //MainPanels
    [Header("Horoscope Parent Panel")]
    public GameObject MainMenu;
    public GameObject Horoscope;

    [Header("Horoscope Assesment")]
    public GameObject Panel_HA; 

    [Header("Horoscope Date Picker")]
    public GameObject Panel_HDP;
    public GameObject Title_HDP_GameObject;
    public TextMeshProUGUI Content_HDP;
    public GameObject Content_HDP_GameObject;
    public TextMeshProUGUI Sign_HDP;
    public GameObject Sign_HDP_GameObject;
    public Image Image_Sign_HDP;
    public GameObject Image_Sign_HDP_GameObject;
    public GameObject Button_Back_HDP;
    public GameObject Button_ChangeDate_HDP;
    public GameObject Button_Next_HDP;
    public CalendarDatePicker calendarDatePicker;

    [Header("Horoscope Sign Picker")]
    public GameObject Panel_HSP;
    public TextMeshProUGUI Title_HSP;
    public TextMeshProUGUI Sign_HSP;
    public Image Image_Sign_HSP;
    public GameObject Button_Back_HSP;
    public GameObject Button_ChangeSign_HSP;
    public GameObject Button_Next_HSP;
    public GameObject Panel_SignPicker;

    [Header("Horoscope Tarot Daily")]
    public GameObject Panel_HTD;
    public Image Image_Sign_HTD;
    public GameObject Button_Back_HTD;
    public GameObject Button_ChangeSign_HTD;
    public GameObject Panel_Interpretation_HTD;
    public TextMeshProUGUI Interpretation_HTD;
    public GameObject VFX_HTD;

    [Header("Animator Controllers")]
    public AnimatorOverrideController cardFadeIn;


    [Header("DO NOT CHANGE!!!")]
    public bool SignSet;
    public int SignPos; 


    private int DAY_birth;
    private int MONTH_birth;
    private int Determined_Sign_Position;

    private Vector3 CardPosition_initial;
    private Vector3 CardPosition_last;
    private Vector3 CardPosition_last_rotation;
    private GameObject card_horoscope;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("SignSet", 0);
        PlayerPrefs.SetInt("SignPos", 0);

        CardPosition_initial = new Vector3(0f, 2f, -6.6f);
        CardPosition_last = new Vector3(-2.4f, 6.8f, -1.65f);
        CardPosition_last_rotation = new Vector3(-110f, 90f, -90f);


        if (PlayerPrefs.GetInt("SignSet",0) == 0)
        {
            SignSet = false;
        } else
        {
            SignSet = true;
            SignPos = PlayerPrefs.GetInt("SignPos", 0);
        }


        Determined_Sign_Position = 0;

        

    }

    private void FixedUpdate()
    {


    }

    public void VFX_Test()
    {
        VFX_HTD.SetActive(true);
    }

    private IEnumerator example_delay() //used as example through out
    {
        yield return new WaitForSeconds(0.1f);
        
    }



    // (1) ----- Horoscope Assesment -----

    public void HoroscopeFlow()
    {
        MainMenu.SetActive(false);
        Horoscope.SetActive(true);

        if(SignSet == false)
        {

            Panel_HA.SetActive(true);

        }
        else
        {

            StartCoroutine(DailyTarot());

        }

    }

    public void HoroscopeBack_MainMenu()
    {
        Panel_HTD.SetActive(false);
        VFX_HTD.SetActive(false);
        Button_Back_HTD.SetActive(false);
        Button_ChangeSign_HTD.SetActive(false);
        Panel_Interpretation_HTD.SetActive(false);

        if(card_horoscope == null)
        {
            Debug.Log("It has not been created and no destroy");
        } else
        {
            Destroy(card_horoscope);
            Debug.Log("The card is destroyed");
        }

        


        Horoscope.SetActive(false);
        MainMenu.SetActive(true);

    }


    public void ZodiacSignDeterminer()
    {
        Panel_HA.SetActive(false);
        Panel_HDP.SetActive(true);
        calendarDatePicker.Show(DateSignDetermined);


    }


    public void ZodiacSignPicker()
    {
        Panel_HA.SetActive(false);
        Panel_HSP.SetActive(true);



    }

    public void HoroscopeBack_from_ZodiacSign()
    {
        Panel_HA.SetActive(true);
        Panel_HDP.SetActive(false);
        Panel_HSP.SetActive(false);


        //Zodiac Determine Elements
        Title_HDP_GameObject.SetActive(true);
        Content_HDP_GameObject.SetActive(false);
        Sign_HDP_GameObject.SetActive(false);
        Image_Sign_HDP_GameObject.SetActive(false);
        Button_ChangeDate_HDP.SetActive(false);
        Button_Next_HDP.SetActive(false);

    }



    //(2) ----- Horoscope Sign Determiner -----
    private void DateSignDetermined(DateTime result)
    {
        Title_HDP_GameObject.SetActive(false);

        DAY_birth = result.Day;
        MONTH_birth = result.Month;


        if ((MONTH_birth == 1 && DAY_birth >= 20) || (MONTH_birth == 2 && DAY_birth <= 18))
        {
            Determined_Sign_Position = 1;
        }
        else if ((MONTH_birth == 2 && DAY_birth >= 19) || (MONTH_birth == 3 && DAY_birth <= 20))
        {
            Determined_Sign_Position = 2;
        }
        else if ((MONTH_birth == 3 && DAY_birth >= 21) || (MONTH_birth == 4 && DAY_birth <= 20))
        {
            Determined_Sign_Position = 3;
        }
        else if ((MONTH_birth == 4 && DAY_birth >= 21) || (MONTH_birth == 5 && DAY_birth <= 20))
        {
            Determined_Sign_Position = 4;
        }
        else if ((MONTH_birth == 5 && DAY_birth >= 21) || (MONTH_birth == 6 && DAY_birth <= 21))
        {
            Determined_Sign_Position = 5;
        }
        else if ((MONTH_birth == 6 && DAY_birth >= 22) || (MONTH_birth == 7 && DAY_birth <= 22))
        {
            Determined_Sign_Position = 6;
        }
        else if ((MONTH_birth == 7 && DAY_birth >= 23) || (MONTH_birth == 8 && DAY_birth <= 22))
        {
            Determined_Sign_Position = 7;
        }
        else if ((MONTH_birth == 8 && DAY_birth >= 23) || (MONTH_birth == 9 && DAY_birth <= 22))
        {
            Determined_Sign_Position = 8;
        }
        else if ((MONTH_birth == 9 && DAY_birth >= 23) || (MONTH_birth == 10 && DAY_birth <= 22))
        {
            Determined_Sign_Position = 9;
        }
        else if ((MONTH_birth == 10 && DAY_birth >= 23) || (MONTH_birth == 11 && DAY_birth <= 21))
        {
            Determined_Sign_Position = 10;
        }
        else if ((MONTH_birth == 11 && DAY_birth >= 22) || (MONTH_birth == 12 && DAY_birth <= 20))
        {
            Determined_Sign_Position = 11;
        }
        else
        {
            Determined_Sign_Position = 12;
        }


        if(Determined_Sign_Position == 0)
        {
            calendarDatePicker.Show(DateSignDetermined); //retry to get Determined Position
        }


        ZodiacRoster ZODIAC = ZodiacRoster.FindEntity(entity => entity.Position == Determined_Sign_Position);

        Image_Sign_HDP_GameObject.SetActive(true);
        Image_Sign_HDP.sprite = ZODIAC.ZRound;

        Sign_HDP_GameObject.SetActive(true);
        Sign_HDP.text = ZODIAC.Sign_RO;


        ZodiacLabelsContent ZLC = ZodiacLabelsContent.FindEntity(entity => entity.Part == "Horoscope Date Picker" && entity.LabelType == "Content" && entity.Order == 1 && entity.Lang == "ro");

        Content_HDP_GameObject.SetActive(true);
        Content_HDP.text = ZLC.PRE_Label + result.ToString("dddd - dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ro")) + ZLC.AFTER_Label;


        Button_ChangeDate_HDP.SetActive(true);
        Button_Next_HDP.SetActive(true);


    }

    public void ZodiacSignDeterminer_changeDate()
    {
        calendarDatePicker.Show(DateSignDetermined);

        Title_HDP_GameObject.SetActive(true);
        Content_HDP_GameObject.SetActive(false);
        Sign_HDP_GameObject.SetActive(false);
        Image_Sign_HDP_GameObject.SetActive(false);
        Button_ChangeDate_HDP.SetActive(false);
        Button_Next_HDP.SetActive(false);

    }

    public void ZodiacSignDeterminer_Next()
    {
        //Reset existing GameObjects
        Title_HDP_GameObject.SetActive(true);
        Content_HDP_GameObject.SetActive(false);
        Sign_HDP_GameObject.SetActive(false);
        Image_Sign_HDP_GameObject.SetActive(false);
        Button_ChangeDate_HDP.SetActive(false);
        Button_Next_HDP.SetActive(false);

        Panel_HDP.SetActive(false);


        SignSet = true;
        SignPos = Determined_Sign_Position;

        PlayerPrefs.SetInt("SignSet", 1);
        PlayerPrefs.SetInt("SignPos", SignPos);

        StartCoroutine(DailyTarot());
    }







    //(4) ----- Horoscope Tarot Daily -----

    private IEnumerator DailyTarot()
    {

        ZodiacRoster ZODIAC = ZodiacRoster.FindEntity(entity => entity.Position == SignPos);

        Image_Sign_HTD.sprite = ZODIAC.ZRound;



        Panel_HTD.SetActive(true);

        VFX_HTD.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        int randomCard = Random.Range(1,23);

        TarotDecks pickedCard = TarotDecks.FindEntity(entity => entity.No == randomCard);

        card_horoscope = Instantiate(
            pickedCard.Deck2,
            CardPosition_initial,
            Quaternion.Euler(-90f, 0f, 0f),
            transform);

        card_horoscope.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Animator animator = card_horoscope.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = cardFadeIn as RuntimeAnimatorController;

        yield return new WaitForSeconds(2f);

        card_horoscope.transform.DOMove(CardPosition_last, 1f).SetEase(Ease.InOutSine);
        card_horoscope.transform.DORotate(CardPosition_last_rotation, 1f, RotateMode.Fast);

        yield return new WaitForSeconds(1f);

        Button_Back_HTD.SetActive(true);
        Button_ChangeSign_HTD.SetActive(true);
        Panel_Interpretation_HTD.SetActive(true);


    }






}
