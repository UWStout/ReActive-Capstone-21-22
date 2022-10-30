using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle any changes to the players current health.
/// </summary>
public class HealthManager : MonoBehaviour
{
    private int playerHealth = 3;

    /// <summary>
    /// The object in the canvas that displays the health bar images
    /// </summary>
    RawImage healthIcon;

    /// <summary>
    /// The image of the health bar when the player is at full health
    /// </summary>
    [SerializeField] Texture healthFull;

    /// <summary>
    /// The image of the health bar when the player is at half health
    /// </summary>
    [SerializeField] Texture healthHalf;

    /// <summary>
    /// The image of the health bar when the player is at quarter health
    /// </summary>
    [SerializeField] Texture healthQuarter;

    /// <summary>
    /// The image of the health bar when the player has no health
    /// </summary>
    [SerializeField] Texture healthEmpty;

    /// <summary>
    /// Used to check if the player can be hit.
    /// </summary>
    private bool invincible = false;
    /// <summary>
    /// player move script ref
    /// </summary>
    private CharacterMove playerMove;

    [SerializeField] private SoundEffectType damageSound;

     public static bool isDead=false;
     
    /// <summary>
    /// Amount of time that the player will be invincible.
    /// </summary>
    [SerializeField]private float timer = 1.5f;
    private void Start() {
        healthIcon = RootScript.UIElements.GetHealthRawImage();
        playerMove=GetComponent<CharacterMove>();
    }
    // Update is called once per frame
    void Update()
    {
     
        //If the player cannot take damage
        if (invincible)
        {
            //If the invincibility timer has run out
            if (timer <= 0)
            {
                //Make it so the player can be hit
                invincible = false;

                //Reset the invincibility timer
                timer = 3f;
            }
            //Reduce timer by time
            timer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Once the player is hit, reduce the player's health by the specified damage and make them invincible.
    /// </summary>
    /// <param name="damage"> The amount of health the player will lose.</param>
    public void takeDamage(int damage)
    {
        //If the player can be hit
        if (invincible == false)
        {
            //Reduce the player's health by the desired amount
           
           // playerHealth -= damage;

           RootScript.TheGameManager.HealthAction(GameManager.HealthValue.remove,damage);

           int currentHealth= RootScript.TheGameManager.GetHealth();

            //Update the health bar in the UI to reflect the new health value
            healthUpdate(currentHealth);

            //If the player's health is at, or below, the minimum amount.
            if(currentHealth <= 0)
            {
                invincible = true;
                //Set the player's health to zero
                float transitionLength = 0.5f;
                RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromLeft, transitionLength);
                RootScript.GlobalQueue.AddCoroutine(ResetAfterDeath(0));
                RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToUp, transitionLength);

                
                Debug.Log("Dead");
            }
            else
            {
                RootScript.SoundManager.PlaySound(damageSound, -1, transform);
            }

            //If the canvas is currently inactive, make it active.
            RootScript.UIElements.HealthUpdate();

            //Makes the player unable to take damage.
            invincible = true;
        }
//        Debug.Log(playerHealth);
    }
    /*
    IEnumerator Fade(){
        SetFade();
        //FIX THIS
        playerMove.enabled=false;
        isDead=true;
        yield return new WaitForSeconds(.5f);
        playerMove.enabled=true;
        isDead=false;

        SetFade();
    }
    */
    IEnumerator ResetAfterDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMove.Respawn();
        invincible = false;
        //playerHealth = 3;
        RootScript.TheGameManager.HealthAction(GameManager.HealthValue.reset);
        healthUpdate(3);
    }

        
    
    /// <summary>
    /// Increases the player's current health by a specified amount
    /// </summary>
    /// <param name="health"> The amount of health the player will gain.</param>
    public void gainHealth(int health)
    {
        //Increase the player's current health by the desired value.
        
        //playerHealth += health;
        
        RootScript.TheGameManager.HealthAction(GameManager.HealthValue.add,health);

        //Update the health bar in the UI to relfect the new health value
       
        //healthUpdate(playerHealth);

        healthUpdate(RootScript.TheGameManager.GetHealth());


        //If the player's current health is at, or above, the maximum value.
      
       /* if (playerHealth > 3)
        {
            //Sets the player's current health to 3.
            playerHealth = 3;
        }*/

        //If the canvas is currently inactive, make it active.
        if (!RootScript.UIElements.gameObject.activeSelf)
        {
            RootScript.UIElements.gameObject.SetActive(true);
        }
    }

    public void ResetHealth()
    {
        RootScript.TheGameManager.HealthAction(GameManager.HealthValue.reset,3);

        healthUpdate(RootScript.TheGameManager.GetHealth());
        
    }

    /// <summary>
    /// Updates the health bar in the UI to the current value of the player's health
    /// </summary>
    /// <param name="health"> The amount of health that should be seen in the health bar.</param>
    private void healthUpdate(int health)
    {
        //Based on the player's current health, change the image of health bar in the canvas.
        switch (health)
        {
            case 0:
                healthIcon.texture = healthEmpty;
                break;
            case 1:
                healthIcon.texture = healthQuarter;
                break;
            case 2:
                healthIcon.texture = healthHalf;
                break;
            case 3:
                healthIcon.texture = healthFull;
                break;
            default:
                break;
        }
    }
    public int getCurrentHealth()
    {
        return playerHealth;
    }
}
