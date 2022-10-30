using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Character Stats", menuName = "GameElements/Character Stats")]
public class CharacterMoveStats : ScriptableObject
{
    [Header ("Speed")]
    /// <summary>
	/// The speed at which the character speeds up.
	/// </summary>
	public float walkSpeedAcceleration;

	/// <summary>
	/// The fastest possible speed the character can move at horizontally.
	/// </summary>
	public float walkSpeedCap;

	/// <summary>
	/// Air move speed multiplier
	/// </summary>
	public float airMoveMultiplier = 0.25f;

    /// <summary>
	/// The drag laterally when you're on the ground.
	/// </summary>
	public float lateralDrag;

    [Header ("Jump")]

	/// <summary>
	/// The jump force the character recieves while jumping on the ground.
	/// </summary>
	public float jumpMultiplierGround;

	/// <summary>
	/// The jump force the character recieves while jumping in the air.
	/// </summary>
	public float jumpMultiplierAir;

    /// <summary>
	/// The amount of jumps the player has access to.
	/// </summary>
	public int maxJumps;

    /// <summary>
	/// The pull of gravity on the main character;
	/// </summary>
	public float gravity;

    [Header ("Dash")]

	/// <summary>
	/// The jump force the character recieves while jumping in the air.
	/// </summary>
	public float dashForce;

    /// <summary>
	/// The time after a dash the player is counted in the dash state.
	/// </summary>
	public float dashStateDuration = 1f;

	public float dashGravityDuration = 0.5f;

    [Header ("Grounded")]

	/// <summary>
	/// The distance the character checks to see if they are on the ground.
	/// </summary>
	public float groundedRayDistance = 1.1f;

	/// <summary>
	/// The amount of time the player will be counted as grounded after leaving the ground.
	/// </summary>
	public float groundedBuffer;

	

	

	

	
}
