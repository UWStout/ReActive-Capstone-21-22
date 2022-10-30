using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    private List<ResObject> resoultions = new List<ResObject>();
    private int currentRes = 0;
    [SerializeField] private TMPro.TMP_Text resText;

    [SerializeField] private TMPro.TMP_Text audioText;

    [SerializeField] private Slider volumeSlider;

    private bool isFullScreen = true;

    [SerializeField] private Toggle fullScreenToogle;

    [SerializeField] private GameObject[] pauseMenuUI;

    [SerializeField] private GameObject[] settingsUI;


    /// <summary>
    /// Add common resoultions at start
    /// </summary>
    private void AddResAtStart()
    {
        resoultions.Add(new ResObject(1280, 720));
        resoultions.Add(new ResObject(1920, 1080));
        resoultions.Add(new ResObject(2560, 1440));
        resoultions.Add(new ResObject(2560, 1440));



    }

    void Start()
    {
        //add common game resoultions
        AddResAtStart();
        /// <summary>
        /// check if player has a saved music and full screen value
        /// </summary>
        /// <value></value>
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);

        }


        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 1);

        }
        ///load saved values
        Load();
        bool hasFound = false;
        /// <summary>
        /// check if the list of game resoultions is the users screen
        /// </summary>
        /// <value></value>
        for (int i = 0; i < resoultions.Count; ++i)
        {
            if (Screen.width == resoultions[i].GetWidth() && Screen.height == resoultions[i].GetHeight())
            {
                /// <summary>
                /// update the index where the current res is 
                /// </summary>
                currentRes = i;
                hasFound = true;
                UpdateResText();
            }
        }
        /// <summary>
        /// if resoultion is not found add it to the list 
        /// </summary>
        /// <value></value>
        if (!hasFound)
        {
            //add the new resoultion to the list of resoultions and update it

            resoultions.Add(new ResObject(Screen.width, Screen.height));
            currentRes = resoultions.Count - 1;
            UpdateResText();
        }
        fullScreenToogle.isOn = isFullScreen;
    }
    /// <summary>
    /// called by user hitting left arrow in settings menu
    /// </summary>
    public void ChangeResLeft()
    {
        --currentRes;
        if (currentRes < 0)
        {
            currentRes = 0;
        }
        UpdateResText();

    }

    /// <summary>
    /// called when user hits right arrow in settings menu
    /// </summary>
    public void ChangeResRight()
    {

        ++currentRes;
        if (currentRes > resoultions.Count - 1)
        {
            currentRes = resoultions.Count - 1;
        }
        UpdateResText();
    }
    /// <summary>
    /// update the resoultion text 
    /// </summary>
    private void UpdateResText()
    {

        resText.text = "\n" + resoultions[currentRes].GetWidth() + "X" + resoultions[currentRes].GetHeight();
    }
    /// <summary>
    /// toogle settings UI
    /// </summary>
    /// <param name="active"></param>
    public void ToogleSettingsUI(bool active)
    {


        foreach (GameObject i in settingsUI)
        {
            i.SetActive(active);
        }
       


    }
    /// <summary>
    /// toogle pause UI
    /// </summary>
    /// <param name="active"></param>
    public void TooglePauseUI(bool active)
    {
        foreach (GameObject i in pauseMenuUI)
        {
            i.SetActive(active);
        }


    }
    /// <summary>
    /// applies the graphics settings when the user hits apply
    /// </summary>
    public void ApplyGraphics()
    {
        Screen.SetResolution(resoultions[currentRes].GetWidth(), resoultions[currentRes].GetHeight(), fullScreenToogle.isOn);

    }
    /// <summary>
    /// should the game be in full screen mode
    /// </summary>
    public void ChangeFullScreenToogle()
    {

        isFullScreen = !isFullScreen;
        Save();
    }
    /// <summary>
    /// change the volume of the game
    /// </summary>
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        ChangeAudioText();
        Save();

    }
    /// <summary>
    /// update the audio text
    /// </summary>
    private void ChangeAudioText()
    {
        audioText.text = "Volume:" + (int)((volumeSlider.value) * 100);

    }
    /// <summary>
    /// load values the user saved
    /// </summary>
    private void Load()
    {
        volumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume"));
        isFullScreen = PlayerPrefs.GetInt("fullscreen") == 1;
        ChangeAudioText();
    }
    /// <summary>
    /// saves values
    /// </summary>
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);

        PlayerPrefs.SetInt("fullscreen", fullScreenToogle.isOn ? 1 : 0);
    }

}
[System.Serializable]


/// <summary>
/// class to hold resoultions 
/// </summary>
public class ResObject
{
    public ResObject(int _width, int _height)
    {
        width = _width;
        height = _height;
    }
    private int width;
    private int height;

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
