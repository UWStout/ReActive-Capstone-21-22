using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnterTrigger;
    [SerializeField] private GameObject gameUIReticle;

    public void OnEnterStart()
    {
        Debug.Log("OnEnterStart");
        OnEnterTrigger.Invoke();
    }

    public void ChangeLevel(string newLevel)
    {
        Debug.Log("Change Level");
        if (RootScript.GlobalQueue.QueueEmpty())
        {
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.LoadAllOfScene(newLevel, true);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 1f, LeanTweenType.easeOutSine);
            RootScript.GlobalQueue.AddCoroutine(RootScript.UIElements.ShowAllUIOverTimeCoroutine(5f, 0.5f), "Show All UI", false);
        }
        RootScript.TheGameManager.HealthAction(GameManager.HealthValue.reset,3);
        RootScript.UIElements.HealthUpdate();
        
    }
    
    public void ChangeSceneWithoutUI(string newLevel)
    {
        if (RootScript.GlobalQueue.QueueEmpty())
        {
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.LoadAllOfScene(newLevel, true);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 1f, LeanTweenType.easeOutSine);
        }
        
    }

    public void EscapeIntro()
    {
        
        Debug.Log("Going From Tutorial to Main Level");

        // Dependencies
        SoundSystem.SoundType levelEscapeSound = RootScript.SoundManager.FindSoundTypeByString("Escape");

        // Stuff
        if (RootScript.GlobalQueue.QueueEmpty())
        {
            RootScript.UIElements.HideAllUI(0.5f);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.AddCoroutine(RootScript.SoundManager.PlaySoundCoroutine(levelEscapeSound), "Play Escape Sound", false);
            RootScript.GlobalQueue.AddCoroutine(RootScript.UIElements.DisplayTransitionText("Bit-Bot sets out to find the batteries to restart the factory..."), "Show Transition Text", true);
            RootScript.GlobalQueue.WaitForTime(1f);
            RootScript.GlobalQueue.LoadAllOfScene("FlyingToLevel", true);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 0.5f, LeanTweenType.easeOutSine);
            RootScript.GlobalQueue.LoadAllOfScene("BuildScene", false);
            RootScript.GlobalQueue.WaitForTime(5f);
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.5f, LeanTweenType.easeInExpo);
            RootScript.GlobalQueue.AddCoroutine(RootScript.SceneLoader.FinishLoadingScene(true));
            RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, 1f, LeanTweenType.easeOutSine);
            RootScript.GlobalQueue.AddCoroutine(RootScript.UIElements.ShowAllUIOverTimeCoroutine(5f, 0.5f), "Show All UI", false);
        }
        
    }
}
