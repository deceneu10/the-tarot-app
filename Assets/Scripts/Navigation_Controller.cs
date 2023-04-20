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
using Toggle = UnityEngine.UI.Toggle;

public class Navigation_Controller : MonoBehaviour
{

    [Header("Dependencies")]
    public Camera Camera;
    public Fade_Script fadeFct;
    public Toggle YN_toggle;
    public TarotReading_YesNo tarotController;
    public CardController cardController;

    [Header("Elements")]
    public GameObject SettingsPanel;
    public GameObject SettingsPanel_main;
    public GameObject SettingsPanel_credits;

    private Animator settingsPanelAnimator;

    [Header("Flag - Reset instructions with 0")]
    public bool resetYesNoInstructions = false;

    private Vector3 cameraZeroAngle;
    private Vector3 cameraTarotReadingAngle;


    void Start()
    {
        Camera.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        cameraTarotReadingAngle = new Vector3(70f, 0f, 0f);


        if(resetYesNoInstructions == true)
        {

            PlayerPrefs.SetInt("Yes No How to Panel", 0); //Reset flag for showing the dialog box on how it works
            resetYesNoInstructions = false;

        }

        settingsPanelAnimator = SettingsPanel.GetComponent<Animator>();

    }



    void Update()
    {

    }

    public void ApplicationQuit()
    {

        fadeFct.FadeOutStartMenu(0.5f);
        StartCoroutine(ApplicationQuit_delay());

    }

    private IEnumerator ApplicationQuit_delay()
    {
        yield return new WaitForSeconds(0.65f);
        Application.Quit();
    }



    //----------- START OF YES-NO TAROT ----------------

    public void YesNo_HowTo_Toggle_Handler()
    {

        if(YN_toggle.isOn == true)
        {
            PlayerPrefs.SetInt("Yes No How to Panel", 1);

        } 
        else
        {
            PlayerPrefs.SetInt("Yes No How to Panel", 0);

        }

    }


    public void navigateToYNtarot()
    {
        StartCoroutine(navigateToYNtarot_delay());
    }

    private IEnumerator navigateToYNtarot_delay()
    {
        fadeFct.FadeOutStartMenu(1.5f);

        Camera.transform.DORotate(cameraTarotReadingAngle, 2f, RotateMode.Fast)
                .SetEase(Ease.InOutSine)
                .SetRelative(false);

        yield return new WaitForSeconds(1.5f);

        int YN_HIW = PlayerPrefs.GetInt("Yes No How to Panel", 0);

        if (YN_HIW == 0)
        {
            fadeFct.FadeIn_YN_HowTo(1.5f);

        }
        else if (YN_HIW == 1)
        {
            fadeFct.FadeIn_YN_MainFlow(1.5f);
            tarotController.hideSuffleBtn = false;
            cardController.positionDeterminer = 0;
            tarotController.cardDrawAidContent.text = tarotController.aidText1;
        }

    }

    public void navigateToYNtarot_MainFlow()
    {

        StartCoroutine(navigateToYNtarot_MainFlow_delay());

    }

    private IEnumerator navigateToYNtarot_MainFlow_delay()
    {
        tarotController.hideSuffleBtn = false;
        cardController.positionDeterminer = 0;
        tarotController.cardDrawAidContent.text = tarotController.aidText1;

        YesNo_HowTo_Toggle_Handler();

        fadeFct.FadeOut_YN_HowTo(0.5f);
        yield return new WaitForSeconds(0.65f);
        fadeFct.FadeIn_YN_MainFlow(1.5f);
    }


    public void BackTo_MainMenu_YNTarot()
    {
        StartCoroutine(BackTo_MainMenu_YNTarot_delay());
    }

    private IEnumerator BackTo_MainMenu_YNTarot_delay()
    {
        fadeFct.FadeOut_YN_MainFlow(1.5f);
        Camera.transform.DORotate(cameraZeroAngle, 2f, RotateMode.Fast)
                .SetEase(Ease.InOutSine)
                .SetRelative(false);

        yield return new WaitForSeconds(1.5f);

        fadeFct.FadeInStartMenu(1.5f);
        tarotController.newReading();
    }

    //----------- END OF YES-NO TAROT ----------------


    //----------- Settings Panel ----------------

    public void SettingsPanel_SHOW()
    {

        settingsPanelAnimator.SetInteger("settingsState", 1);
        SettingsPanel_main.SetActive(true);
        SettingsPanel_credits.SetActive(false);

    }

    public void SettingsPanel_HIDE()
    {

        settingsPanelAnimator.SetInteger("settingsState", 2);

    }

    public void SettingsPanel_goto_Credits()
    {

        SettingsPanel_main.SetActive(false);
        SettingsPanel_credits.SetActive(true);

    }

    public void SettingsPanel_backfrom_Credits()
    {

        SettingsPanel_main.SetActive(true);
        SettingsPanel_credits.SetActive(false);

    }

    public void SettingsPanel_RateApp()
    {

        Application.OpenURL("market://details?id=com.ZalmoxeLand.TheTarotApp");

    }



}
