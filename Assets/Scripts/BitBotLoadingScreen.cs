using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBotLoadingScreen : MonoBehaviour
{
    [SerializeField] private float verticalBobMagnitude = 3;
    [SerializeField] private float verticalBobTime = 3;

    [SerializeField] private float yawMagnitude = 1f;
    [SerializeField] private float yawTime = 0.5f;

    [SerializeField] private float pitchMagnitude = 1f;
    [SerializeField] private float pitchTime = 0.5f;

    [SerializeField] private float rollMagnitude = 1f;
    [SerializeField] private float rollTime = 0.5f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        transform.localPosition = startPosition + transform.up * Mathf.Sin(Time.time / verticalBobTime) * verticalBobMagnitude;
        transform.localRotation = startRotation * Quaternion.Euler(Mathf.Sin(Time.time / pitchTime) * pitchMagnitude, Mathf.Sin(Time.time / yawTime) * yawMagnitude, Mathf.Sin(Time.time / yawTime) * yawMagnitude);
    }
}
