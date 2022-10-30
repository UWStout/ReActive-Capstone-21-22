using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AnimatorController))]
[RequireComponent(typeof(CharacterMove))]


public class PlayerAnimatorController : MonoBehaviour
{

    private float timer;
    private GrapplingHook grapplingHook;
    private CharacterMove characterController;

    [SerializeField] Animator playerAnimator;
    private const string STRETCH="stretch";

    private const string LOOK_AROUND="look_around";

    private bool isGrappling=false;

    private const string GRAPPLE_PULL="isGrapplePull";

    private const string DOUBLE_JUMP="isDoubleJump";

    private bool isDoubleJump=false;


    private bool playStretch=true;
    // Start is called before the first frame update
    void Start()
    {
        grapplingHook=GetComponent<GrapplingHook>();
        characterController=GetComponent<CharacterMove>();
        
    }
    

    // Update is called once per frame
    void Update()
    { 
        if(!grapplingHook.GetIsGrappling()){
        bool isMoving= !characterController.IsStationary() && characterController.GetPlayerGrounded() && !characterController.IsDashingGravity();
       if(isMoving){
        playerAnimator.SetBool("isMoving",true);  
        playerAnimator.SetTrigger("move");
       }
       else{
        playerAnimator.SetBool("isMoving",false);  
        playerAnimator.ResetTrigger("move"); 
        playerAnimator.SetBool(GRAPPLE_PULL,false);
        isGrappling=false;
       }
       
    }
    else{
        //player is grappling
        //transtion to other grappling animation
        PlaySplitGrapple();
    }
   

    if(characterController.PlayerIdle()&& characterController.ShouldPlayerIdleAnimation()){
   

                if(playStretch){
                playerAnimator.ResetTrigger(LOOK_AROUND);

                playerAnimator.SetTrigger(STRETCH);
                playStretch=false;

                }
                else{
                playerAnimator.ResetTrigger(STRETCH);

                playerAnimator.SetTrigger(LOOK_AROUND);
                playStretch=true;
                }
            
    }
    else{
       //Debug.Log( "is player idle "+characterController.PlayerIdle() );
        //Debug.Log( "SHOULD PLAYER IDLE "+ characterController.ShouldPlayerIdleAnimation());
    }
        
    }
    public void SetGrappleTrigger(){
      
        playerAnimator.SetTrigger("grapple");

    }
    private void PlaySplitGrapple(){
        playerAnimator.ResetTrigger("grapple");
        playerAnimator.SetTrigger("splitGrapple");
    }
 
    public void SetDashTrigger(){
        if(!grapplingHook.GetIsGrappling()){
        playerAnimator.SetTrigger("dash");
        
        }
    }
    public void SetJumpTrigger(){

        playerAnimator.SetTrigger("jump");
        
    }
    public void SetFallTrigger(bool isFalling){
        playerAnimator.SetBool("isFalling",isFalling);
        
    }
    public void SetLandedTrigger(bool landed){
        playerAnimator.SetBool("hasFinished",landed);
    }

 

    
   
    }

