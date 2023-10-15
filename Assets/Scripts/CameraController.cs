using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5);
    private float shakeDuration = 0;
    private float shakeIntensity = 0;
    private bool gradualShake = false;

    private void Start()
    {
        transform.position = Player.position + offset;
        FacePlayer();
    }

    void LateUpdate()
    {
        HandleCameraRotation();

        // After rotating, adjust the position to maintain the offset
        if (Player)
        {
            transform.position = Player.position + offset;
        }
        
    }

    private void HandleCameraRotation()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateAroundPlayer(-45);  // Rotate counter-clockwise
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateAroundPlayer(45);  // Rotate clockwise
        }
    }

    private void RotateAroundPlayer(float angle)
    {
        transform.RotateAround(Player.position, Vector3.up, angle);

        // Update the offset after rotating
        offset = transform.position - Player.position;
    }

    // Makes sure the camera is always facing the player after adjusting its position
    private void FacePlayer()
    {
        transform.LookAt(Player);
    }

    public void changeOffset(Vector3 newOffset)
    {
        this.offset = newOffset;
        transform.position = Player.position + offset;
        FacePlayer();
    }

    private void cameraShake()
    {
        if (shakeDuration > 0)
        {
            transform.position += UnityEngine.Random.insideUnitSphere * shakeIntensity;
            shakeDuration -= Time.deltaTime;

            if (gradualShake == true)
            {
                shakeIntensity = Mathf.Max(0, shakeIntensity - shakeIntensity * Time.deltaTime * 3);
            }
        }
    }

    public void setCameraShake(float intensity, float duration, bool gradual)
    {
        if (shakeDuration < duration)
        {
            shakeDuration = duration;
        }
        shakeIntensity = intensity;
        gradualShake = gradual;
    }
}
