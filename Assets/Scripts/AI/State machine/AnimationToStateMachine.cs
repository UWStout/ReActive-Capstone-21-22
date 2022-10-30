using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    private AttackState attackState;

    public void setAttackState(AttackState state)
    {
        attackState = state;
    }
    /// <summary>
    /// called from animation event 
    /// </summary>
    public void TriggerAttack()
    {
        attackState.TriggerAttack();
        //Debug.Log("attack triggered");
    }
    public void FinishAttack()
    {
        attackState.FinishAttack();
        //Debug.Log("attack finished");
    }

}