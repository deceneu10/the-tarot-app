using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardShuffle : MonoBehaviour
{

    [Header("Dependencies")]
    public CardController cardController;
    public TarotReading_YesNo tarotController;


    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
    public GameObject card6;
    public GameObject card7;
    public GameObject card8;
    public GameObject card9;
    public GameObject card10;
    public GameObject card11;
    public GameObject card13;
    public GameObject card14;
    public GameObject card15;
    public GameObject card16;
    public GameObject card17;
    public GameObject card18;
    public GameObject card19;
    public GameObject card20;
    public GameObject card21;
    public GameObject card22;


    public float stackSpeed = 2f;
    public float splitSpeed = 2f;
    public float suffleSpeed= 2f;
    public float suffleInterlaceSpeed = 0.25f;


    private Vector3 card1_initialPosition;
    private Vector3 card2_initialPosition;
    private Vector3 card3_initialPosition;
    private Vector3 card4_initialPosition;
    private Vector3 card5_initialPosition;
    private Vector3 card6_initialPosition;
    private Vector3 card7_initialPosition;
    private Vector3 card8_initialPosition;
    private Vector3 card9_initialPosition;
    private Vector3 card10_initialPosition;
    private Vector3 card11_initialPosition;
    private Vector3 card13_initialPosition;
    private Vector3 card14_initialPosition;
    private Vector3 card15_initialPosition;
    private Vector3 card16_initialPosition;
    private Vector3 card17_initialPosition;
    private Vector3 card18_initialPosition;
    private Vector3 card19_initialPosition;
    private Vector3 card20_initialPosition;
    private Vector3 card21_initialPosition;
    private Vector3 card22_initialPosition;

    private Vector3 card1_initialRotation;
    private Vector3 card2_initialRotation;
    private Vector3 card3_initialRotation;
    private Vector3 card4_initialRotation;
    private Vector3 card5_initialRotation;
    private Vector3 card6_initialRotation;
    private Vector3 card7_initialRotation;
    private Vector3 card8_initialRotation;
    private Vector3 card9_initialRotation;
    private Vector3 card10_initialRotation;
    private Vector3 card11_initialRotation;
    private Vector3 card13_initialRotation;
    private Vector3 card14_initialRotation;
    private Vector3 card15_initialRotation;
    private Vector3 card16_initialRotation;
    private Vector3 card17_initialRotation;
    private Vector3 card18_initialRotation;
    private Vector3 card19_initialRotation;
    private Vector3 card20_initialRotation;
    private Vector3 card21_initialRotation;
    private Vector3 card22_initialRotation;

    private Vector3 sufflePosition1;
    private Vector3 sufflePosition2;
    private Vector3 sufflePosition3;
    private Vector3 sufflePosition4;
    private Vector3 sufflePosition5;
    private Vector3 sufflePosition6;
    private Vector3 sufflePosition7;
    private Vector3 sufflePosition8;
    private Vector3 sufflePosition9;
    private Vector3 sufflePosition10;
    private Vector3 sufflePosition11;
    private Vector3 sufflePosition13;
    private Vector3 sufflePosition14;
    private Vector3 sufflePosition15;
    private Vector3 sufflePosition16;
    private Vector3 sufflePosition17;
    private Vector3 sufflePosition18;
    private Vector3 sufflePosition19;
    private Vector3 sufflePosition20;
    private Vector3 sufflePosition21;
    private Vector3 sufflePosition22;

    private Vector3 spreadPosition1;
    private Vector3 spreadPosition2;
    private Vector3 spreadPosition3;
    private Vector3 spreadPosition4;
    private Vector3 spreadPosition5;
    private Vector3 spreadPosition6;
    private Vector3 spreadPosition7;
    private Vector3 spreadPosition8;
    private Vector3 spreadPosition9;
    private Vector3 spreadPosition10;
    private Vector3 spreadPosition11;
    private Vector3 spreadPosition13;
    private Vector3 spreadPosition14;
    private Vector3 spreadPosition15;
    private Vector3 spreadPosition16;
    private Vector3 spreadPosition17;
    private Vector3 spreadPosition18;
    private Vector3 spreadPosition19;
    private Vector3 spreadPosition20;
    private Vector3 spreadPosition21;
    private Vector3 spreadPosition22;


    private Vector3 suffleLeft1;
    private Vector3 suffleLeft2;
    private Vector3 suffleLeft3;
    private Vector3 suffleLeft4;
    private Vector3 suffleLeft5;
    private Vector3 suffleLeft6;
    private Vector3 suffleLeft7;
    private Vector3 suffleLeft8;
    private Vector3 suffleLeft9;
    private Vector3 suffleLeft10;

    private Vector3 suffleRight1;
    private Vector3 suffleRight2;
    private Vector3 suffleRight3;
    private Vector3 suffleRight4;
    private Vector3 suffleRight5;
    private Vector3 suffleRight6;
    private Vector3 suffleRight7;
    private Vector3 suffleRight8;
    private Vector3 suffleRight9;
    private Vector3 suffleRight10;
    private Vector3 suffleRight11;

        
        


    private Vector3 suffleRotationInitial;
    private Vector3 suffleRotation1;


    // Start is called before the first frame update
    void Start()
    {
        


        card1_initialPosition = card1.transform.position;
        card2_initialPosition = card2.transform.position;
        card3_initialPosition = card3.transform.position;
        card4_initialPosition = card4.transform.position;
        card5_initialPosition = card5.transform.position;
        card6_initialPosition = card6.transform.position;
        card7_initialPosition = card7.transform.position;
        card8_initialPosition = card8.transform.position;
        card9_initialPosition = card9.transform.position;
        card10_initialPosition = card10.transform.position;
        card11_initialPosition = card11.transform.position;
        card13_initialPosition = card13.transform.position;
        card14_initialPosition = card14.transform.position;
        card15_initialPosition = card15.transform.position;
        card16_initialPosition = card16.transform.position;
        card17_initialPosition = card17.transform.position;
        card18_initialPosition = card18.transform.position;
        card19_initialPosition = card19.transform.position;
        card20_initialPosition = card20.transform.position;
        card21_initialPosition = card21.transform.position;
        card22_initialPosition = card22.transform.position;

        card1_initialRotation = card1.transform.rotation.eulerAngles;
        card2_initialRotation = card2.transform.rotation.eulerAngles;
        card3_initialRotation = card3.transform.rotation.eulerAngles;
        card4_initialRotation = card4.transform.rotation.eulerAngles;
        card5_initialRotation = card5.transform.rotation.eulerAngles;
        card6_initialRotation = card6.transform.rotation.eulerAngles;
        card7_initialRotation = card7.transform.rotation.eulerAngles;
        card8_initialRotation = card8.transform.rotation.eulerAngles;
        card9_initialRotation = card9.transform.rotation.eulerAngles;
        card10_initialRotation = card10.transform.rotation.eulerAngles;
        card11_initialRotation = card11.transform.rotation.eulerAngles;
        card13_initialRotation = card13.transform.rotation.eulerAngles;
        card14_initialRotation = card14.transform.rotation.eulerAngles;
        card15_initialRotation = card15.transform.rotation.eulerAngles;
        card16_initialRotation = card16.transform.rotation.eulerAngles;
        card17_initialRotation = card17.transform.rotation.eulerAngles;
        card18_initialRotation = card18.transform.rotation.eulerAngles;
        card19_initialRotation = card19.transform.rotation.eulerAngles;
        card20_initialRotation = card20.transform.rotation.eulerAngles;
        card21_initialRotation = card21.transform.rotation.eulerAngles;
        card22_initialRotation = card22.transform.rotation.eulerAngles;



        sufflePosition1 = card11_initialPosition;
        sufflePosition2 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.01f, sufflePosition1.z);
        sufflePosition3 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.02f, sufflePosition1.z);
        sufflePosition4 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.03f, sufflePosition1.z);
        sufflePosition5 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.04f, sufflePosition1.z);
        sufflePosition6 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.05f, sufflePosition1.z);
        sufflePosition7 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.06f, sufflePosition1.z);
        sufflePosition8 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.07f, sufflePosition1.z);
        sufflePosition9 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.08f, sufflePosition1.z);
        sufflePosition10 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.09f, sufflePosition1.z);
        sufflePosition11 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.1f, sufflePosition1.z);
        sufflePosition13 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.11f, sufflePosition1.z);
        sufflePosition14 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.12f, sufflePosition1.z);
        sufflePosition15 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.13f, sufflePosition1.z);
        sufflePosition16 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.14f, sufflePosition1.z);
        sufflePosition17 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.15f, sufflePosition1.z);
        sufflePosition18 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.16f, sufflePosition1.z);
        sufflePosition19 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.17f, sufflePosition1.z);
        sufflePosition20 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.18f, sufflePosition1.z);
        sufflePosition21 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.19f, sufflePosition1.z);
        sufflePosition22 = new Vector3(sufflePosition1.x, sufflePosition1.y + 0.2f, sufflePosition1.z);

        suffleLeft1 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.5f, sufflePosition1.z);
        suffleLeft2 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.51f, sufflePosition1.z);
        suffleLeft3 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.52f, sufflePosition1.z);
        suffleLeft4 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.53f, sufflePosition1.z);
        suffleLeft5 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.54f, sufflePosition1.z);
        suffleLeft6 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.55f, sufflePosition1.z);
        suffleLeft7 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.56f, sufflePosition1.z);
        suffleLeft8 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.57f, sufflePosition1.z);
        suffleLeft9 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.58f, sufflePosition1.z);
        suffleLeft10 = new Vector3(sufflePosition1.x - 0.2f, sufflePosition1.y + 0.59f, sufflePosition1.z);

        suffleRight1 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.5f, sufflePosition1.z);
        suffleRight2 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.51f, sufflePosition1.z);
        suffleRight3 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.52f, sufflePosition1.z);
        suffleRight4 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.53f, sufflePosition1.z);
        suffleRight5 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.54f, sufflePosition1.z);
        suffleRight6 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.55f, sufflePosition1.z);
        suffleRight7 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.56f, sufflePosition1.z);
        suffleRight8 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.57f, sufflePosition1.z);
        suffleRight9 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.58f, sufflePosition1.z);
        suffleRight10 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.59f, sufflePosition1.z);
        suffleRight11 = new Vector3(sufflePosition1.x + 0.2f, sufflePosition1.y + 0.6f, sufflePosition1.z);


        spreadPosition1 = card1_initialPosition;
        spreadPosition2 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.01f, spreadPosition1.z);
        spreadPosition3 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.02f, spreadPosition1.z);
        spreadPosition4 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.03f, spreadPosition1.z);
        spreadPosition5 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.04f, spreadPosition1.z);
        spreadPosition6 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.05f, spreadPosition1.z);
        spreadPosition7 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.06f, spreadPosition1.z);
        spreadPosition8 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.07f, spreadPosition1.z);
        spreadPosition9 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.08f, spreadPosition1.z);
        spreadPosition10 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.09f, spreadPosition1.z);
        spreadPosition11 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.1f, spreadPosition1.z);
        spreadPosition13 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.11f, spreadPosition1.z);
        spreadPosition14 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.12f, spreadPosition1.z);
        spreadPosition15 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.13f, spreadPosition1.z);
        spreadPosition16 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.14f, spreadPosition1.z);
        spreadPosition17 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.15f, spreadPosition1.z);
        spreadPosition18 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.16f, spreadPosition1.z);
        spreadPosition19 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.17f, spreadPosition1.z);
        spreadPosition20 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.18f, spreadPosition1.z);
        spreadPosition21 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.19f, spreadPosition1.z);
        spreadPosition22 = new Vector3(spreadPosition1.x, spreadPosition1.y + 0.2f, spreadPosition1.z);




        suffleRotationInitial = new Vector3(0f, 0f, 0f);
        suffleRotation1 = new Vector3(0f, 180f, 0f);
    }

    public void stackCards()
    {
        card1.transform.DOMove(sufflePosition1, stackSpeed) .SetEase(Ease.InOutSine);
        card2.transform.DOMove(sufflePosition2, stackSpeed).SetEase(Ease.InOutSine);
        card3.transform.DOMove(sufflePosition3, stackSpeed).SetEase(Ease.InOutSine);
        card4.transform.DOMove(sufflePosition4, stackSpeed).SetEase(Ease.InOutSine);
        card5.transform.DOMove(sufflePosition5, stackSpeed).SetEase(Ease.InOutSine);
        card6.transform.DOMove(sufflePosition6, stackSpeed).SetEase(Ease.InOutSine);
        card7.transform.DOMove(sufflePosition7, stackSpeed).SetEase(Ease.InOutSine);
        card8.transform.DOMove(sufflePosition8, stackSpeed).SetEase(Ease.InOutSine);
        card9.transform.DOMove(sufflePosition9, stackSpeed).SetEase(Ease.InOutSine);
        card10.transform.DOMove(sufflePosition10, stackSpeed).SetEase(Ease.InOutSine);
        card11.transform.DOMove(sufflePosition11, stackSpeed).SetEase(Ease.InOutSine);
        card13.transform.DOMove(sufflePosition13, stackSpeed).SetEase(Ease.InOutSine);
        card14.transform.DOMove(sufflePosition14, stackSpeed).SetEase(Ease.InOutSine);
        card15.transform.DOMove(sufflePosition15, stackSpeed).SetEase(Ease.InOutSine);
        card16.transform.DOMove(sufflePosition16, stackSpeed).SetEase(Ease.InOutSine);
        card17.transform.DOMove(sufflePosition17, stackSpeed).SetEase(Ease.InOutSine);
        card18.transform.DOMove(sufflePosition18, stackSpeed).SetEase(Ease.InOutSine);
        card19.transform.DOMove(sufflePosition19, stackSpeed).SetEase(Ease.InOutSine);
        card20.transform.DOMove(sufflePosition20, stackSpeed).SetEase(Ease.InOutSine);
        card21.transform.DOMove(sufflePosition21, stackSpeed).SetEase(Ease.InOutSine);
        card22.transform.DOMove(sufflePosition22, stackSpeed).SetEase(Ease.InOutSine);

        card1.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card2.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card3.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card4.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card5.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card6.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card7.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card8.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card9.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card10.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card11.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card13.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card14.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card15.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card16.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card17.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card18.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card19.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card20.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card21.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);
        card22.transform.DORotate(suffleRotationInitial, stackSpeed, RotateMode.Fast);


    }

    public void splitCards()
    {
        card1.transform.DOMove(suffleLeft1, splitSpeed).SetEase(Ease.InOutSine);
        card2.transform.DOMove(suffleLeft2, splitSpeed).SetEase(Ease.InOutSine);
        card3.transform.DOMove(suffleLeft3, splitSpeed).SetEase(Ease.InOutSine);
        card4.transform.DOMove(suffleLeft4, splitSpeed).SetEase(Ease.InOutSine);
        card5.transform.DOMove(suffleLeft5, splitSpeed).SetEase(Ease.InOutSine);
        card6.transform.DOMove(suffleLeft6, splitSpeed).SetEase(Ease.InOutSine);
        card7.transform.DOMove(suffleLeft7, splitSpeed).SetEase(Ease.InOutSine);
        card8.transform.DOMove(suffleLeft8, splitSpeed).SetEase(Ease.InOutSine);
        card9.transform.DOMove(suffleLeft9, splitSpeed).SetEase(Ease.InOutSine);
        card10.transform.DOMove(suffleLeft10, splitSpeed).SetEase(Ease.InOutSine);


        card11.transform.DOMove(suffleRight1, splitSpeed).SetEase(Ease.InOutSine);
        card13.transform.DOMove(suffleRight2, splitSpeed).SetEase(Ease.InOutSine);
        card14.transform.DOMove(suffleRight3, splitSpeed).SetEase(Ease.InOutSine);
        card15.transform.DOMove(suffleRight4, splitSpeed).SetEase(Ease.InOutSine);
        card16.transform.DOMove(suffleRight5, splitSpeed).SetEase(Ease.InOutSine);
        card17.transform.DOMove(suffleRight6, splitSpeed).SetEase(Ease.InOutSine);
        card18.transform.DOMove(suffleRight7, splitSpeed).SetEase(Ease.InOutSine);
        card19.transform.DOMove(suffleRight8, splitSpeed).SetEase(Ease.InOutSine);
        card20.transform.DOMove(suffleRight9, splitSpeed).SetEase(Ease.InOutSine);
        card21.transform.DOMove(suffleRight10, splitSpeed).SetEase(Ease.InOutSine);
        card22.transform.DOMove(suffleRight11, splitSpeed).SetEase(Ease.InOutSine);


    }

    public void suffle1()
    {
        StartCoroutine(suffle1_timing_B());
    }


    private IEnumerator suffle1_timing()
    {
        card1.transform.DOMove(sufflePosition1, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card11.transform.DOMove(sufflePosition2, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card2.transform.DOMove(sufflePosition3, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card13.transform.DOMove(sufflePosition4, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card3.transform.DOMove(sufflePosition5, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card14.transform.DOMove(sufflePosition6, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card4.transform.DOMove(sufflePosition7, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card15.transform.DOMove(sufflePosition8, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card5.transform.DOMove(sufflePosition9, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card16.transform.DOMove(sufflePosition10, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card6.transform.DOMove(sufflePosition11, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card17.transform.DOMove(sufflePosition14, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card7.transform.DOMove(sufflePosition13, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card18.transform.DOMove(sufflePosition16, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card8.transform.DOMove(sufflePosition15, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card19.transform.DOMove(sufflePosition18, suffleSpeed).SetEase(Ease.InFlash);
        card20.transform.DOMove(sufflePosition20, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card9.transform.DOMove(sufflePosition17, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card21.transform.DOMove(sufflePosition22, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card10.transform.DOMove(sufflePosition19, suffleSpeed).SetEase(Ease.InFlash);
        yield return new WaitForSeconds(suffleInterlaceSpeed);
        card22.transform.DOMove(sufflePosition22, suffleSpeed).SetEase(Ease.InFlash);


    }

    private IEnumerator suffle1_timing_B()
    {
        card1.transform.DOMove(sufflePosition1, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card11.transform.DOMove(sufflePosition2, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card2.transform.DOMove(sufflePosition3, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card13.transform.DOMove(sufflePosition4, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card3.transform.DOMove(sufflePosition5, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card14.transform.DOMove(sufflePosition6, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card4.transform.DOMove(sufflePosition7, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card15.transform.DOMove(sufflePosition8, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card5.transform.DOMove(sufflePosition9, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card16.transform.DOMove(sufflePosition10, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card6.transform.DOMove(sufflePosition11, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card17.transform.DOMove(sufflePosition14, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card7.transform.DOMove(sufflePosition13, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card18.transform.DOMove(sufflePosition16, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card8.transform.DOMove(sufflePosition15, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card19.transform.DOMove(sufflePosition18, 0.1f);
        card20.transform.DOMove(sufflePosition20, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card9.transform.DOMove(sufflePosition17, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card21.transform.DOMove(sufflePosition22, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card10.transform.DOMove(sufflePosition19, 0.1f);
        yield return new WaitForSeconds(0.05f);
        card22.transform.DOMove(sufflePosition22, 0.1f);


    }

    public void splitCards2()
    {
        card2.transform.DOMove(suffleLeft1, splitSpeed).SetEase(Ease.InOutSine);
        card4.transform.DOMove(suffleLeft2, splitSpeed).SetEase(Ease.InOutSine);
        card6.transform.DOMove(suffleLeft3, splitSpeed).SetEase(Ease.InOutSine);
        card8.transform.DOMove(suffleLeft4, splitSpeed).SetEase(Ease.InOutSine);
        card10.transform.DOMove(suffleLeft5, splitSpeed).SetEase(Ease.InOutSine);
        card14.transform.DOMove(suffleLeft6, splitSpeed).SetEase(Ease.InOutSine);
        card16.transform.DOMove(suffleLeft7, splitSpeed).SetEase(Ease.InOutSine);
        card18.transform.DOMove(suffleLeft8, splitSpeed).SetEase(Ease.InOutSine);
        card20.transform.DOMove(suffleLeft9, splitSpeed).SetEase(Ease.InOutSine);
        card22.transform.DOMove(suffleLeft10, splitSpeed).SetEase(Ease.InOutSine);

        card2.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card4.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card6.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card8.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card10.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card14.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card16.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card18.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card20.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);
        card22.transform.DORotate(suffleRotation1, splitSpeed, RotateMode.Fast);


        card1.transform.DOMove(suffleRight1, splitSpeed).SetEase(Ease.InOutSine);
        card3.transform.DOMove(suffleRight2, splitSpeed).SetEase(Ease.InOutSine);
        card5.transform.DOMove(suffleRight3, splitSpeed).SetEase(Ease.InOutSine);
        card7.transform.DOMove(suffleRight4, splitSpeed).SetEase(Ease.InOutSine);
        card9.transform.DOMove(suffleRight5, splitSpeed).SetEase(Ease.InOutSine);
        card11.transform.DOMove(suffleRight6, splitSpeed).SetEase(Ease.InOutSine);
        card13.transform.DOMove(suffleRight7, splitSpeed).SetEase(Ease.InOutSine);
        card15.transform.DOMove(suffleRight8, splitSpeed).SetEase(Ease.InOutSine);
        card17.transform.DOMove(suffleRight9, splitSpeed).SetEase(Ease.InOutSine);
        card19.transform.DOMove(suffleRight10, splitSpeed).SetEase(Ease.InOutSine);
        card21.transform.DOMove(suffleRight11, splitSpeed).SetEase(Ease.InOutSine);

    }

    public void splitCards3()
    {
        card2.transform.DOMove(suffleLeft1, splitSpeed).SetEase(Ease.InOutSine);
        card4.transform.DOMove(suffleLeft2, splitSpeed).SetEase(Ease.InOutSine);
        card6.transform.DOMove(suffleLeft3, splitSpeed).SetEase(Ease.InOutSine);
        card8.transform.DOMove(suffleLeft4, splitSpeed).SetEase(Ease.InOutSine);
        card10.transform.DOMove(suffleLeft5, splitSpeed).SetEase(Ease.InOutSine);
        card14.transform.DOMove(suffleLeft6, splitSpeed).SetEase(Ease.InOutSine);
        card16.transform.DOMove(suffleLeft7, splitSpeed).SetEase(Ease.InOutSine);
        card18.transform.DOMove(suffleLeft8, splitSpeed).SetEase(Ease.InOutSine);
        card20.transform.DOMove(suffleLeft9, splitSpeed).SetEase(Ease.InOutSine);
        card22.transform.DOMove(suffleLeft10, splitSpeed).SetEase(Ease.InOutSine);

        card2.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card4.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card6.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card8.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card10.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card14.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card16.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card18.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card20.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);
        card22.transform.DORotate(suffleRotationInitial, splitSpeed, RotateMode.Fast);


        card1.transform.DOMove(suffleRight1, splitSpeed).SetEase(Ease.InOutSine);
        card3.transform.DOMove(suffleRight2, splitSpeed).SetEase(Ease.InOutSine);
        card5.transform.DOMove(suffleRight3, splitSpeed).SetEase(Ease.InOutSine);
        card7.transform.DOMove(suffleRight4, splitSpeed).SetEase(Ease.InOutSine);
        card9.transform.DOMove(suffleRight5, splitSpeed).SetEase(Ease.InOutSine);
        card11.transform.DOMove(suffleRight6, splitSpeed).SetEase(Ease.InOutSine);
        card13.transform.DOMove(suffleRight7, splitSpeed).SetEase(Ease.InOutSine);
        card15.transform.DOMove(suffleRight8, splitSpeed).SetEase(Ease.InOutSine);
        card17.transform.DOMove(suffleRight9, splitSpeed).SetEase(Ease.InOutSine);
        card19.transform.DOMove(suffleRight10, splitSpeed).SetEase(Ease.InOutSine);
        card21.transform.DOMove(suffleRight11, splitSpeed).SetEase(Ease.InOutSine);

    }

    public void masterSuffle()
    {
        StartCoroutine(masterSuffle_timing());
        
    }

    private IEnumerator masterSuffle_timing()
    {
        stackCards(); 
        yield return new WaitForSeconds(1.1f);
        splitCards(); //
        yield return new WaitForSeconds(1.5f);
        suffle1();
        yield return new WaitForSeconds(2.85f);//
        splitCards2();
        yield return new WaitForSeconds(1.5f);
        splitCards();
        yield return new WaitForSeconds(1.75f);
        suffle1();
        yield return new WaitForSeconds(2.7f);
        splitCards3();
        yield return new WaitForSeconds(1.5f);
        suffle1();
        yield return new WaitForSeconds(2.7f);
        spreadStartPosition();
        yield return new WaitForSeconds(1.2f);//
        StartCoroutine(spreadCards());

        cardController.positionDeterminer = 1;
    }


    public void shortSuffle()
    {
        cardController.positionDeterminer = 0;
        tarotController.hideSuffleBtn = true;
        tarotController.buttonBackToMainMenu.SetActive(false);

        StartCoroutine(shortSuffle_timing());

    }

    private IEnumerator shortSuffle_timing()
    {
        stackCards();
        yield return new WaitForSecondsRealtime(0.65f);
        splitCards(); //
        yield return new WaitForSecondsRealtime(1f);
        suffle1();
        yield return new WaitForSecondsRealtime(1.5f);
        spreadStartPosition();
        yield return new WaitForSecondsRealtime(1.25f);
        StartCoroutine(spreadCardsB());
        yield return new WaitForSecondsRealtime(1.75f);

        cardController.positionDeterminer = 1;
        tarotController.cardDrawAidContent.text = tarotController.aidText2;

        if(tarotController.hideSuffleBtn == true)
        {

            tarotController.buttonBackToMainMenu.SetActive(true);

        } else
        {
            tarotController.buttonSuffleCards.SetActive(true);
            tarotController.buttonBackToMainMenu.SetActive(true);
            tarotController.hideSuffleBtn = true;
        }


    }


    //Move suffled stack to the far left

    private void spreadStartPosition()
    {
        card1.transform.DOMove(spreadPosition1, stackSpeed).SetEase(Ease.InOutSine);
        card2.transform.DOMove(spreadPosition2, stackSpeed).SetEase(Ease.InOutSine);
        card3.transform.DOMove(spreadPosition3, stackSpeed).SetEase(Ease.InOutSine);
        card4.transform.DOMove(spreadPosition4, stackSpeed).SetEase(Ease.InOutSine);
        card5.transform.DOMove(spreadPosition5, stackSpeed).SetEase(Ease.InOutSine);
        card6.transform.DOMove(spreadPosition6, stackSpeed).SetEase(Ease.InOutSine);
        card7.transform.DOMove(spreadPosition7, stackSpeed).SetEase(Ease.InOutSine);
        card8.transform.DOMove(spreadPosition8, stackSpeed).SetEase(Ease.InOutSine);
        card9.transform.DOMove(spreadPosition9, stackSpeed).SetEase(Ease.InOutSine);
        card10.transform.DOMove(spreadPosition10, stackSpeed).SetEase(Ease.InOutSine);
        card11.transform.DOMove(spreadPosition11, stackSpeed).SetEase(Ease.InOutSine);
        card13.transform.DOMove(spreadPosition13, stackSpeed).SetEase(Ease.InOutSine);
        card14.transform.DOMove(spreadPosition14, stackSpeed).SetEase(Ease.InOutSine);
        card15.transform.DOMove(spreadPosition15, stackSpeed).SetEase(Ease.InOutSine);
        card16.transform.DOMove(spreadPosition16, stackSpeed).SetEase(Ease.InOutSine);
        card17.transform.DOMove(spreadPosition17, stackSpeed).SetEase(Ease.InOutSine);
        card18.transform.DOMove(spreadPosition18, stackSpeed).SetEase(Ease.InOutSine);
        card19.transform.DOMove(spreadPosition19, stackSpeed).SetEase(Ease.InOutSine);
        card20.transform.DOMove(spreadPosition20, stackSpeed).SetEase(Ease.InOutSine);
        card21.transform.DOMove(spreadPosition21, stackSpeed).SetEase(Ease.InOutSine);
        card22.transform.DOMove(spreadPosition22, stackSpeed).SetEase(Ease.InOutSine);

        card1.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card2.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card3.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card4.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card5.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card6.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card7.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card8.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card9.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card10.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card11.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card13.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card14.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card15.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card16.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card17.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card18.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card19.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card20.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card21.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);
        card22.transform.DORotate(card1_initialRotation, stackSpeed, RotateMode.Fast);


    }

    private IEnumerator spreadCards()
    {
        card2.transform.DOMove(card2_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card2.transform.DORotate(card2_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card3.transform.DOMove(card3_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card3.transform.DORotate(card3_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card4.transform.DOMove(card4_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card4.transform.DORotate(card4_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card5.transform.DOMove(card5_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card5.transform.DORotate(card5_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card6.transform.DOMove(card6_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card6.transform.DORotate(card6_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card7.transform.DOMove(card7_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card7.transform.DORotate(card7_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card8.transform.DOMove(card8_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card8.transform.DORotate(card8_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card9.transform.DOMove(card9_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card9.transform.DORotate(card9_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card10.transform.DOMove(card10_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card10.transform.DORotate(card10_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card11.transform.DOMove(card11_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card11.transform.DORotate(card11_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card13.transform.DOMove(card13_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card13.transform.DORotate(card13_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card14.transform.DOMove(card14_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card14.transform.DORotate(card14_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card15.transform.DOMove(card15_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card15.transform.DORotate(card15_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card16.transform.DOMove(card16_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card16.transform.DORotate(card16_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card17.transform.DOMove(card17_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card17.transform.DORotate(card17_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card18.transform.DOMove(card18_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card18.transform.DORotate(card18_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card19.transform.DOMove(card19_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card19.transform.DORotate(card19_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card20.transform.DOMove(card20_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card20.transform.DORotate(card20_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card21.transform.DOMove(card21_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card21.transform.DORotate(card21_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);
        card22.transform.DOMove(card22_initialPosition, stackSpeed).SetEase(Ease.InOutSine);
        card22.transform.DORotate(card22_initialRotation, stackSpeed, RotateMode.Fast);
        yield return new WaitForSeconds(0.1f);

    }

    private IEnumerator spreadCardsB()
    {
        card2.transform.DOMove(card2_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card2.transform.DORotate(card2_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card3.transform.DOMove(card3_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card3.transform.DORotate(card3_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card4.transform.DOMove(card4_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card4.transform.DORotate(card4_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card5.transform.DOMove(card5_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card5.transform.DORotate(card5_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card6.transform.DOMove(card6_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card6.transform.DORotate(card6_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card7.transform.DOMove(card7_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card7.transform.DORotate(card7_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card8.transform.DOMove(card8_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card8.transform.DORotate(card8_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card9.transform.DOMove(card9_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card9.transform.DORotate(card9_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card10.transform.DOMove(card10_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card10.transform.DORotate(card10_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card11.transform.DOMove(card11_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card11.transform.DORotate(card11_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card13.transform.DOMove(card13_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card13.transform.DORotate(card13_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card14.transform.DOMove(card14_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card14.transform.DORotate(card14_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card15.transform.DOMove(card15_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card15.transform.DORotate(card15_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card16.transform.DOMove(card16_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card16.transform.DORotate(card16_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card17.transform.DOMove(card17_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card17.transform.DORotate(card17_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card18.transform.DOMove(card18_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card18.transform.DORotate(card18_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card19.transform.DOMove(card19_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card19.transform.DORotate(card19_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card20.transform.DOMove(card20_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card20.transform.DORotate(card20_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card21.transform.DOMove(card21_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card21.transform.DORotate(card21_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);
        card22.transform.DOMove(card22_initialPosition, 0.1f).SetEase(Ease.InOutSine);
        card22.transform.DORotate(card22_initialRotation, 0.1f, RotateMode.Fast);
        yield return new WaitForSeconds(0.05f);

    }


}
