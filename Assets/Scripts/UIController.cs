using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    // NOTE: these are the actual gameObjects that is being rendered on the UI, not the UI elements themselves!
    [SerializeField] GameObject menuPlayButton;         
    [SerializeField] GameObject menuEscButton;          
    [SerializeField] GameObject endRestartButton;       
    [SerializeField] GameObject pausePlayButton;        
    [SerializeField] GameObject pauseEscButton;         
    [SerializeField] GameObject pauseRestartButton;

    [SerializeField] GameObject normalCow;
    [SerializeField] GameObject freezingCow;
    [SerializeField] GameObject hotCow;
    [SerializeField] GameObject nitroCow;
    [SerializeField] GameObject cloudCow;
    //////////////////////////////////////////////////

    [SerializeField] GameObject menuUIPanel;
    [SerializeField] GameObject gameUIPanel;
    [SerializeField] GameObject pauseUIPanel;
    [SerializeField] GameObject endUIPanel;

    Animator menuPlayButtonAnimator;
    Animator menuEscButtonAnimator;
    Animator endRestartButtonAnimator;
    Animator pausePlayButtonAnimator;
    Animator pauseEscButtonAnimator;
    Animator pauseRestartButtonAnimator;

    TextMeshProUGUI skeletonsScoreTMP;
    TextMeshProUGUI aliensScoreTMP;
    TextMeshProUGUI timerTMP;

    TextMeshProUGUI skeletonsNormalCowsCaptured;
    TextMeshProUGUI skeletonsFreezingCowsCaptured;
    TextMeshProUGUI skeletonsHotCowsCaptured;
    TextMeshProUGUI skeletonsNitroCowsCaptured;
    TextMeshProUGUI skeletonsCloudCowsCaptured;
    TextMeshProUGUI skeletonsFinalScore;

    TextMeshProUGUI aliensNormalCowsCaptured;
    TextMeshProUGUI aliensFreezingCowsCaptured;
    TextMeshProUGUI aliensHotCowsCaptured;
    TextMeshProUGUI aliensNitroCowsCaptured;
    TextMeshProUGUI aliensCloudCowsCaptured;
    TextMeshProUGUI aliensFinalScore;

    private void Awake()
    {
        menuPlayButtonAnimator = menuPlayButton.GetComponent<Animator>();
        if(!menuPlayButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find playButtonAnimator");

        menuEscButtonAnimator = menuEscButton.GetComponent<Animator>();
        if(!menuEscButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find escButtonAnimator");

        endRestartButtonAnimator = endRestartButton.GetComponent<Animator>();
        if(!endRestartButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find restartButtonAnimator");

        pausePlayButtonAnimator = pausePlayButton.GetComponent<Animator>();
        if(!pausePlayButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find pausePlayButtonAnimator");

        pauseEscButtonAnimator = pauseEscButton.GetComponent<Animator>();
        if(!pauseEscButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find pauseEscButtonAnimator");

        pauseRestartButtonAnimator = pauseRestartButton.GetComponent<Animator>();
        if(!pauseRestartButtonAnimator)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find pauseRestartButtonAnimator");

        skeletonsScoreTMP = gameUIPanel.transform.Find("SkeletonsScore").GetComponent<TextMeshProUGUI>();
        if(!skeletonsScoreTMP)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsScoreTMP");

        aliensScoreTMP = gameUIPanel.transform.Find("AliensScore").GetComponent<TextMeshProUGUI>();
        if(!aliensScoreTMP)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensScoreTMP");

        timerTMP = gameUIPanel.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        if(!timerTMP)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find timerTMP");

        skeletonsNormalCowsCaptured = 
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsNormalCowScore").GetComponent<TextMeshProUGUI>();
        if(!skeletonsNormalCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsNormalCowFinalScore");

        skeletonsFreezingCowsCaptured = 
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsFreezingCowScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsFreezingCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsFreezingCowFinalScore");

        skeletonsHotCowsCaptured = 
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsHotCowScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsHotCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsHotCowFinalScore");

        skeletonsNitroCowsCaptured = 
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsNitroCowScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsNitroCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsNitroCowFinalScore");

        skeletonsCloudCowsCaptured = 
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsCloudCowScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsCloudCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsCloudCowFinalScore");

        skeletonsFinalScore =
            endUIPanel.transform.Find("SkeletonsPanel").Find("SkeletonsTotalScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsFinalScore)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find skeletonsFinalScore");

        aliensNormalCowsCaptured =
            endUIPanel.transform.Find("AliensPanel").Find("AliensNormalCowScore").GetComponent<TextMeshProUGUI>();
        if (!aliensNormalCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensNormalCowFinalScore");

        aliensFreezingCowsCaptured =
            endUIPanel.transform.Find("AliensPanel").Find("AliensFreezingCowScore").GetComponent<TextMeshProUGUI>();
        if (!aliensFreezingCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensFreezingCowFinalScore");

        aliensHotCowsCaptured =
            endUIPanel.transform.Find("AliensPanel").Find("AliensHotCowScore").GetComponent<TextMeshProUGUI>();
        if (!aliensHotCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensHotCowFinalScore");

        aliensNitroCowsCaptured =
            endUIPanel.transform.Find("AliensPanel").Find("AliensNitroCowScore").GetComponent<TextMeshProUGUI>();
        if (!aliensNitroCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensNitroCowFinalScore");

        aliensCloudCowsCaptured =
            endUIPanel.transform.Find("AliensPanel").Find("AliensCloudCowScore").GetComponent<TextMeshProUGUI>();
        if (!aliensCloudCowsCaptured)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensCloudCowFinalScore");

        aliensFinalScore =
            endUIPanel.transform.Find("AliensPanel").Find("AliensTotalScore").GetComponent<TextMeshProUGUI>();
        if (!skeletonsFinalScore)
            Debug.Log("Error! In MenuUIController: void Awake(): Could not find aliensFinalScore");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.gameState)
        {
            case GameManager.GameState.MENU:
            {
                DisableGameUI();
                DisablePauseUI();
                DisableEndUI();
                EnableMenuUI();

            } break;

            case GameManager.GameState.GAME:
            {
                DisableMenuUI();
                DisablePauseUI();
                DisableEndUI();
                EnableGameUI();

                UpdateScores();
                UpdateTimer();

            } break;

            case GameManager.GameState.PAUSE:
            {
                DisableMenuUI();
                DisableGameUI();
                DisableEndUI();
                EnablePauseUI();

            } break;

            case GameManager.GameState.END:
            {
                DisableMenuUI();
                DisableGameUI();
                DisablePauseUI();
                EnableEndUI();

                UpdateFinalScores();

            } break;

            default:
            {
                DisableMenuUI();
                DisableGameUI();
                DisablePauseUI();
                DisableEndUI();

            } break;

        }
    }

    void UpdateScores()
    {
        skeletonsScoreTMP.text = GameManager.GetPlayerScore(GameManager.PlayerFaction.SKELETONS).ToString();
        aliensScoreTMP.text = GameManager.GetPlayerScore(GameManager.PlayerFaction.ALIENS).ToString();
    }

    void UpdateTimer()
    {
        timerTMP.text = GameManager.gameTimer.ToString();
    }

    void UpdateFinalScores()
    {
        skeletonsNormalCowsCaptured.text = "x " + GameManager.skeletons.nNormalCowsCaptured.ToString();
        skeletonsFreezingCowsCaptured.text = "x " + GameManager.skeletons.nFreezingCowsCaptured.ToString();
        skeletonsHotCowsCaptured.text = "x " + GameManager.skeletons.nHotCowsCaptured.ToString();
        skeletonsNitroCowsCaptured.text = "x " + GameManager.skeletons.nNitroCowsCaptured.ToString();
        skeletonsCloudCowsCaptured.text = "x " + GameManager.skeletons.nCloudCowsCaptured.ToString();
        skeletonsFinalScore.text = GameManager.skeletons.score.ToString();

        aliensNormalCowsCaptured.text = "x " + GameManager.aliens.nNormalCowsCaptured.ToString();
        aliensFreezingCowsCaptured.text = "x " + GameManager.aliens.nFreezingCowsCaptured.ToString();
        aliensHotCowsCaptured.text = "x " + GameManager.aliens.nHotCowsCaptured.ToString();
        aliensNitroCowsCaptured.text = "x " + GameManager.aliens.nNitroCowsCaptured.ToString();
        aliensCloudCowsCaptured.text = "x " + GameManager.aliens.nCloudCowsCaptured.ToString();
        aliensFinalScore.text = GameManager.aliens.score.ToString();
    }

    void EnableMenuUI()
    {
        menuUIPanel.SetActive(true);
    }

    void DisableMenuUI()
    {
        menuUIPanel.SetActive(false);
    }

    void EnableGameUI()
    {
        gameUIPanel.SetActive(true);
    }

    void DisableGameUI()
    {
        gameUIPanel.SetActive(false);
    }

    void EnablePauseUI()
    {
        pauseUIPanel.SetActive(true);
    }

    void DisablePauseUI()
    {
        pauseUIPanel.SetActive(false);
    }

    void EnableEndUI()
    {
        endUIPanel.SetActive(true);
    }

    void DisableEndUI()
    {
        endUIPanel.SetActive(false);
    }

    // menu play button mouse events
    public void MenuPlayButtonMouseClick()
    {
        menuPlayButtonAnimator.ResetTrigger("TriggerDefault");
        menuPlayButtonAnimator.SetTrigger("TriggerRotate");
        menuEscButtonAnimator.SetTrigger("TriggerShrink");
    }

    public void MenuPlayButtonMouseEnter()
    {
        menuPlayButtonAnimator.ResetTrigger("TriggerRotate");
        menuPlayButtonAnimator.ResetTrigger("TriggerDefault");
        menuPlayButtonAnimator.SetTrigger("TriggerShake");
    }

    public void MenuPlayButtonMouseExit()
    {
        menuPlayButtonAnimator.ResetTrigger("TriggerRotate");
        menuPlayButtonAnimator.ResetTrigger("TriggerShake");
        menuPlayButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////


    // menu esc button mouse events
    public void MenuEscButtonMouseClick()
    {
        menuEscButtonAnimator.ResetTrigger("TriggerDefault");
        menuEscButtonAnimator.SetTrigger("TriggerRotate");

        menuPlayButtonAnimator.SetTrigger("TriggerShrink");
    }

    public void MenuEscButtonMouseEnter()
    {
        menuEscButtonAnimator.ResetTrigger("TriggerRotate");
        menuEscButtonAnimator.ResetTrigger("TriggerDefault");
        menuEscButtonAnimator.SetTrigger("TriggerShake");
    }

    public void MenuEscButtonMouseExit()
    {
        menuEscButtonAnimator.ResetTrigger("TriggerRotate");
        menuEscButtonAnimator.ResetTrigger("TriggerShake");
        menuEscButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////


    // end restart button mouse events
    public void EndRestartButtonMouseClick()
    {
        endRestartButtonAnimator.ResetTrigger("TriggerDefault");
        endRestartButtonAnimator.ResetTrigger("TriggerShake");
        endRestartButtonAnimator.SetTrigger("TriggerRotate");
    }

    public void EndRestartButtonMouseEnter()
    {
        endRestartButtonAnimator.ResetTrigger("TriggerRotate");
        endRestartButtonAnimator.ResetTrigger("TriggerDefault");
        endRestartButtonAnimator.SetTrigger("TriggerShake");
    }

    public void EndRestartButtonMouseExit()
    {
        endRestartButtonAnimator.ResetTrigger("TriggerRotate");
        endRestartButtonAnimator.ResetTrigger("TriggerShake");
        endRestartButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////


    // pause play button mouse events
    public void PausePlayButtonMouseClick()
    {
        pausePlayButtonAnimator.ResetTrigger("TriggerDefault");
        pausePlayButtonAnimator.SetTrigger("TriggerRotate");

        pauseEscButtonAnimator.SetTrigger("TriggerShrink");
        pauseRestartButtonAnimator.SetTrigger("TriggerShrink");
    }

    public void PausePlayButtonMouseEnter()
    {
        pausePlayButtonAnimator.ResetTrigger("TriggerRotate");
        pausePlayButtonAnimator.ResetTrigger("TriggerDefault");
        pausePlayButtonAnimator.SetTrigger("TriggerShake");
    }

    public void PausePlayButtonMouseExit()
    {
        pausePlayButtonAnimator.ResetTrigger("TriggerRotate");
        pausePlayButtonAnimator.ResetTrigger("TriggerShake");
        pausePlayButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////


    // pause esc button mouse events
    public void PauseEscButtonMouseClick()
    {
        pauseEscButtonAnimator.ResetTrigger("TriggerDefault");
        pauseEscButtonAnimator.SetTrigger("TriggerRotate");

        pausePlayButtonAnimator.SetTrigger("TriggerShrink");
        pauseRestartButtonAnimator.SetTrigger("TriggerShrink");
    }

    public void PauseEscButtonMouseEnter()
    {
        pauseEscButtonAnimator.ResetTrigger("TriggerRotate");
        pauseEscButtonAnimator.ResetTrigger("TriggerDefault");
        pauseEscButtonAnimator.SetTrigger("TriggerShake");
    }

    public void PauseEscButtonMouseExit()
    {
        pauseEscButtonAnimator.ResetTrigger("TriggerRotate");
        pauseEscButtonAnimator.ResetTrigger("TriggerShake");
        pauseEscButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////


    // pause restart button mouse events
    public void PauseRestartButtonMouseClick()
    {
        pauseRestartButtonAnimator.ResetTrigger("TriggerDefault");
        pauseRestartButtonAnimator.ResetTrigger("TriggerShake");
        pauseRestartButtonAnimator.SetTrigger("TriggerRotate");

        pauseEscButtonAnimator.SetTrigger("TriggerShrink");
        pausePlayButtonAnimator.SetTrigger("TriggerShrink");
    }

    public void PauseRestartButtonMouseEnter()
    {
        pauseRestartButtonAnimator.ResetTrigger("TriggerRotate");
        pauseRestartButtonAnimator.ResetTrigger("TriggerDefault");
        pauseRestartButtonAnimator.SetTrigger("TriggerShake");
    }

    public void PauseRestartButtonMouseExit()
    {
        pauseRestartButtonAnimator.ResetTrigger("TriggerRotate");
        pauseRestartButtonAnimator.ResetTrigger("TriggerShake");
        pauseRestartButtonAnimator.SetTrigger("TriggerDefault");
    }
    /////////////////////////////////////

}
