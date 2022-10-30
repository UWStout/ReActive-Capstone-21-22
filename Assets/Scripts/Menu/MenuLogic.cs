using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuLogic : MonoBehaviour
{
    [Header ("Menus")]

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header ("First Selected Menus")]

    [SerializeField] private GameObject mainMenuFirstButton;
    [SerializeField] private GameObject controlsMenuFirstButton;
    [SerializeField] private GameObject creditsMenuFirstButton;
    [SerializeField] private GameObject levelSelectMenuFirstButton;

    [SerializeField] private GameObject settingsSelectMenuFirstButton;

    [SerializeField] private MusicTrack menuMusic;


    private string targetLevel = "Intro";

    ///[Header ("Dependencies")]

    private EventSystem eventSystem;

    [Header("Camera Positions")]

    [SerializeField] GameObject levelSelectCamera;
    [SerializeField] float cameraStart;
    [SerializeField] float cameraEnd;



    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();

        RootScript.MusicManager.PlayMusicTrack(menuMusic, 0.5f, 0.5f);
    }


    /// <summary>
    /// Load into the selected level.
    /// </summary>
    public void LoadLevel()
    {
        RootScript.UIElements.PlayButton(targetLevel);
        
    }

    public void ReturnToMainMenu()
    {
        RootScript.UIElements.ReturnToMenuButton("Menu2");
        

    }

    /// <summary>
    /// Set the active state of the main menu
    /// </summary>
    /// <param name="active">Whether or not the menu is active</param>
    public void SetMainMenu(bool active)
    {
        mainMenu.SetActive(active);
        if (active) ChangeActiveButton(mainMenuFirstButton);
    }

    /// <summary>
    /// Set the active state of the controls menu
    /// </summary>
    /// <param name="active">Whether or not the menu is active</param>
    public void SetControls(bool active)
    {
        controlsMenu.SetActive(active);
        if (active) ChangeActiveButton(controlsMenuFirstButton);
    }
    public void SetSettings(bool active){
        settingsMenu.SetActive(active);
        if(active){
            ChangeActiveButton(settingsSelectMenuFirstButton);
        }
    }

    /// <summary>
    /// Set the active state of the credits menu
    /// </summary>
    /// <param name="active">Whether or not the menu is active</param>
    public void SetCredits(bool active)
    {
        creditsMenu.SetActive(active);
        if (active) ChangeActiveButton(creditsMenuFirstButton);
    }

    /// <summary>
    /// Set the active state of the level select menu
    /// </summary>
    /// <param name="active">Whether or not the menu is active</param>
    public void SetLevelSelect(bool active)
    {
        levelSelectMenu.SetActive(active);
        if (active)
        {
            LeanTween.moveY(levelSelectCamera, cameraEnd, 0.8f).setEase(LeanTweenType.easeOutCubic);
            ChangeActiveButton(levelSelectMenuFirstButton);
        } else
        {
            LeanTween.moveY(levelSelectCamera, cameraStart, 0.8f).setEase(LeanTweenType.easeOutCubic);
        }
    }



    /// <summary>
    /// change the active button for the UI 
    /// </summary>
    /// <param name="newActiveButton">Object to make active</param>
    public void ChangeActiveButton(GameObject newActiveButton)
    {
        eventSystem.SetSelectedGameObject(null);

        //add the back button as the currently selected game object
        eventSystem.SetSelectedGameObject(newActiveButton);
        
    }

    /// <summary>
    /// Sets the target level to go to.
    /// </summary>
    /// <param name="target">Target level string.</param>
    public void SetTargetLevel(string target)
    {
        targetLevel = target;
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
