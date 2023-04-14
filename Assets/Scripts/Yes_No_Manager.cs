using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;

public class Yes_No_Manager : MonoBehaviour
{
    [Header("GUI")]
    public GameObject GUI;

    [Header("Deck to use")]
    public int Deck;

    [Header("Animator Controllers")]
    public AnimatorOverrideController cardanim;

    [Header("Cards extracted")]
    public GameObject suffleCard1;
    public GameObject suffleCard2;
    public GameObject suffleCard3;

    [Header("Animation Timing")]
    public float timeServe1andReveal1 = 2f;
    public float timeServe2andReveal2 = 2f;
    public float timeServe3andReveal3 = 2f;
    public float timeReveal1andServe2 = 2f;
    public float timeReveal2andServe3 = 2f;


    //UI System
    public VisualElement StartUI;
    public VisualElement EndUI;

    public Button CardGet;
    public Button CardDestroy;

    public VisualElement answerBox;
    public Label answerText;


    //Privates
    private GameObject Card1GO;
    private GameObject Card2GO;
    private GameObject Card3GO;

    private int card1;
    private int card2;
    private int card3;
    private int card1group;
    private int card2group;
    private int card3group;
    private string card1Name;
    private string card2Name;
    private string card3Name;
    private string isReversed1;
    private string isReversed2;
    private string isReversed3;
    private float yes1percent;
    private float yes2percent;
    private float yes3percent;
    private float Total_Yes;
    private float Total_No;
    private string Answer;

    private Vector3 card1Position;
    private Vector3 card2Position;
    private Vector3 card3Position;
    private Vector3 shuffleCard1Position;
    private Vector3 shuffleCard2Position;
    private Vector3 shuffleCard3Position;


    // Start is called before the first frame update
    void Start()
    {
        card1Position = new Vector3(-0.043f, 1.521f, -0.178f);
        card2Position = new Vector3(0.289f, 1.513f, -0.1805f);
        card3Position = new Vector3(0.126f, 1.545f, -0.102f);

        shuffleCard1Position = suffleCard1.transform.position;
        shuffleCard2Position = suffleCard2.transform.position;
        shuffleCard3Position = suffleCard3.transform.position;

        var root = GUI.GetComponent<UIDocument>().rootVisualElement;

        CardGet = root.Q<Button>("CardGet");
        CardDestroy = root.Q<Button>("CardDestroy");
        StartUI = root.Q<VisualElement>("TestButtonsContainer1");
        EndUI = root.Q<VisualElement>("TestButtonsContainer2");

        answerBox = root.Q<VisualElement>("Answer_Test_Section");
        answerText = root.Q<Label>("Answer_Content");

        CardGet.clicked += drawCard1;
        CardDestroy.clicked += reset_for_new_question;

    }

    // Update is called once per frame
    void Update()
    {
        
    }






    //ThreeCard Yes/No questions ------- SYSTEM ---------------------
    //---------------------------------------------------------------
    void drawCard1()
    {
        StartUI.style.display = DisplayStyle.None;

        card1 = Random.Range(1, 45);

        Yes_No_Master YNtable1 = Yes_No_Master.FindEntity(entity => entity.CardNo == card1);

        card1group = YNtable1.Group;
        card1Name = YNtable1.Card;
        isReversed1 = YNtable1.Reversed;
        yes1percent = YNtable1.Yes;

        
        StartCoroutine(drawCard1_UI_delay());
    }

    private IEnumerator drawCard1_UI_delay()
    {
        suffleCard1.GetComponent<Animator>().SetInteger("Trigger", 1);

        yield return new WaitForSeconds(timeServe1andReveal1);

        drawCard1_UI();
    }

