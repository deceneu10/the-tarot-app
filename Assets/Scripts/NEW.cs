using UnityEngine;
using UnityEditor;
using Models;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;

public class NEW : MonoBehaviour
{
    public AnimatorOverrideController c3anim;


    private string newPost;

    private int c1;
    private int c2;
    private int c3;

    private int demoC1;
    private Vector3 cardPosition;
    private GameObject card1;

    public Button CardGet;
    public Button CardDestroy;

    // Start is called before the first frame update
    void Start()
    {
      //  rand3();
      //  StartCoroutine(horoscop());

      //  var root = GetComponent<UIDocument>().rootVisualElement;

       // CardGet = root.Q<Button>("CardGet");
       // CardDestroy = root.Q<Button>("CardDestroy");

      //  CardGet.clicked += pickAcard;
      //  CardDestroy.clicked += destroyCard;

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

    void rand3()
    {
        c1 = Random.Range(1, 45);

        c2 = Random.Range(1, 45);

        c3 = Random.Range(1, 45);

        Debug.Log("The 3 numbers are: " + c1 + " and " + c2 + " and " + c3);

    }


    void pickAcard()
    {
        demoC1 = Random.Range(1, 23);

        TarotDecks td1 = TarotDecks.FindEntity(entity => entity.No == demoC1);

        cardPosition = new Vector3(0.126f, 1.545f, -0.102f);

        card1 = Instantiate(
            td1.Deck1,
            cardPosition,
            Quaternion.Euler(180f, 180f, 0f),
            transform);

        //Quaternion.Euler(0f, 0f, 0f) X - control of side 180 show back 0 show face; y - control reversed position 0 normal 180 reversed

        Debug.Log("Card should be picked");
        card1.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        Animator animator = card1.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = c3anim as RuntimeAnimatorController;
    }

    void destroyCard()
    {
        Destroy(card1);
        Debug.Log("Card should get destroyed");
    }

}
