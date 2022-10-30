using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

///Character Movement Script.
public class CharacterMove : MonoBehaviour
{
	[SerializeField] public CharacterMoveStats Stats;

	/// <summary>
	/// The amount of time the character has spent in the air.
	/// </summary>
	[SerializeField] private float airtime;

	/// <summary>
	/// Whether or not the player is able to move.
	/// </summary>
	[SerializeField] private bool canPlayerMove = true;

	/// <summary>
	/// The GameObject that holds the player's model.
	/// </summary>
	[SerializeField] private GameObject playerModel;

	[SerializeField] private VisualEffect dashEffect;
	[SerializeField] private VisualEffect doubleJumpEffect;

	[SerializeField] private CharacterGroundedTrigger groundedTrigger;

	[SerializeField] private MusicTrack thisSceneMusicTrack;

	[Header ("Idle Values")]

	
	[SerializeField] private float idleTimeShowMenu;
	[SerializeField] private float idleTimeExtendedAnimation;
	private float currentIdleTime;
	private float currentGravity;


	private float dashStateCurrent;
	private float dashStateGravityCurrent;
	private int currentJumps;
	private float moveForward;
	private float moveStrafe;
	private float rotX;
	private float rotY;
	private float horizontalSensitivity = 3F;
	private float verticalSensitivity = 1F;
	private float speedCapMultiplier = 5F;
	private Vector3 respawnPosition;
	private Quaternion respawnRotation;
	private bool isJumping;
	private bool canDashAir;
	private float timer = 4f;
	//animations
	private PlayerAnimatorController playerAnimatorController;
	private Rigidbody rb;

	private bool isGroundedImmediate;
	private bool isGroundedBuffer;
	private float isGroundedBufferTime;

	
	[SerializeField] private SoundEffectType jumpSound;
	[SerializeField] private SoundEffectType jumpDoubleSound;
	[SerializeField] private SoundEffectType landSound;
	[SerializeField] private SoundEffectType dashSound;
	[SerializeField] private SoundEffectType deathSound;
	

	public GameObject grapplingHookObject;

/// <summary>
/// is the player idle
/// </summary>
/// <returns>if player is idle or not</returns>
	public bool PlayerIdle(){
		return currentIdleTime>0;
	}
	public bool ShouldPlayerIdleAnimation(){
		return currentIdleTime>10F;
	}
	void Start ()
	{
		playerAnimatorController=GetComponent<PlayerAnimatorController>();
		rb = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		currentJumps = Stats.maxJumps;

		respawnPosition = transform.position;
		respawnRotation = transform.rotation;

		ResetGravity();

		
		groundedTrigger.ResetFoundObjects();

		RootScript.CharMove = this;

		RootScript.UIElements.ReticleActive();

		if (thisSceneMusicTrack != null)
		{
			RootScript.MusicManager.PlayMusicTrack(thisSceneMusicTrack, 0.5f, 0.5f);
		}
		
	}


