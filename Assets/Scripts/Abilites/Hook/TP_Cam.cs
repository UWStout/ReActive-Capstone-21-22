using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TP_Cam : MonoBehaviour {
public Vector3 CameraStandard;
	public Transform CameraExpected;
	public Transform CameraAround;
	public float CameraSmoother = 2;
	GrapplingHook Grapple;
	public GameObject cursor;
	private float RotX;
	private float RotY;
	private float HorizontalSensitivity = 3F;
	private float VerticalSensitivity = 2F;
	float SmoothingFactor;

	private bool cameraActive = true;

	GameObject Player;
	[SerializeField] private Camera thisCamera;

	void Start()
	{
		RootScript.PlayerCamera = this;
		CameraStandard = Camera.main.transform.localPosition;
		Grapple = GameObject.FindGameObjectWithTag("Player").GetComponent<GrapplingHook>();
		Player = GameObject.FindGameObjectWithTag("Player");
	}


	void Rotate()
	{
		float multiplier = RootScript.Input.LookZoom.IsPressed ? 0.25f : 1f;
		RotX = RootScript.Input.LookHorizontal.Value * HorizontalSensitivity * multiplier;
		RotY = RootScript.Input.LookVertical.Value * VerticalSensitivity * multiplier;

		Player.transform.Rotate(0, RotX, 0);
		if (RotX != 0)
		{
			Player.GetComponent<CharacterMove>().RotateModel(-RotX);
		}
		
		transform.RotateAround(CameraAround.position, transform.right, -RotY );

		float signedAngle = transform.eulerAngles.x;
		if (Vector3.Dot(transform.up, Vector3.up) >= 0f)
		{
		if (transform.eulerAngles.x >= 0f && transform.eulerAngles.x <= 90f)
		{
		signedAngle = transform.eulerAngles.x;
		}
		if (transform.eulerAngles.x >= 270f && transform.eulerAngles.x <= 360f)
		{
		signedAngle = transform.eulerAngles.x - 360f;
		}
		}
		if (Vector3.Dot(transform.up, Vector3.up) < 0f)
		{
		if (transform.eulerAngles.x >= 0f && transform.eulerAngles.x <= 90f)
		{
		signedAngle = 180 - transform.eulerAngles.x;
		}
		if (transform.eulerAngles.x >= 270f && transform.eulerAngles.x <= 360f)
		{
		signedAngle = 180 - transform.eulerAngles.x;
		}
		}

		float offAngle = signedAngle - Mathf.Clamp(signedAngle, -90f, 90f);
		//Debug.Log(offAngle);
		transform.RotateAround(CameraAround.position, transform.right, -offAngle * Mathf.Deg2Rad * 10f);
	}






	void Update()
	{
		if (cameraActive)
		{
			if (cursor != null)
			{
					Cursor. visible = false;
					Cursor.lockState= CursorLockMode.Confined;
				cursor.GetComponent<RectTransform>().position=Input.mousePosition;
			}
		
			if(!Grapple.GetIsGrappling())
			{
				Rotate();

			}
		}
		HandleFieldOfView();
	}

	float NormalizeAngle(float a)
	{
		return a - 180f * Mathf.Floor((a + 180f) / 180f);
	}

	void HandleFieldOfView()
	{
		if (RootScript.Input.LookZoom.IsPressed)
		{
			thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 30f, Time.deltaTime * 10f);
		}
		else
		{
			thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, 75f, Time.deltaTime * 10f);

		}
	}

	float ModularClamp(float val, float min, float max, float rangemin = -180f, float rangemax = 180f)
	{
		var modulus = Mathf.Abs(rangemax - rangemin);
		if((val %= modulus) < 0f) val += modulus;
		return Mathf.Clamp(val + Mathf.Min(rangemin, rangemax), min, max);
	}

	public void SetCameraActive(bool active)
	{
		cameraActive = active;
	}
	
	public bool GetCameraActive() { return cameraActive; }
}

