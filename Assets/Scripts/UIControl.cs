using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/// <summary>
/// Handles any text updates within the UI
/// </summary>
public class UIControl : MonoBehaviour
{
    /// <summary>
    /// The fade image that appears during a transition
    /// </summary>



    public enum TransitionEffect
    {
        InstantIn,
        InstantOut,
        CircleIn,
        CircleOut,
        SquareIn,
        SquareOut,
        SlideInFromRight,
        SlideOutToRight,
        SlideInFromLeft,
        SlideOutToLeft,
        SlideInFromUp,
        SlideOutToUp,
        SlideInFromDown,
        SlideOutToDown,
    }



    /// <summary>
    /// The Object that contains the in-game UI.
    /// </summary>
    [SerializeField] GameObject gameUI;
    [SerializeField] private GameObject gameUIOrbs;
    [SerializeField] private GameObject gameUIBattery;
    [SerializeField] private GameObject gameUITask;
    [SerializeField] private GameObject gameUIHealth;
    [SerializeField] private GameObject gameUIReticle;

    [SerializeField] private TMPro.TMP_Text orbText;
    [SerializeField] private TMPro.TMP_Text batteryText;
    [SerializeField] private TMPro.TMP_Text taskText;

    /// <summary>
    /// The UI image of the pause menu
    /// </summary>
    [SerializeField] GameObject pause;

    /// <summary>
    /// The player's current number of keys
    /// </summary>
    [SerializeField] int currentKeys;

    /// <summary>
    /// The player's current amount of fuel
    /// </summary>
    [SerializeField] int currentFuel;

    [SerializeField] TMP_Text transitionText;


