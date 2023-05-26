using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CardController : MonoBehaviour
{

    [Header("Dependencies")]
    public CameraPhysics CP;
    public TarotReading_YesNo tarotController;

    [Header("Audio elements")]
    public AudioSource audio_cardSlide;


    [Header("Parameters")]
    public float drawSpeed = 2f;
    public GameObject[] cards;

    //cards picked by user from the deck
    public GameObject deckCardLeft;
    public GameObject deckCardRight;
    public GameObject deckCardCenter;

    public Vector3 leftCardPosition;
    public Vector3 rightCardPosition;
    public Vector3 centerCardPosition;

    private Vector3 cardsRotation;

    private int randomNo1;
    private int randomNo2;
    private int randomNo3;




    public int positionDeterminer;

    // Start is called before the first frame update
    void Start()
    {
        leftCardPosition = new Vector3(-0.245f, 1.05f, -8.66f);
        centerCardPosition = new Vector3(0f, 1.05f, -8.66f);
        rightCardPosition = new Vector3(0.245f, 1.05f, -8.66f);
        cardsRotation = new Vector3(-20f, 0f, 0f);

        positionDeterminer = 1;

        automaticCardPicker1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void automaticCardPicker1()
    {
        randomNo1 = Random.Range(1, 22);
        automaticCardPicker2();
    }

    public void automaticCardPicker2()
    {
        int temrandomNo2 = Random.Range(1, 22);
        if(randomNo1 == temrandomNo2)
        {
            automaticCardPicker2();
        }
        else
        {
            randomNo2 = temrandomNo2;
            automaticCardPicker3();
        }

    }

    public void automaticCardPicker3()
    {
        int temrandomNo3 = Random.Range(1, 22);
        if (randomNo1 == temrandomNo3 || randomNo2 == temrandomNo3)
        {
            automaticCardPicker3();
        }
        else
        {
            randomNo3 = temrandomNo3;
        }

    }

    public void cardPicked()
    {
        if(positionDeterminer == 1)
        {
            deckCardLeft = CP.selectedCard.transform.gameObject;
         //   deckCardLeft = cards[randomNo1 - 1].transform.gameObject;
            deckCardLeft.transform.DOMove(leftCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardLeft.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
              .SetRelative(false) ;


            positionDeterminer = 2;

            //GUI
            tarotController.cardDrawAidContent.text = tarotController.aidText3;
            tarotController.buttonSuffleCards.SetActive(false);
            tarotController.buttonBackToMainMenu.SetActive(false);

        } 
        else if (positionDeterminer == 2 && CP.selectedCard.transform.name != deckCardLeft.transform.name)
        {

            deckCardCenter = CP.selectedCard.transform.gameObject;
            deckCardCenter.transform.DOMove(centerCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardCenter.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
                .SetRelative(false);
            

            positionDeterminer = 3;
            //GUI
            tarotController.cardDrawAidContent.text = tarotController.aidText4;

        }
        else if (positionDeterminer == 3 && CP.selectedCard.transform.name != deckCardLeft.transform.name && CP.selectedCard.transform.name != deckCardCenter.transform.name)
        {
            deckCardRight = CP.selectedCard.transform.gameObject;
            deckCardRight.transform.DOMove(rightCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardRight.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
                .SetRelative(false);


            //GUI
            tarotController.cardDrawAidContent.text = "";
            tarotController.buttonReveal.SetActive(true);

            positionDeterminer = 0;
        }

        Debug.Log("Card picked: " + CP.hitCardName);
    }




    public void cardPicked_2()
    {
        if (positionDeterminer == 1)
        {

            deckCardLeft = cards[randomNo1 - 1].transform.gameObject;
            deckCardLeft.transform.DOMove(leftCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardLeft.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
              .SetRelative(false);


            positionDeterminer = 2;

            //GUI
            tarotController.cardDrawAidContent.text = tarotController.aidText3;
            tarotController.buttonSuffleCards.SetActive(false);
            tarotController.buttonBackToMainMenu.SetActive(false);

        }
        else if (positionDeterminer == 2)
        {

            deckCardCenter = cards[randomNo2 - 1].transform.gameObject;
            deckCardCenter.transform.DOMove(centerCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardCenter.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
                .SetRelative(false);


            positionDeterminer = 3;
            //GUI
            tarotController.cardDrawAidContent.text = tarotController.aidText4;

        }
        else if (positionDeterminer == 3)
        {
            deckCardRight = cards[randomNo3 - 1].transform.gameObject;
            deckCardRight.transform.DOMove(rightCardPosition, drawSpeed).SetEase(Ease.InOutSine);
            deckCardRight.transform.DORotate(cardsRotation, drawSpeed, RotateMode.Fast)
                .SetRelative(false);


            //GUI
            tarotController.cardDrawAidContent.text = "";
            tarotController.buttonReveal.SetActive(true);

            positionDeterminer = 0;
            StartCoroutine(NewRandoms());

        }

        audio_cardSlide.Play();

    }

    private IEnumerator NewRandoms()
    {
        yield return new WaitForSeconds(3.5f);
        automaticCardPicker1();
    }


}
