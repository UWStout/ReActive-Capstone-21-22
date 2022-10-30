using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationTriggerPlayer : MonoBehaviour
{
    [SerializeField] private CharacterMove characterMove;

     private Animator playerAnimator;

    private const string STRETCH="stretch";

    private const string LOOK_AROUND="look_around";


    


    private SoundSystem.SoundType stepSound;
    private SoundSystem.SoundType stepMetalSound;

    void Start()
    {
        stepSound = RootScript.SoundManager.FindSoundTypeByString("Step");
        stepMetalSound = RootScript.SoundManager.FindSoundTypeByString("MetalStep");
        playerAnimator=GetComponent<Animator>();
    }
    void Update(){
       
       if(characterMove.PlayerIdle() && characterMove.ShouldPlayerIdleAnimation() && 
        playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Blink"))
        {
           int ran=Random.Range(0,100);
           if(ran%2==0){
           playerAnimator.SetTrigger(STRETCH);
           }
           else{
            playerAnimator.SetTrigger(LOOK_AROUND);

           }
       }
    }

    public void TriggerSoundEffect()
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            RootScript.SoundManager.PlaySound(stepMetalSound, -1, transform);

        }
        else
        {
            RootScript.SoundManager.PlaySound(stepSound, -1, transform);

        }
        
    }

}
