using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGun : MonoBehaviour {

	private Quaternion originalRotation;
	[SerializeField] private GrapplingHook grapple;

	void Start ()
	{
		originalRotation = transform.localRotation;
	}
}
