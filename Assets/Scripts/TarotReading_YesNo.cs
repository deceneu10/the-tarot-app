using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class TarotReading_YesNo : MonoBehaviour
{
    [Header("Dependencies")]
    public CardController cC;
    public CardShuffle cS;

    [Header("UI Elements")]
    public GameObject answerPanel;

    public GameObject buttonReveal;
    public GameObject buttonNewReading;
    public GameObject buttonSuffleCards;
    public GameObject buttonBackToMainMenu;


    public TextMeshProUGUI answerContent;
    public TextMeshProUGUI cardDrawAidContent;


    [Header("Public elements for other clases - DO NOT CHANGE")]
    public string aidText1;
    public string aidText2;
    public string aidText3;
    public string aidText4;
    public bool hideSuffleBtn;


    //Privates
    private GameObject card1_determined;
    private GameObject card2_determined;
    private GameObject card3_determined;

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

    private Vector3 cardFlipRotationNormal;
    private Vector3 cardFlipRotationReversed;

    private Vector3 cardFlipBackRotationNormal;
    private Vector3 cardFlipBackRotationReversed;

    private Vector3 cardsMoveBug; //Move the draw deck cards out of the screen so that I do not get the bug of deck cards beeing stuck
    


    // Start is called before the first frame update
    void Start()
    {
        hideSuffleBtn = false;  

        TarotTypeFlows TTF1 = TarotTypeFlows.FindEntity(entity => entity.Type == "YesNoFlow" && entity.Step == 1);
        TarotTypeFlows TTF2 = TarotTypeFlows.FindEntity(entity => entity.Type == "YesNoFlow" && entity.Step == 2);
        TarotTypeFlows TTF3 = TarotTypeFlows.FindEntity(entity => entity.Type == "YesNoFlow" && entity.Step == 3);
        TarotTypeFlows TTF4 = TarotTypeFlows.FindEntity(entity => entity.Type == "YesNoFlow" && entity.Step == 4);

        aidText1 = TTF1.Labels;
        aidText2 = TTF2.Labels;
        aidText3 = TTF3.Labels;
        aidText4 = TTF4.Labels;


        cardFlipRotationNormal = new Vector3(-20f, 0f, 0f);
        cardFlipRotationReversed = new Vector3(20f, 180f, 0f);

        cardFlipBackRotationNormal = new Vector3(160f, 0f, 0f);
        cardFlipBackRotationReversed = new Vector3(-160f, 180f, 0f);

        cardsMoveBug = new Vector3(-200f, -200f, -200f);

        //UI Setup
        cardDrawAidContent.text = aidText1;

    }

    private void FixedUpdate()
    {
        if(hideSuffleBtn == true)
        {
            buttonSuffleCards.SetActive(false);
        } 
        else
        {
            buttonSuffleCards.SetActive(true);
        }
    }


    void drawCard1()
    {

        card1 = Random.Range(1, 45);

        //Find the card 1 in the Yes No Master table
        Yes_No_Master YNtable1 = Yes_No_Master.FindEntity(entity => entity.CardNo == card1);

        card1group = YNtable1.Group;
        card1Name = YNtable1.Card;
        isReversed1 = YNtable1.Reversed;
        yes1percent = YNtable1.Yes;

        drawCard1_UI();
    }

    void drawCard1_UI()
    {
        cC.deckCardLeft.SetActive(false);

        //Get the card from the deck by card name
        TarotDecks tarotCard1 = TarotDecks.FindEntity(entity => entity.Card == card1Name);

        float angleY;
        float angleX;

        if (isReversed1 == "Yes")
        {
            angleX = 160f;
            angleY = 0f;

        }
        else
        {
            angleX = -160f;
            angleY = 180f;
        }

        

        card1_determined = Instantiate(
            tarotCard1.Deck2,
            cC.deckCardLeft.transform.position,
            Quaternion.Euler(angleX, angleY, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed

        card1_determined.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        
        drawCard2();

    }



    void drawCard2()
    {
        card2 = Random.Range(1, 45);

        Yes_No_Master YNtable2 = Yes_No_Master.FindEntity(entity => entity.CardNo == card2);

        if (YNtable2.Group == card1group)
        {
            drawCard2();
        }
        else
        {
            card2group = YNtable2.Group;
            card2Name = YNtable2.Card;
            isReversed2 = YNtable2.Reversed;
            yes2percent = YNtable2.Yes;

            drawCard2_UI();
        }
        
    }

    void drawCard2_UI()
    {
        cC.deckCardCenter.SetActive(false);

        //Get the card from the deck by card name
        TarotDecks tarotCard2 = TarotDecks.FindEntity(entity => entity.Card == card2Name);

        float angleY;
        float angleX;

        if (isReversed2 == "Yes")
        {
            angleX = 160f;
            angleY = 0f;

        }
        else
        {
            angleX = -160f;
            angleY = 180f;
        }



        card2_determined = Instantiate(
            tarotCard2.Deck2,
            cC.deckCardCenter.transform.position,
            Quaternion.Euler(angleX, angleY, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed

        card2_determined.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        
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

            drawCard3_UI();
            determineAnswer();
        }

    }

    void drawCard3_UI()
    {
        cC.deckCardRight.SetActive(false);

        //Get the card from the deck by card name
        TarotDecks tarotCard3 = TarotDecks.FindEntity(entity => entity.Card == card3Name);

        float angleY;
        float angleX;

        if (isReversed3 == "Yes")
        {
            angleX = 160f;
            angleY = 0f;

        }
        else
        {
            angleX = -160f;
            angleY = 180f;
        }


        card3_determined = Instantiate(
            tarotCard3.Deck2,
            cC.deckCardRight.transform.position,
            Quaternion.Euler(angleX, angleY, 0f),
            transform);
        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0-normal 180-reversed

        card3_determined.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

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

        Debug.Log("Answer: " + Answer);
        StartCoroutine(flipCards_delay());

        cC.deckCardLeft.transform.position = cardsMoveBug;
        cC.deckCardCenter.transform.position = cardsMoveBug;
        cC.deckCardRight.transform.position = cardsMoveBug;
    }


    private IEnumerator flipCards_delay()
    {
        if (isReversed1 == "Yes")
        {
            card1_determined.transform.DORotate(cardFlipRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card1_determined.transform.DORotate(cardFlipRotationNormal, 1f, RotateMode.Fast);
        }

        yield return new WaitForSeconds(1f);

        if(isReversed2 == "Yes")
        {
            card2_determined.transform.DORotate(cardFlipRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card2_determined.transform.DORotate(cardFlipRotationNormal, 1f, RotateMode.Fast);
        }
        

        yield return new WaitForSeconds(1f);


        if (isReversed3 == "Yes")
        {
            card3_determined.transform.DORotate(cardFlipRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card3_determined.transform.DORotate(cardFlipRotationNormal, 1f, RotateMode.Fast);
        }

        //GUI
        answerPanel.SetActive(true);

        buttonReveal.SetActive(false);
        buttonNewReading.SetActive(true);


        answerContent.text = Answer;


    }

    public void Reveal()
    {
        drawCard1();
    }


    public void newReading()
    {
        StartCoroutine(newReading_delay());
    }

    private IEnumerator newReading_delay()
    {
        if (isReversed1 == "Yes")
        {
            card1_determined.transform.DORotate(cardFlipBackRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card1_determined.transform.DORotate(cardFlipBackRotationNormal, 1f, RotateMode.Fast);
        }

        if (isReversed2 == "Yes")
        {
            card2_determined.transform.DORotate(cardFlipBackRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card2_determined.transform.DORotate(cardFlipBackRotationNormal, 1f, RotateMode.Fast);
        }

        if (isReversed3 == "Yes")
        {
            card3_determined.transform.DORotate(cardFlipBackRotationReversed, 1f, RotateMode.Fast);
        }
        else
        {
            card3_determined.transform.DORotate(cardFlipBackRotationNormal, 1f, RotateMode.Fast);
        }

        yield return new WaitForSeconds(1.25f);

        cC.deckCardLeft.transform.position = cC.leftCardPosition;
        cC.deckCardCenter.transform.position = cC.centerCardPosition;
        cC.deckCardRight.transform.position = cC.rightCardPosition;


        cC.deckCardLeft.SetActive(true);
        cC.deckCardCenter.SetActive(true);
        cC.deckCardRight.SetActive(true);

        Destroy(card1_determined);
        Destroy(card2_determined);
        Destroy(card3_determined);

        yield return new WaitForSeconds(0.25f);

        cS.shortSuffle();


        //GUI
        cardDrawAidContent.text = aidText1;
        answerPanel.SetActive(false);
        buttonReveal.SetActive(false);
        buttonNewReading.SetActive(false);



    }
}
