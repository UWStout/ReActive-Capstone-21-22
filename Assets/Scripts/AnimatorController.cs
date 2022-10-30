using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to  change animations 
/// </summary>
public class AnimatorController: MonoBehaviour

{
    /// <summary>
    /// ref for the animator 
    /// </summary>
    private Animator animator;
     /// <summary>
     /// the name of the current animation 
     /// </summary>
    private string currentAnimation;

   
  /// <summary>
  /// set for the animator 
  /// </summary>
  /// <param name="nAnimator"></param>
  public void SetAnimator(Animator nAnimator){
      if(nAnimator!=null){
    
      animator=nAnimator;
      }
  }
    /// <summary>
    /// change the current animation 
    /// </summary>
    /// <param name="animationName"></param>
   public void ChangeAnimation(string animationName){
       if(animationName==currentAnimation){
           return;
       }
       animator.Play(animationName);
      currentAnimation=animationName;

   }
}
