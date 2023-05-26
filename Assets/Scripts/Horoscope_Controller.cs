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
using UnityEngine.EventSystems;
using TigerForge;

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
    public GameObject Sign_HSP_GameObject;
    public Image Image_Sign_HSP;
    public GameObject Image_Sign_HSP_GameObject;
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
    public ScrollRect Interpretation_Scroll;
    public GameObject VFX_HTD;

    [Header("Horoscope Sounds")]
    public AudioSource audio1;

    [Header("Animator Controllers")]
    public AnimatorOverrideController cardFadeIn;

    [Header("Reset Sign")]
    public bool ResetSign = false;

    [Header("DO NOT CHANGE!!!")]
    public bool SignSet;
    public int SignPos; 


    private int DAY_birth;
    private int MONTH_birth;
    private int Determined_Sign_Position;
    private int Picked_Sign_Position;
    private int randomDrawnCard;
    private int int_General_index;
    private int int_Personal_index;
    private int int_Love_index;
    private int int_Career_index;
    private int int_Financial_index;
    private int int_Spiritual_index;

    private int cardNo_DB;
    private int interpretationNo_DB;

    private Vector3 CardPosition_initial;
    private Vector3 CardPosition_last;
    private Vector3 CardPosition_last_rotation;
    private GameObject card_horoscope;
    private String todayDate;
    private String todayHoroscopeDate;
    private bool todaySignSeen;

    // Start is called before the first frame update
    void Start()
    {
        if(ResetSign == true)
        {
            PlayerPrefs.SetInt("SignSet", 0);
            PlayerPrefs.SetInt("SignPos", 0);
        }




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


        todayDate = DateTime.Now.ToString("dd-MM-yyyy");
        todaySignSeen = false;

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

        ZodiacLabelsContent ZLC_title = ZodiacLabelsContent.FindEntity(entity => entity.Part == "Horoscope Sign Picker" && entity.LabelType == "Title" && entity.Order == 1 && entity.Lang == "ro");
        Title_HSP.text = ZLC_title.PRE_Label;

    }

    public void HoroscopeBack_from_ZodiacSign()
    {

        if(SignSet == false)
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

            //Zodial Pick Elements
            Panel_SignPicker.SetActive(true);
            Image_Sign_HSP_GameObject.SetActive(false);
            Sign_HSP_GameObject.SetActive(false);
            Button_Next_HSP.SetActive(false);
            Button_ChangeSign_HSP.SetActive(false);
        }
        else
        {
            //Reset existing GameObjects
            Panel_SignPicker.SetActive(true);
            Image_Sign_HSP_GameObject.SetActive(false);
            Sign_HSP_GameObject.SetActive(false);
            Button_Next_HSP.SetActive(false);
            Button_ChangeSign_HSP.SetActive(false);

            Panel_HSP.SetActive(false);

            StartCoroutine(DailyTarot());
        }


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


    //(3) ----- Horoscope Sign Picker -----

    public void ZodiacPick_Sign()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;

        Picked_Sign_Position = int.Parse(name);

        ZodiacRoster ZODIAC = ZodiacRoster.FindEntity(entity => entity.Position == Picked_Sign_Position);

        Image_Sign_HSP_GameObject.SetActive(true);
        Image_Sign_HSP.sprite = ZODIAC.ZRound;

        Sign_HSP_GameObject.SetActive(true);
        Sign_HSP.text = ZODIAC.Sign_RO;

        ZodiacLabelsContent ZLC_title = ZodiacLabelsContent.FindEntity(entity => entity.Part == "Horoscope Sign Picker" && entity.LabelType == "Title" && entity.Order == 2 && entity.Lang == "ro");
        Title_HSP.text = ZLC_title.PRE_Label;


        Button_Next_HSP.SetActive(true);
        Button_ChangeSign_HSP.SetActive(true);


        Panel_SignPicker.SetActive(false);
    }

    public void ZodiacPick_Change_Sign()
    {
        Panel_SignPicker.SetActive(true);
        Image_Sign_HSP_GameObject.SetActive(false);
        Sign_HSP_GameObject.SetActive(false);
        Button_Next_HSP.SetActive(false);
        Button_ChangeSign_HSP.SetActive(false);
    }

    public void ZodiacSignPicker_Next()
    {
        //Reset existing GameObjects
        Panel_SignPicker.SetActive(true);
        Image_Sign_HSP_GameObject.SetActive(false);
        Sign_HSP_GameObject.SetActive(false);
        Button_Next_HSP.SetActive(false);
        Button_ChangeSign_HSP.SetActive(false);

        Panel_HSP.SetActive(false);


        SignSet = true;
        SignPos = Picked_Sign_Position;

        PlayerPrefs.SetInt("SignSet", 1);
        PlayerPrefs.SetInt("SignPos", SignPos);

        StartCoroutine(DailyTarot());
    }



    //(4) ----- Horoscope Tarot Daily -----

    private IEnumerator DailyTarot()
    {
        checkDailyHoroscope();

        yield return new WaitForSeconds(0.5f);

        ZodiacRoster ZODIAC = ZodiacRoster.FindEntity(entity => entity.Position == SignPos);

        Image_Sign_HTD.sprite = ZODIAC.ZRound;

        Panel_HTD.SetActive(true);


        if (todaySignSeen == true)
        {

            TarotDecks pickedCard = TarotDecks.FindEntity(entity => entity.No == cardNo_DB);

            card_horoscope = Instantiate(
                    pickedCard.Deck2,
                    CardPosition_last,
                    Quaternion.Euler(-110f, 90f, -90f),
                    transform);

            card_horoscope.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        }
        else
        {
            VFX_HTD.SetActive(true);
            audio1.Play();
            yield return new WaitForSeconds(0.5f);
            int randomCard = Random.Range(1, 23);

            TarotDecks pickedCard = TarotDecks.FindEntity(entity => entity.No == randomCard);

            randomDrawnCard = randomCard;

            card_horoscope = Instantiate(
                pickedCard.Deck2,
                CardPosition_initial,
                Quaternion.Euler(-90f, 0f, 0f),
                transform);

            card_horoscope.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Animator animator = card_horoscope.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = cardFadeIn as RuntimeAnimatorController;

            cardNo_DB = randomDrawnCard;

            //interpretation chunck
            List<Zodiac_to_Tarot> ZtT_general = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 1);
            List<Zodiac_to_Tarot> ZtT_personal = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 2);
            List<Zodiac_to_Tarot> ZtT_love = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 3);
            List<Zodiac_to_Tarot> ZtT_profesion = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 4);
            List<Zodiac_to_Tarot> ZtT_finance = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 5);
            List<Zodiac_to_Tarot> ZtT_spirit = Zodiac_to_Tarot.FindEntities(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 6);

            int_General_index = Random.Range(1, ZtT_general.Count + 1);
            int_Personal_index = Random.Range(1, ZtT_personal.Count + 1);
            int_Love_index = Random.Range(1, ZtT_love.Count + 1);
            int_Career_index = Random.Range(1, ZtT_profesion.Count + 1);
            int_Financial_index = Random.Range(1, ZtT_finance.Count + 1);
            int_Spiritual_index = Random.Range(1, ZtT_spirit.Count + 1);


            yield return new WaitForSeconds(2f);

            card_horoscope.transform.DOMove(CardPosition_last, 1f).SetEase(Ease.InOutSine);
            card_horoscope.transform.DORotate(CardPosition_last_rotation, 1f, RotateMode.Fast);


            writeDailyHoroscope();
        }

        todayHoroscopeDate = DateTime.Now.ToString("dd-MMMM-yyyy", CultureInfo.CreateSpecificCulture("ro"));

        //interpretation formation continue
        ZodiacMiniRoster zmr = ZodiacMiniRoster.FindEntity(entity => entity.Position == SignPos);
        Zodiac_to_Tarot ztt_general = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 1 && entity.InterpretationIndex == int_General_index);
        Zodiac_to_Tarot ztt_personal = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 2 && entity.InterpretationIndex == int_Personal_index);
        Zodiac_to_Tarot ztt_love = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 3 && entity.InterpretationIndex == int_Love_index);
        Zodiac_to_Tarot ztt_career = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 4 && entity.InterpretationIndex == int_Career_index);
        Zodiac_to_Tarot ztt_finance = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 5 && entity.InterpretationIndex == int_Financial_index);
        Zodiac_to_Tarot ztt_spirit = Zodiac_to_Tarot.FindEntity(entity => entity.CardNo == cardNo_DB && entity.TypeIndex == 6 && entity.InterpretationIndex == int_Spiritual_index);


        string dailyHoroscope =
            zmr.HoroscopeTitle + "\n" +
            zmr.HoroscopeDateAid1 + todayHoroscopeDate + zmr.HoroscopeDateAid2 + "\n" + "\n" +

            ztt_general.Type + "\n" + "\n" +

            zmr.Preposition + ztt_general.Interpretation + "\n" + "\n" +

            ztt_personal.Type + "\n" + "\n" +

            ztt_personal.Interpretation + "\n" + "\n" +

            ztt_love.Type + "\n" + "\n" +

            ztt_love.Interpretation + "\n" + "\n" +

            ztt_career.Type + "\n" + "\n" +

            ztt_career.Interpretation + "\n" + "\n" +

            ztt_finance.Type + "\n" + "\n" +

            ztt_finance.Interpretation + "\n" + "\n" +

            ztt_spirit.Type + "\n" + "\n" +

            ztt_spirit.Interpretation + "\n" + "\n";

        Interpretation_HTD.text = dailyHoroscope;


        yield return new WaitForSeconds(1f);



        Button_Back_HTD.SetActive(true);
        Button_ChangeSign_HTD.SetActive(true);
        Panel_Interpretation_HTD.SetActive(true);
        Interpretation_Scroll.verticalNormalizedPosition = 1f;

    }

    public void DailyTarot_changeSign()
    {

        Panel_HTD.SetActive(false);
        VFX_HTD.SetActive(false);
        Button_Back_HTD.SetActive(false);
        Button_ChangeSign_HTD.SetActive(false);
        Panel_Interpretation_HTD.SetActive(false);

        if (card_horoscope == null)
        {
            Debug.Log("It has not been created and no destroy");
        }
        else
        {
            Destroy(card_horoscope);
            Debug.Log("The card is destroyed");
        }


        Panel_HSP.SetActive(true);
        Panel_SignPicker.SetActive(true);
        Image_Sign_HSP_GameObject.SetActive(false);
        Sign_HSP_GameObject.SetActive(false);
        Button_Next_HSP.SetActive(false);
        Button_ChangeSign_HSP.SetActive(false);

    }

    void checkDailyHoroscope(){

        var result = UniRESTClient.Async.ReadOne<DB.Tta_horoscopeDaily_1card>
               (API.thetarotapp_horoscopedaily1card,
               new DB.Tta_horoscopeDaily_1card
               {
                   username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                   unityDate = todayDate,
                   signNo = SignPos

               }, (DB.Tta_horoscopeDaily_1card result, bool ok) => {
                   if (ok)
                   {
                       Debug.Log("Sign already seen");
                       cardNo_DB = result.cardNo;
                       int_General_index = result.intGeneral;
                       int_Personal_index = result.intPersonal;
                       int_Love_index = result.intLove;
                       int_Career_index = result.intCareer;
                       int_Financial_index = result.intFinance;
                       int_Spiritual_index = result.intSpirit;
                       todaySignSeen = true;

                   }
                   else
                   {

                       Debug.Log("New daily horoscope needed!");
                       todaySignSeen = false;

                   }
               });
    }

    void writeDailyHoroscope()
    {

        var result = UniRESTClient.Async.Update(
            API.thetarotapp_horoscopedaily1card,
            new DB.Tta_horoscopeDaily_1card
            {
                username = "TTA_" + SystemInfo.deviceUniqueIdentifier,
                unityDate = todayDate,
                signNo = SignPos,
                cardNo = randomDrawnCard,
                intGeneral = int_General_index,
                intPersonal = int_Personal_index,
                intLove = int_Love_index,
                intCareer = int_Career_index,
                intFinance = int_Financial_index,
                intSpirit = int_Spiritual_index

            },
     (bool ok) =>
     {
         Debug.Log("Record written for sign position " + SignPos + " and the card generated " + randomDrawnCard);
     });
    }







}