	void Update ()
    {
        // Make sure that the number of jumps is proper.
        HandleMultiJump();

        // Check for jump input and jump viability
        if (RootScript.Input.Jump.WasPressed && (Time.timeScale == 1.0f)) PlayerJump();

		// Handle Dashing
        dashStateCurrent -= Time.deltaTime;
        dashStateGravityCurrent -= Time.deltaTime;
        if (RootScript.Input.Dash.WasPressed) PlayerDash();

        //If the hud button was pressed, activate the canvas
        if (RootScript.Input.Hud.WasPressed)
        {
			StartCoroutine(RootScript.UIElements.ShowAllUIOverTimeCoroutine(3f, 0.5f));
        }

        if (RootScript.Input.Pause.WasPressed)
        {
            SwitchPauseState();
            RootScript.UIElements.gameObject.SetActive(true);
            RootScript.UIElements.pauseUpdate();
        }

		if (RootScript.Input.PanicKill.WasPressed)
		{
			RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideInFromUp, 0.3f, LeanTweenType.easeOutSine);
			RootScript.GlobalQueue.RespawnPlayer();
			RootScript.GlobalQueue.QueueTransition(UIControl.TransitionEffect.SlideOutToDown, 0.3f, LeanTweenType.easeInSine);
		}
    }

	void FixedUpdate()
	{
		UpdateGroundedState(Time.fixedDeltaTime);

		// Get if game's camera is in active mode.
		if (RootScript.PlayerCamera.GetCameraActive())
		{
			// Gravity
			if (!GetPlayerGroundedImmediate() && !IsDashingGravity())
			{
				rb.AddForce(0, currentGravity * Time.fixedDeltaTime, 0, ForceMode.Impulse);
			}
			
			// Player Movement
			if (canPlayerMove)
            {
                Vector3 movement = GetNewMovementVector();

                //Handle character rotation
                if (movement.magnitude > 0.1f) RotatePlayerModelToMovementDirection();

                rb.AddForce(movement, ForceMode.Impulse);

                // Cap Velocity
                Vector3 moveVelocity = CapMovementVelocity();
                float verticalSpeed = rb.velocity.y;

                // If the player is in the air, apply drag
                moveVelocity = HandleDragOnIdle(moveVelocity, verticalSpeed);

                //Handle the Idle State
                HandlePlayerIdle();
            }
        }
		else
		{
			rb.velocity = Vector3.zero;
		}
		
	}

	/// <summary>
	/// Handle player velocity drag when move is idle
	/// </summary>
	/// <param name="moveVelocity">Current movement velocity</param>
	/// <param name="verticalSpeed">Current vertical speed</param>
	/// <returns>Adjusted movement velocity</returns>
    private Vector3 HandleDragOnIdle(Vector3 moveVelocity, float verticalSpeed)
    {
        if (RootScript.Input.Move.Value.magnitude < 0.1f)
        {
            moveVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, Stats.lateralDrag * Time.fixedDeltaTime);

            if (moveVelocity.magnitude < 1f)
            {
                moveVelocity = Vector3.zero;
            }

            moveVelocity.y = verticalSpeed;

            rb.velocity = moveVelocity;
        }

        return moveVelocity;
    }


	/// <summary>
	/// Handle drag and showing UI when the player is not moving.
	/// </summary>
    private void HandlePlayerIdle()
    {
        if (rb.velocity.magnitude < 0.1f  && RootScript.GlobalQueue.QueueEmpty())
        {
            currentIdleTime += Time.fixedDeltaTime;

            if (currentIdleTime >= idleTimeShowMenu && currentIdleTime - Time.fixedDeltaTime < idleTimeShowMenu)
            {
                RootScript.UIElements.ShowAllUI(0.5f);
            }
        }
        else
        {
            if (currentIdleTime >= idleTimeShowMenu)
            {
                RootScript.UIElements.HideAllUI(0.25f);
            }
            currentIdleTime = 0f;
        }
    }

	/// <summary>
	/// Make sure the player's move speed isn't too fast
	/// </summary>
	/// <returns>Capped movement velocity</returns>
    private Vector3 CapMovementVelocity()
    {
        float vertSpeed = rb.velocity.y;
        Vector3 moveVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // If player velocity is greater than the cap, cap the speed.
        if (moveVelocity.magnitude > Stats.walkSpeedCap)
        {
            moveVelocity = moveVelocity.normalized * Mathf.Lerp(moveVelocity.magnitude, Stats.walkSpeedCap, Time.fixedDeltaTime * speedCapMultiplier);
            moveVelocity.y = vertSpeed;
            rb.velocity = moveVelocity;
        }

		return moveVelocity;
    }
	
	/// <summary>
	/// Rotate Player's model to the direction you're moving
	/// </summary>
    private void RotatePlayerModelToMovementDirection()
    {
        float angle = (float)((Mathf.Atan2(RootScript.Input.MoveHorizontal.Value, RootScript.Input.MoveVertical.Value) / Mathf.PI) * 180f);
        angle = Mathf.LerpAngle(playerModel.transform.localEulerAngles.y, angle, Time.fixedDeltaTime * 5f);
        playerModel.transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    /// <summary>
    /// Create a movement Vector3 with direction in mind.
    /// </summary>
    /// <returns>Movement Vector3 in world space</returns>
    private Vector3 GetNewMovementVector()
    {
        Vector3 movement = ((transform.forward * RootScript.Input.MoveVertical.Value) + (transform.right * RootScript.Input.MoveHorizontal.Value)) * Stats.walkSpeedAcceleration * Time.fixedDeltaTime;

        if (!GetPlayerGrounded()) movement = movement * Stats.airMoveMultiplier;
		
        return movement;
    }

    /// <summary>
    /// Call when you want the player to jump.
    /// </summary>
    private void PlayerJump()
    {
        if (currentJumps > 0 && RootScript.PlayerCamera.GetCameraActive())
        {
			//single jump
            playerAnimatorController.SetJumpTrigger();


            if (GetPlayerGrounded())
            {
                //rb.velocity = new Vector3(rb.velocity.x, 1, rb.velocity.z);
                rb.AddForce(0, Stats.jumpMultiplierGround, 0, ForceMode.Impulse);
                isJumping = true;
				isGroundedBufferTime = Stats.groundedBuffer;

				RootScript.SoundManager.PlaySound(jumpSound, -1f, transform);
            }
            else
            {
				//double jump
                doubleJumpEffect.SendEvent("OnJump");
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(0, Stats.jumpMultiplierAir, 0, ForceMode.Impulse);
                isJumping = true;

				RootScript.SoundManager.PlaySound(jumpDoubleSound, -1f, transform);

            }
            currentJumps--;

            UpdateGroundedState(0f);
        }
    }

	/// <summary>
	/// Handle multiple jumps and animations for it.
	/// </summary>
    private void HandleMultiJump()
    {
        if (!GetPlayerGrounded())
        {
            if (currentJumps == Stats.maxJumps && airtime > 0)
            {
                currentJumps = Stats.maxJumps - 1;
            }

            if (rb.velocity.y < 0)
            {
                isJumping = false;
                if (!IsDashing() && airtime > 0)
                {
                    playerAnimatorController.SetFallTrigger(true);
                }

            }
            airtime += Time.deltaTime;
            playerAnimatorController.SetLandedTrigger(false);
        }
        else
        {
            playerAnimatorController.SetFallTrigger(false);
            currentJumps = Stats.maxJumps;
        }
	}

	/// <summary>
	/// Make the player dash.
	/// </summary>
	private void PlayerDash()
	{
		if (!IsDashing() && RootScript.PlayerCamera.GetCameraActive() && (canDashAir || GetPlayerGrounded()))
		{
			Vector3 dashValue = ((transform.forward * RootScript.Input.MoveVertical.Value) + (transform.right * RootScript.Input.MoveHorizontal.Value));

			if (dashValue.magnitude < 0.1f)
			{
				dashValue = playerModel.transform.forward;
			}

			rb.AddForce(Stats.dashForce * dashValue, ForceMode.Impulse);
			rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			dashStateCurrent = Stats.dashStateDuration;
			dashStateGravityCurrent = Stats.dashGravityDuration;
			
			playerAnimatorController.SetDashTrigger();
			playerAnimatorController.SetFallTrigger(false);

			dashEffect.SendEvent("OnDash");

			RootScript.SoundManager.PlaySound(dashSound, -1f, transform);

			canDashAir = false;
		}
	}

	

	/// <summary>
	/// Check if the player is moving.
	/// </summary>
	/// <returns>Whether the player is moving or not.</returns>
	public bool IsStationary()
	{
		return RootScript.Input.Move.Value.magnitude < 0.1f && GetMoveSpeed() < 1f;
	}

	/// <summary>
	/// Get the player's lateral movement speed.
	/// </summary>
	/// <returns>Player's movement speed without vertical movement.</returns>
	public float GetMoveSpeed()
	{
		return new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
	}

	/// <summary>
	/// Get whether the player is currently in a dash state.
	/// </summary>
	/// <returns>Player is dashing.</returns>
	public bool IsDashing()
	{
		return dashStateCurrent > 0;
	}

	/// <summary>
	/// Get whether the player is currently in a dash state for gravity.
	/// </summary>
	/// <returns>Player is dashing.</returns>
	public bool IsDashingGravity()
	{
		return dashStateGravityCurrent > 0;
	}

	/// <summary>
	/// Get whether the player is jumping.
	/// </summary>
	/// <returns>Player is currently jumping.</returns>
	public bool IsJumping()
	{
		return isJumping;
	}
	
	/// <summary>
	/// Set the player's respawn variables
	/// </summary>
	/// <param name="position">Respawn Position</param>
	/// <param name="rotation">Respawn Rotation</param>
	public void SetRespawn(Vector3 position, Quaternion rotation)
	{
		SetRespawnPosition(position);
		SetRespawnRotation(rotation);

	}

	/// <summary>
	/// Set the player's respawn position
	/// </summary>
	/// <param name="position">Respawn Position</param>
	public void SetRespawnPosition(Vector3 position)
	{
		respawnPosition = position;
	}

	/// <summary>
	/// Set the player's respawn rotation
	/// </summary>
	/// <param name="rotation">Player Rotation</param>
	public void SetRespawnRotation(Quaternion rotation)
	{
		respawnRotation = rotation;
	}

	/// <summary>
	/// Respawn the player at the currently set respawn values.
	/// </summary>
	public void Respawn()
	{
		transform.position = respawnPosition;
		transform.rotation = respawnRotation;
		GetComponent<HealthManager>().ResetHealth();
		GetComponent<GrapplingHook>().UnHookFromRespawn();
		RootScript.SoundManager.PlaySound(deathSound, -1f, transform);
	}

	/// <summary>
	/// Rotate player model to look correctly.
	/// </summary>
	/// <param name="rot">Rotational Angle</param>
	public void RotateModel(float rot)
	{
		playerModel.transform.Rotate(playerModel.transform.up * rot);
	}

	public void SetPauseState(bool pause)
	{
		Time.timeScale = pause ? 0f : 1.0f;
	}

	public void SwitchPauseState()
	{
		Time.timeScale = (Time.timeScale == 1.0f) ? 0f : 1.0f;
		Cursor.lockState = (Time.timeScale == 1.0f) ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = (Time.timeScale != 1.0f);
	}

	/// <summary>
	/// Handle Grounded States
	/// </summary>
	public void UpdateGroundedState(float dt)
	{
		/*
		RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up, Stats.groundedRayDistance);

		bool foundCollider = false;

		// Raycast down to check if there is an object below.
		foreach (RaycastHit hit in hits)
		{
			if (!hit.collider.isTrigger)
			{
				foundCollider = true;
			}
		}
		*/

		bool foundCollider = groundedTrigger.GetGrounded();

		if (foundCollider && !isJumping)
		{
			isGroundedImmediate = true;
			isGroundedBuffer = true;
			if (airtime > 0)
			{
				//rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				playerAnimatorController.SetLandedTrigger(true);

				RootScript.SoundManager.PlaySound(landSound, -1f, transform);

			}

			isGroundedBufferTime = 0f;
			airtime = 0f;
			canDashAir = true;
		}
		else
		{
			isGroundedImmediate = false;
			isGroundedBufferTime += dt;
			if (isGroundedBufferTime > Stats.groundedBuffer)
			{
				isGroundedBuffer = false;
				airtime += dt;
			}
			else
			{
				isGroundedBuffer = true;
			}
		}
	}
	
	/// <summary>
	/// Get if the player is grounded - buffer version
	/// </summary>
	/// <returns>isGroundedBuffer</returns>
	public bool GetPlayerGrounded()
	{
		return isGroundedBuffer;
	}

	/// <summary>
	/// Get if the player is grounded - immediate version
	/// </summary>
	/// <returns>isGroundedImmediate</returns>
	public bool GetPlayerGroundedImmediate()
	{
		return isGroundedImmediate;
	}

	/// <summary>
	/// Set currentGravity to a defined value
	/// </summary>
	/// <param name="newGravity">New gravity value</param>
	public void SetGravity(float newGravity)
	{
		currentGravity = newGravity;
	}

	/// <summary>
	/// Reset the currentGravity value to the CharacterMoveStats value.
	/// </summary>
	public void ResetGravity()
	{
		currentGravity = Stats.gravity;
	}

	public GameObject GetPlayerModel()
	{
		return playerModel;
	}

	void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position + -transform.up * Stats.groundedRayDistance);
    }

	public void CauseJump()
    {
		PlayerJump();
    }
	public void CauseDash()
    {
		PlayerDash();
    }

}