    void drawCard1_UI()
    {

        TarotDecks tarotCard1GO = TarotDecks.FindEntity(entity => entity.Card == card1Name);

        float angle;

        if (isReversed1 == "Yes")
        {
            angle = 0f - 16.699f;
        }
        else
        {
            angle = 180f - 16.699f;
        }

        suffleCard1.GetComponent<Animator>().SetInteger("Trigger", 0);


        Card1GO = Instantiate(
            tarotCard1GO.Deck2,
            card1Position,
            Quaternion.Euler(180f, angle, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed


        Card1GO.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        Animator animator = Card1GO.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = cardanim as RuntimeAnimatorController;

        StartCoroutine(drawCard1_to_2_UI_delay());
    }

    private IEnumerator drawCard1_to_2_UI_delay()
    {

        yield return new WaitForSeconds(timeReveal1andServe2);

        drawCard2();
    }


    void drawCard2()
    {
        card2 = Random.Range(1, 45);

        Yes_No_Master YNtable2 = Yes_No_Master.FindEntity(entity => entity.CardNo == card2);

        if(YNtable2.Group == card1group)
        {
            drawCard2();
        }
        else
        {
            card2group = YNtable2.Group;
            card2Name = YNtable2.Card;
            isReversed2 = YNtable2.Reversed;
            yes2percent = YNtable2.Yes;


            StartCoroutine(drawCard2_UI_delay());
        }

    }

    private IEnumerator drawCard2_UI_delay()
    {
        suffleCard2.GetComponent<Animator>().SetInteger("Trigger", 2);

        yield return new WaitForSeconds(timeServe2andReveal2);

        drawCard2_UI();
    }

    void drawCard2_UI()
    {

        TarotDecks tarotCard2GO = TarotDecks.FindEntity(entity => entity.Card == card2Name);

        float angle;

        if (isReversed2 == "Yes")
        {
            angle = 0f + 10.716f;
        }
        else
        {
            angle = 180f + 10.716f;
        }

        suffleCard2.GetComponent<Animator>().SetInteger("Trigger", 0);

        Card2GO = Instantiate(
            tarotCard2GO.Deck2,
            card2Position,
            Quaternion.Euler(180f, angle, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed


        Card2GO.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        Animator animator = Card2GO.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = cardanim as RuntimeAnimatorController;

        StartCoroutine(drawCard2_to_3_UI_delay());
    }


    private IEnumerator drawCard2_to_3_UI_delay()
    {

        yield return new WaitForSeconds(timeReveal2andServe3);

        drawCard3();
    }


    void drawCard3()
    {
        card3 = Random.Range(1, 45);

        Yes_No_Master YNtable3 = Yes_No_Master.FindEntity(entity => entity.CardNo == card3);

        if (YNtable3.Group == card1group || YNtable3.Group == card2group)
        {
            drawCard3();
        }
        else
        {
            card3group = YNtable3.Group;
            card3Name = YNtable3.Card;
            isReversed3 = YNtable3.Reversed;
            yes3percent = YNtable3.Yes;

            Debug.Log("The cards picked are: " + card1 + " and " + card2 + " and " + card3);

            determineAnswer();
        }

    }

    void determineAnswer()
    {
        Total_Yes = (yes1percent + yes2percent + yes3percent) / 3;
        Total_No = 1 - Total_Yes;

        if (Total_Yes >= 0.5 && Total_Yes < 0.6 || Total_No >= 0.5 && Total_No < 0.6)
        {
            int i = Random.Range(1, 5);

            YES_NO_Evaluation YNEvaluation = YES_NO_Evaluation.FindEntity(entity => entity.Group == "IDK" && entity.GroupIndex == i);

            Answer = YNEvaluation.AnswerRO;

        } 
        else if (Total_Yes >= 0.6)
        {
            int i = Random.Range(1, 4);

            YES_NO_Evaluation YNEvaluation = YES_NO_Evaluation.FindEntity(entity => entity.Group == "Yes" && entity.Min <= Total_Yes && entity.Max > Total_Yes && entity.GroupIndex == i);

            Answer = YNEvaluation.AnswerRO;

        }
        else if (Total_No >= 0.6)
        {
            int i = Random.Range(1, 4);

            YES_NO_Evaluation YNEvaluation = YES_NO_Evaluation.FindEntity(entity => entity.Group == "No" && entity.Min <= Total_No && entity.Max > Total_No && entity.GroupIndex == i);

            Answer = YNEvaluation.AnswerRO;

        }



        StartCoroutine(drawCard3_UI_delay());
    }

    private IEnumerator drawCard3_UI_delay()
    {
        suffleCard3.GetComponent<Animator>().SetInteger("Trigger", 3);

        yield return new WaitForSeconds(timeServe3andReveal3);

        drawCard3_UI();
    }


    void drawCard3_UI()
    {

        TarotDecks tarotCard3GO = TarotDecks.FindEntity(entity => entity.Card == card3Name);

        float angle;

        if (isReversed3 == "Yes")
        {
            angle = 0f;
        }
        else
        {
            angle = 180f;
        }

        suffleCard3.GetComponent<Animator>().SetInteger("Trigger", 0);

        Card3GO = Instantiate(
            tarotCard3GO.Deck2,
            card3Position,
            Quaternion.Euler(180f, angle, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed


        Card3GO.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        Animator animator = Card3GO.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = cardanim as RuntimeAnimatorController;

        AnswerGUI();
    }

    //ThreeCard Yes/No questions ------- END ---------------------
    //------------------------------------------------------------
    void AnswerGUI()
    {
        answerBox.style.display = DisplayStyle.Flex;
        answerText.text = Answer;
        EndUI.style.display = DisplayStyle.Flex;

    }

    void reset_for_new_question()
    {
        answerBox.style.display = DisplayStyle.None;
        EndUI.style.display = DisplayStyle.None;
        StartUI.style.display = DisplayStyle.Flex;

        Destroy(Card1GO);
        Destroy(Card2GO);
        Destroy(Card3GO);

        

    }


}
