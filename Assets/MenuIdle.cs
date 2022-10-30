using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the arc movement of each leg on the main menu.
/// </summary>
public class MenuIdle : MonoBehaviour
{
    /// <summary>
    /// Used to offset the arc motion of the leg by the provided degree amount.
    /// </summary>
    [SerializeField] float offset;

    /// <summary>
    /// Determines how far the leg swings in each direction. 
    /// </summary>
    [SerializeField] float angle;

    /// <summary>
    /// Used to control how fast the leg moves.
    /// </summary>
    [SerializeField] float speed;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Uses a Sin calculation to create a new rotate value in a set curve, which is multiplied by the given angle.
        float rotate = Mathf.Sin(Time.time*speed) * angle;

        // Only changes the X rotation of the leg, creating the swinging arc motion.
        transform.localRotation = Quaternion.AngleAxis(rotate + offset, Vector3.right);
    }
}
