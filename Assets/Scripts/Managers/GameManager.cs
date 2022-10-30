using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    /// <summary>
    /// how many orbs the player has
    /// </summary>
    private int currentOrbs;
    /// <summary>
    /// how much hp the player has
    /// </summary>
    private int playerHp=3;

    /// <summary>
    /// the max amount of orbs in the scene
    /// </summary>
    private int maxOrbs;
    /// <summary>
    /// how many batterys the player has
    /// </summary>
    private int batterys;
   
   
    public enum HealthValue{
        add,remove,reset
    }
    /// <summary>
    /// adds hp to the player
    /// </summary>
    /// <param name="hp"></param>
    /// 
    /// 
    private void AddHealth(int hp){
        playerHp+=hp;
        if(playerHp>3){
            playerHp=3;
        }
       
      
    }
    /// <summary>
    /// removes hp from the player
    /// </summary>
    /// <param name="hp"></param>
    private void RemoveHP(int hp){
        playerHp-=hp;
        if(playerHp<0){
            playerHp=0;
        }
    }
    private void ResetHP(){
        playerHp=3;
    }
    public void HealthAction(HealthValue value, int amount=3){
        switch(value){
            case HealthValue.add:
            AddHealth(amount);
            break;
            case HealthValue.remove:
            RemoveHP(amount);
            break;
            case HealthValue.reset:
            ResetHP();
            break;
            default:
            Debug.LogError("Health Value not supported yet");
            break;
        }
    }

    public int GetHealth(){
        return playerHp;
    }



    /// <summary>
    /// init the game mananger script if not already in the scene
    /// </summary>
    private void Start()
    {
        if (RootScript.TheGameManager == null)
        {
            DontDestroyOnLoad(this);
            RootScript.TheGameManager = this;
            maxOrbs = FindObjectsOfType<Pickup>().Length;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// this function should only be called at the start of a level
    /// sets how many orbs are in the scene
    /// </summary>
    /// <param name="count"></param>
    public void SetMaxOrbs(int count){
        maxOrbs=count;
    }
    /// <summary>
    /// gets the max orbs from the level
    /// </summary>
    /// <returns>max orbs</returns>
    public int GetMaxOrbs(){
        return maxOrbs;
    }
    /// <summary>
    /// returns the current orbs in the scene
    /// </summary>
    /// <returns> the current orbs</returns>
    public int GetCurrentOrbs(){
        return currentOrbs;
    }
    /// <summary>
    /// function to collect an orb
    /// </summary>
    public void CollectOrb(){
        currentOrbs++;
        RootScript.UIElements.OrbUpdate(currentOrbs);
        
    }
    /// <summary>
    /// gets how many batterys the player has
    /// </summary>
    /// <returns>the batterys</returns>
    public int GetBatterys(){
        return batterys;
    }
    /// <summary>
    /// updates the amount of batterys the player has
    /// </summary>
    public void UpdateBattery(){
        batterys++;
        RootScript.UIElements.BatteryUpdate(batterys);
    }
    /// <summary>
    /// resets the orbs the player has
    /// </summary>
    public void ResetOrbs(){
        currentOrbs=0;
    }
    /// <summary>
    /// removes a certian amount of orbs from the player
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveOrbs(int amount){
        currentOrbs-=amount;
        if(currentOrbs<0){
            currentOrbs=0;
        }
    }
    /// <summary>
    /// resets the batterys the player has 
    /// </summary>
    public void ResetBatterys(){
        batterys=0;
    }
}