    /// <summary>
    /// The fade image that appears during a transition
    /// </summary>
    [SerializeField] Graphic fadeImage;
    [SerializeField] RawImage healthIcon;

    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] TMP_Text interactText;

    [SerializeField] GameObject pauseMenuSelectedGameObject;

    

    /*
    Temporary Values, should not be editable
    */

    private RectTransform orbRect;
    private RectTransform batteryRect;
    private RectTransform taskRect;
    private RectTransform healthRect;

    private Vector3 orbPos;
    private Vector3 batteryPos;
    private Vector3 taskPos;
    private Vector3 healthPos;

    private Vector3 orbPos_Out;
    private Vector3 batteryPos_Out;
    private Vector3 taskPos_Out;
    private Vector3 healthPos_Out;

    private Coroutine orbCor;
    private Coroutine batteryCor;
    private Coroutine taskCor;
    private Coroutine healthCor;

    private bool orbCorActive;
    private bool batteryCorActive;
    private bool taskCorActive;
    private bool healthCorActive;

    string colorTag = "<color=#00000000>";



    /*
    Functions
    */

    //var for the main menu and pause menu highlight logic 
    [Header("var for the main menu")]

    [SerializeField] private GameObject creditGameObject;

   [SerializeField] private GameObject controlGameObject;

   [SerializeField] private GameObject levelGameObject;

   [SerializeField] private GameObject menu;

   [SerializeField] private string sceneToLoad;


  // [SerializeField] private GameObject loadingScreen;

   //[SerializeField] Slider loadingBar;

   [SerializeField] private EventSystem eventSystem;


    [Header("Buttons that get highlighted from the event system")]
    [SerializeField]private GameObject levelMenuFirstSelected;

    [SerializeField]private GameObject controlMenuFirstSelected;

    [SerializeField]private GameObject creditsMenuFirstSelected;

    [SerializeField] private GameObject playGameButton;

    [SerializeField] private GameObject pauseMenuControl;

    [SerializeField]private GameObject settingsPauseFirstSelected;

    [SerializeField]private GameObject settingsMenuFirstSelected;

    [SerializeField]private GameObject menuTwoFirstSelectedButton;

    /// <summary>
    /// used to fix a bug with the settings menu
    /// </summary>
    private bool isNotInMenuTwo=false;
    /// <summary>
    /// is game loading
    /// </summary>
    private bool gameLoading = false;
    /// <summary>
    /// is the game paused
    /// </summary>
    private bool isPaused=false;
    /// <summary>
    /// used to fix a bug where the new game button would not be shown highlighted
    /// </summary>
    private bool hasSelectedMenuTwoButton=false;

    void Start()
    {
        SetGameUIPositions();
        ReticleActive();

        HideOrbs(0);
        HideBatteries(0);
        HideTask(0);
        HideHealth(0);
    }
    private void Update() {
        
       if(SceneManager.GetActiveScene().name!="Menu2"){
          
           hasSelectedMenuTwoButton=false;
       }
         if(SceneManager.GetActiveScene().name=="Menu2" && !hasSelectedMenuTwoButton){
         
            ChangeActiveButton(GameObject.FindGameObjectWithTag("PlayButton"));
            hasSelectedMenuTwoButton=true;
        
        
         }
       
      
        if(!isNotInMenuTwo&&SceneManager.GetActiveScene().name!="Menu2"){
            isNotInMenuTwo=true;
            ChangeActiveButton(settingsPauseFirstSelected);

        }
         if(SceneManager.GetActiveScene().name=="Credits"){
            ChangeActiveButton(GameObject.FindGameObjectWithTag("endcredit"));
        }
       
        
        
    
    }

    /// <summary>
    /// Sets the default value positions for all GameUI objects
    /// </summary>
    private void SetGameUIPositions()
    {
        orbRect = gameUIOrbs.GetComponent<RectTransform>();
        batteryRect = gameUIBattery.GetComponent<RectTransform>();
        taskRect = gameUITask.GetComponent<RectTransform>();
        healthRect = gameUIHealth.GetComponent<RectTransform>();

        orbPos = orbRect.anchoredPosition;
        batteryPos = batteryRect.anchoredPosition;
        taskPos = taskRect.anchoredPosition;
        healthPos = healthRect.anchoredPosition;

        orbPos_Out = orbPos + Vector3.left * 500f;
        batteryPos_Out = batteryPos + Vector3.right * 500f;
        taskPos_Out = taskPos + Vector3.right * 500f;
        healthPos_Out = healthPos + Vector3.up * 200f;
    }

    /// <summary>
    /// Updates the UI text for the player's current orb count.
    /// </summary>
    /// <param name="newValue"> The new orb amount. </param>
    public void OrbUpdate(int newValue)
    {
        //Update the UI text for the player's orb to the new value
        orbText.text = "" + newValue;

        if (orbCorActive)
        {
            StopCoroutine(orbCor);
        }

        orbCor = StartCoroutine(ShowOrbsOverTime(1, 0.25f));
    }

    /// <summary>
    /// Updates the UI text for the player's current battery count.
    /// </summary>
    /// <param name="newValue"> The new battery amount. </param>
    public void BatteryUpdate(int newValue)
    {
        //Update the UI text for the player's battery to the new value
        batteryText.text = "" + newValue;

        if (batteryCorActive)
        {
            StopCoroutine(batteryCor);
        }

        StartCoroutine(ShowBatteriesOverTime(5, 0.5f));
    }

    /// <summary>
    /// Updates the UI text for the player's current objective.
    /// </summary>
    /// <param name="newTask"> The text value for the player's new objective</param>
    public void TaskUpdate(string newTask)
    {
        //Update player's current objective in the UI.
        taskText.text = newTask;

        if (taskCorActive)
        {
            StopCoroutine(taskCor);
        }

        StartCoroutine(ShowTaskOverTime(5, 0.5f));

    }

    /// <summary>
    /// Updates the UI text for the player's current objective.
    /// </summary>
    public void HealthUpdate()
    {
        if (healthCorActive)
        {
            try
            {
                StopCoroutine(healthCor);
            }
            catch
            {
                //Debug.Log("healthCor is null");
            }
        }

        StartCoroutine(ShowHealthOverTime(3, 0.5f));
    }

    /// <summary>
    /// Turns on the pause menu and freezes the game.
    /// </summary>
    public void pauseUpdate()
    {
      GameObject settingsPause=GameObject.FindGameObjectWithTag("pausesetting");
        if(settingsPause!=null&&settingsPause.activeInHierarchy){
            print("true");
             SettingsControl settings= GameObject.FindGameObjectWithTag("root").GetComponent<SettingsControl>();

            settings.ToogleSettingsUI(false);

            settings.TooglePauseUI(true);
        }
        //if game is paused
        isPaused=Time.timeScale==0f;
        pause.SetActive(isPaused);
        if(isPaused){
            PauseButtonHighlightLogic();
            SoundSystem.SoundType pauseSound = RootScript.SoundManager.FindSoundTypeByString("Pause");
            RootScript.SoundManager.PlaySound(pauseSound);

        }
      
        
        
    }
    /// <summary>
    /// changes the active button when the pause menu button is selected 
    /// </summary>
    void PauseButtonHighlightLogic(){
       
        ChangeActiveButton(pauseMenuSelectedGameObject);
    }

    



    /// <summary>
    /// Return the health raw image
    /// </summary>
    /// <returns>health raw image</returns>
    public RawImage GetHealthRawImage()
    {
        return healthIcon;
    }

    /*
    Dialogue
    */

    /// <summary>
    /// Start dialogue
    /// </summary>
    /// <param name="trigger">The dialogue trigger</param>
    public void StartDialogue(DialogueTrigger trigger)
    {
        dialogueManager.StartDialogue(trigger);
    }

    /// <summary>
    /// Show the interact text
    /// </summary>
    /// <param name="active">Whether or not it should be shown</param>
    public void ShowInteractText(bool active)
    {
        if (active)
        {
            if (RootScript.Input.GetInputDeviceClass() == InControl.InputDeviceClass.Controller)
            {
                interactText.text = "Press B to interact";
            }
            else
            {
                interactText.text = "Press E to interact";
            }
        }

        interactText.gameObject.SetActive(active);
    }

    /// <summary>
    /// Returns whether or not dialogue is being shown
    /// </summary>
    /// <returns>Whether or not dialogue is shown</returns>
    public bool DialgoueIsActive()
    {
        return dialogueManager.DialgoueIsActive();
    }


    /*
    Showing and Hiding UI
    */

    /// <summary>
    /// Show all in-game ui over time
    /// </summary>
    /// <param name="duration">The time the ui will appear on screen</param>
    /// <param name="transitionTime">Time the tween will take</param>
    public void ShowAllUIOverTime(float duration, float transitionTime)
    {
        StartCoroutine(ShowOrbsOverTime(duration, transitionTime));
        StartCoroutine(ShowBatteriesOverTime(duration, transitionTime));
        StartCoroutine(ShowTaskOverTime(duration, transitionTime));
        StartCoroutine(ShowHealthOverTime(duration, transitionTime));
    }

    /// <summary>
    /// Show all in-game ui over time
    /// </summary>
    /// <param name="duration">The time the ui will appear on screen</param>
    /// <param name="transitionTime">Time the tween will take</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator ShowAllUIOverTimeCoroutine(float duration, float transitionTime)
    {
        ShowOrbs(transitionTime);
        ShowBatteries(transitionTime);
        ShowTask(transitionTime);
        ShowHealth(transitionTime);

        yield return new WaitForSeconds(transitionTime + duration);
        HideOrbs(transitionTime);
        HideBatteries(transitionTime);
        HideTask(transitionTime);
        HideHealth(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        
    }

    /// <summary>
    /// Show all in-game ui at once
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator ShowAllUICoroutine(float transitionTime)
    {
        gameUI.SetActive(true);
        ShowOrbs(transitionTime);
        ShowBatteries(transitionTime);
        ShowTask(transitionTime);
        ShowHealth(transitionTime);
        yield return null;
    }

    /// <summary>
    /// Show all in-game ui at once
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    public void ShowAllUI(float transitionTime)
    {
        ShowOrbs(transitionTime);
        ShowBatteries(transitionTime);
        ShowTask(transitionTime);
        ShowHealth(transitionTime);
    }

    /// <summary>
    /// Hide all in-game ui at once
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    public void HideAllUI(float transitionTime)
    {
        HideOrbs(transitionTime);
        HideBatteries(transitionTime);
        HideTask(transitionTime);
        HideHealth(transitionTime);
    }

    /// <summary>
    /// Show the graphic - orbs
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void ShowOrbs(float transitionTime)
    {
        LeanTween.move(orbRect, orbPos, transitionTime).setEase( LeanTweenType.easeOutQuad );
    }

    /// <summary>
    /// Show the graphic - batteries
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void ShowBatteries(float transitionTime)
    {
        LeanTween.move(batteryRect, batteryPos, transitionTime).setEase( LeanTweenType.easeOutQuad );
    }

    /// <summary>
    /// Show the graphic - task
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void ShowTask(float transitionTime)
    {
        LeanTween.move(taskRect, taskPos, transitionTime).setEase( LeanTweenType.easeOutQuad );
    }

    /// <summary>
    /// Show the graphic - health
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void ShowHealth(float transitionTime)
    {
        LeanTween.move(healthRect, healthPos, transitionTime).setEase( LeanTweenType.easeOutBounce );
    }

    /// <summary>
    /// Hide the graphic - orbs
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void HideOrbs(float transitionTime)
    {
        LeanTween.move(orbRect, orbPos_Out, transitionTime).setEase( LeanTweenType.easeOutSine );
    }

    /// <summary>
    /// Hide the graphic - batteries
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void HideBatteries(float transitionTime)
    {
        LeanTween.move(batteryRect, batteryPos_Out, transitionTime).setEase( LeanTweenType.easeOutSine );
    }

    /// <summary>
    /// Hide the graphic - task
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void HideTask(float transitionTime)
    {
        LeanTween.move(taskRect, taskPos_Out, transitionTime).setEase( LeanTweenType.easeOutSine );
    }

    /// <summary>
    /// Hide the graphic - health
    /// </summary>
    /// <param name="transitionTime">Time the tween will take</param>
    void HideHealth(float transitionTime)
    {
        LeanTween.move(healthRect, healthPos_Out, transitionTime).setEase( LeanTweenType.easeOutSine );
    }

    /// <summary>
    /// Showing a graph over a period of time - Orb
    /// </summary>
    /// <param name="duration">The time that the graphic will be shown on-screen</param>
    /// <param name="transitionTime">The time the transition will take</param>
    /// <returns>IEnumerator</returns>
    IEnumerator ShowOrbsOverTime(float duration, float transitionTime)
    {
        orbCorActive = true;

        ShowOrbs(transitionTime);
        yield return new WaitForSeconds(duration);
        HideOrbs(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        
        orbCorActive = false;
    }

    /// <summary>
    /// Showing a graph over a period of time - Batteries
    /// </summary>
    /// <param name="duration">The time that the graphic will be shown on-screen</param>
    /// <param name="transitionTime">The time the transition will take</param>
    /// <returns>IEnumerator</returns>
    IEnumerator ShowBatteriesOverTime(float duration, float transitionTime)
    {
        batteryCorActive = true;
        
        ShowBatteries(transitionTime);
        yield return new WaitForSeconds(duration);
        HideBatteries(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        
        batteryCorActive = false;
    }

    /// <summary>
    /// Showing a graph over a period of time - Task
    /// </summary>
    /// <param name="duration">The time that the graphic will be shown on-screen</param>
    /// <param name="transitionTime">The time the transition will take</param>
    /// <returns>IEnumerator</returns>
    IEnumerator ShowTaskOverTime(float duration, float transitionTime)
    {
        taskCorActive = true;

        ShowTask(transitionTime);
        yield return new WaitForSeconds(duration);
        HideTask(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        
        taskCorActive = false;
    }

    /// <summary>
    /// Showing a graph over a period of time - Health
    /// </summary>
    /// <param name="duration">The time that the graphic will be shown on-screen</param>
    /// <param name="transitionTime">The time the transition will take</param>
    /// <returns>IEnumerator</returns>
    IEnumerator ShowHealthOverTime(float duration, float transitionTime)
    {
        healthCorActive = true;

        ShowHealth(transitionTime);
        yield return new WaitForSeconds(duration);
        HideHealth(transitionTime);
        yield return new WaitForSeconds(transitionTime);

        healthCorActive = false;
    }

    /*
    Transition Effects
    */
    
    public IEnumerator PerformEffect_Instant(float duration)
    {
        fadeImage.enabled = true;
        yield return new WaitForSeconds(duration);
        fadeImage.enabled = false;
        yield return null;
    }

    public IEnumerator PerformEffect_SlideInFromLeft(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(1920, 0, 0);
        LeanTween.move(rect, new Vector3(0,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator PerformEffect_SlideInFromRight(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(-1920, 0, 0);
        LeanTween.move(rect, new Vector3(0,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator PerformEffect_SlideInFromUp(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, 1080, 0);
        LeanTween.move(rect, new Vector3(0,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator PerformEffect_SlideInFromDown(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, -1080, 0);
        LeanTween.move(rect, new Vector3(0,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
    }

    public IEnumerator PerformEffect_SlideOutToLeft(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, 0, 0);
        LeanTween.move(rect, new Vector3(-1920,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
        fadeImage.enabled = false;
        yield return null;
    }

    public IEnumerator PerformEffect_SlideOutToRight(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, 0, 0);
        LeanTween.move(rect, new Vector3(1920,0,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
        fadeImage.enabled = false;
        yield return null;
    }

    public IEnumerator PerformEffect_SlideOutToUp(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, 0, 0);
        LeanTween.move(rect, new Vector3(0,1080,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
        fadeImage.enabled = false;
        yield return null;
    }

    public IEnumerator PerformEffect_SlideOutToDown(float duration, LeanTweenType type)
    {
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, 0, 0);
        LeanTween.move(rect, new Vector3(0,-1080,0), duration).setEase( type );
        fadeImage.enabled = true;

        yield return new WaitForSeconds(duration);
        fadeImage.enabled = false;
        yield return null;
    }


    /*Menu control and pause menu control functions*/


     /// <summary>
     /// loads the level when the user hits the play button
     /// </summary>
     /// <param name="level"></param>
    public void PlayButton(string level)
    {
        if (!gameLoading)
        {
            gameLoading = true;
            sceneToLoad = level;
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.LoadAllOfScene(sceneToLoad, true);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 1f, LeanTweenType.easeOutSine);
            RootScript.GlobalQueue.AddCoroutine(RootScript.UIElements.ShowAllUIOverTimeCoroutine(5f, 0.5f), "Show All UI", false);
            ReticleActive();

        }
        
    }

    public void ReturnToMenuButton(string level)
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        
        if (!gameLoading)
        {
            gameLoading = true;
            sceneToLoad = level;
            RootScript.UIElements.HideAllUI(0.5f);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.LoadAllOfScene(sceneToLoad, true);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 1f, LeanTweenType.easeOutSine);
            ReticleActive();
        }
        
        pause.SetActive(false);
        ResetOrbAndBattery();

    }
    //used to reset the batterys and orbs
    private void ResetOrbAndBattery(){
        RootScript.TheGameManager.ResetBatterys();
        RootScript.TheGameManager.ResetOrbs();
        OrbUpdate(RootScript.TheGameManager.GetCurrentOrbs());
        BatteryUpdate(RootScript.TheGameManager.GetBatterys());
    }
    /// <summary>
    /// credit button menu logic
    /// </summary>
    public void CreditButton(){
        menu.SetActive(false);
        controlGameObject.SetActive(false);
        levelGameObject.SetActive(false);
        creditGameObject.SetActive(true);
       

        ChangeActiveButton(creditsMenuFirstSelected);

        



    }
    /// <summary>
    /// level button button logic
    /// </summary>
    public void LevelSelectButton()
    {
        menu.SetActive(false);
        controlGameObject.SetActive(false);
        creditGameObject.SetActive(false);
        levelGameObject.SetActive(true);

        

        ChangeActiveButton(levelMenuFirstSelected);

        
    }
    /// <summary>
    /// control button button logic
    /// </summary>
    public void ControlButton(){
        menu.SetActive(false);
        creditGameObject.SetActive(false);
        levelGameObject.SetActive(false);
        controlGameObject.SetActive(true);

        
        ChangeActiveButton(controlMenuFirstSelected);

    }
    /// <summary>
    /// called when the user hits the back button in the menu
    /// </summary>
    public void BackButton(){
        creditGameObject.SetActive(false);
        controlGameObject.SetActive(false);
        levelGameObject.SetActive(false);
        menu.SetActive(true);

        
        ChangeActiveButton(playGameButton);
    
    }


    /// <summary>
    /// quits the game
    /// </summary>
    public void Exit(){
        Application.Quit();
    }
    /// <summary>
    /// reloads the current level
    /// </summary>
    public void ReloadLevel(){
        if(isPaused){
            pause.SetActive(false);
        }
        Scene sceneToLoad= SceneManager.GetActiveScene();
        Time.timeScale=1;
        
    
        SceneManager.LoadScene(sceneToLoad.name);
        ReticleActive();
    }
    /// <summary>
    /// change the active button for the UI 
    /// </summary>
    /// <param name="newActiveButton"></param>
    public void ChangeActiveButton(GameObject newActiveButton){
        //remove current selected game object
        
        eventSystem.SetSelectedGameObject(null);
        //add the back button as the currently selected game object
       
        
        eventSystem.SetSelectedGameObject(newActiveButton);

        
        
    }

    public void SetLoadingDone()
    {
        gameLoading = false;
    }

    public void ReticleActive()
    {
        if (SceneManager.GetActiveScene().name == "MidpointTest")
        {
            gameUIReticle.SetActive(true);
            if (RootScript.CharMove != null) RootScript.CharMove.grapplingHookObject.SetActive(true);
            
        }
        else
        {
            gameUIReticle.SetActive(false);
            if (RootScript.CharMove != null) RootScript.CharMove.grapplingHookObject.SetActive(false);

        }
    }

    /// <summary>
    /// Display transition text during transition
    /// </summary>
    /// <param name="text">Text to display</param>
    /// <param name="postDisplayWait">Time to wait after all text is displayed</param>
    /// <param name="characterWait">Time between each character displayed</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayTransitionText(string text, float postDisplayWait = 5f, float characterWait = 0.01f)
    {
        transitionText.text = text;
        
        // Set the whole text transparent
        transitionText.color = new Color
            (
                transitionText.color.r,
                transitionText.color.g,
                transitionText.color.b,
                0
            );
        // Need to force the text object to be generated so we have valid data to work with right from the start.
        transitionText.ForceMeshUpdate();
 
 
        TMP_TextInfo textInfo = transitionText.textInfo;
        Vector3[] newVertexPositions;
        Color32[] newVertexColors;
 
        int currentCharacter = 0;
        int startingCharacterRange = currentCharacter;
        bool isRangeMax = false;

        newVertexPositions = textInfo.meshInfo[0].vertices;
 
        while (!isRangeMax)
        {
            int characterCount = textInfo.characterCount;
 
            // Spread should not exceed the number of characters.
            byte fadeSteps = (byte)Mathf.Max(1, 255 / 10f);
 
            for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
            {
                // Skip characters that are not visible (like white spaces)
                if (!textInfo.characterInfo[i].isVisible) continue;
 
                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
 
                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
 
                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
 
                // Get the current character's alpha value.
                byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);
 
                // Set new alpha values.
                newVertexColors[vertexIndex + 0].a = alpha;
                newVertexColors[vertexIndex + 1].a = alpha;
                newVertexColors[vertexIndex + 2].a = alpha;
                newVertexColors[vertexIndex + 3].a = alpha;

                // Get the vertex positions

                // Update vertex positions.
                newVertexPositions[vertexIndex + 0].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 1].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 2].y += (255-alpha)/100f;
                newVertexPositions[vertexIndex + 3].y += (255-alpha)/100f;
 
                if (alpha == 255)
                {
                    startingCharacterRange += 1;
 
                    if (startingCharacterRange == characterCount)
                    {
                        // Update mesh vertex data one last time.
                        transitionText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                        isRangeMax = true; // Would end the coroutine.
 
                        // Reset our counters.
                        currentCharacter = 0;
                        startingCharacterRange = 0;
                        
                    }
                }
            }
 
            // Upload the changed vertex colors to the Mesh.
            transitionText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                transitionText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
 
            if (currentCharacter + 1 < characterCount) currentCharacter += 1;
 
            yield return new WaitForSeconds(0.25f - 24f * 0.01f);
        }

        yield return new WaitForSeconds(postDisplayWait);
        transitionText.text = "";

    }
    public void SetSettingsPauseMenuFirstSelected(){
        ChangeActiveButton(settingsPauseFirstSelected);
    }
    public void SetSettingsMenuFirstSelected(){
        ChangeActiveButton(settingsMenuFirstSelected);
    }

    

    
   


}

