using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the functionality of an item being picked up.
/// </summary>
public class ItemCollect : MonoBehaviour
{
    /// <summary>
    /// Reference to the Canvas.
    /// </summary>
    public GameObject canvas;

    /// <summary>
    /// Bool that checks if the item has been collected
    /// </summary>
    public bool itemCollected = false;

    /// <summary>
    /// After the player collides with the item, update the UI.
    /// </summary>
    /// <param name="other"> The object that the enters the item's hitbox.</param>
    private void OnTriggerEnter(Collider other)
    {
        //If the player hasn't collected this item yet.
        if (other.tag == "Player" && !itemCollected)
        {
            //Update the player's item count in the UI
            RootScript.UIElements.BatteryUpdate(1);

            //Update the player's current objective in the UI
            RootScript.UIElements.TaskUpdate("Reach the top of the mountain");

            //Play Battery Collect Sound Effect
            //RootScript.SoundManager.PlaySound(RootScript.SoundManager.FindSoundTypeByString("BatteryCollect"), -1f, transform);

            //Set the item to collected.
            itemCollected = true;
            
        }
    }
}
