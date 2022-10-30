using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move the text in the credit scene
/// </summary>
public class CreditScroll : MonoBehaviour
{
    /// <summary>
    /// Float that sets the scrolling speed of the text
    /// </summary>
    private float speed = 100;

    /// <summary>
    /// Y position where "Thanks for Playing" will be on the center of the screen
    /// </summary>
    [SerializeField] float endPos;

    /// <summary>
    /// Used to make the credits go faster when true
    /// </summary>
    private bool fastForward = false;

    void Start()
    {
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        // If the top of the text is below the set end position
        if (this.transform.position.y < endPos)
        {
            // If the space bar was pressed and the text isn't going faster
            if (RootScript.Input.Jump.WasPressed && !fastForward)
            {
                // Increase the scrolling speed and set fastForward to true
                fastForward = true;
                speed = 500;
            } // If the space bar was pressed and the text is going faster
            else if (RootScript.Input.Jump.WasPressed && fastForward)
            {
                // Decrease the scrolling speed and set fastForward to true
                fastForward = false;
                speed = 100;
            }

            // Moves the text up based on the current speed variable.
            this.transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
