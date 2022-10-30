using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles character blinking on the main menu, without animations.
/// </summary>
public class Blinking : MonoBehaviour
{
    /// <summary>
    /// Counter variable used in combination with blinkTimer
    /// </summary>
    private float counter = 0f;

    /// <summary>
    /// The amount of time that needs to pass between each blink.
    /// </summary>
    private float blinkTimer = 2f;

    /// <summary>
    /// The amount of time needed for a full blink to be completed.
    /// </summary>
    [SerializeField] float leanTime;

    /// <summary>
    /// Scale of the eyes when they are closed during a blink
    /// </summary>
    private Vector3 blink = new Vector3(1,0.1f,1);

    /// <summary>
    /// Scale of the eyes when they are not blinking
    /// </summary>
    private Vector3 unblink = new Vector3(1, 1, 1);

    /// <summary>
    /// Used to determine wether the eyes are open or not.
    /// </summary>
    private bool openEyes = true;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Increase the counter if eyes don't need to blink yet.
        if (counter < blinkTimer)
        {
            counter++;
        }
        // If the eyes are open and ready to blink
        else if (counter >= blinkTimer && openEyes == true)
        {
            // Calls for the eyes to blink while the game continues to run
            StartCoroutine(BlinkEyes());

            // Prevents the else if condition from passing again.
            openEyes = false;

            // Reset Counter
            counter = 0;

            // Creates a new value to wait until another blink can happen.
            blinkTimer = Random.Range(120, 600);

            // Allows a new blink to occur
            openEyes = true;
        }
    }

    /// <summary>
    /// Handles the blink movement of the eyes via lineral interpolation.
    /// </summary>
    /// <returns> The amount of time the coroutine will pause to allow half a blink to occur </returns>
    IEnumerator BlinkEyes()
    {
        // Scales the eyes down to a closed position
        LeanTween.scale(gameObject, blink, leanTime);

        // Pause the coroutine untill the above scaling is complete
        yield return new WaitForSeconds(leanTime);

        // Reverts the scale of the eyes to their initial state.
        LeanTween.scale(gameObject, unblink, leanTime);
    }
}
