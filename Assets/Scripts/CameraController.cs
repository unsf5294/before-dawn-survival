using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5);
    [SerializeField] private Vector3 startOffset = new Vector3(0, 15, -15); // initial state
    [SerializeField] private float transitionDuration = 2.0f;
    [SerializeField] private float initialDelay = 2.0f;

    private float shakeDuration = 0;
    private float shakeIntensity = 0;
    private bool gradualShake = false;
    private bool transitionFinished = false; 

    private void Start()
    {
        if (Player)
        {
            transform.position = Player.position + startOffset; 
            StartCoroutine(TransitionCamera()); 
        }
        FacePlayer();
    }

    void LateUpdate()
    {
        HandleCameraRotation();

        cameraShake();

        if (transitionFinished && Player)
        {
            transform.position = Player.position + offset;
        }
    }

    IEnumerator TransitionCamera()
    {
        if (Player == null)
        {
            Debug.LogError("Player transform is not set.");
            yield break;
        }

        yield return new WaitForSeconds(initialDelay); // wait for initial 2s

        float t = 0.0f;
        Vector3 startingPos = transform.position; // Current position of the camera

        while (t < 1.0f)
        {
            t += Time.deltaTime / transitionDuration;

            Vector3 finalPos = Player.position + offset; 
            transform.position = Vector3.Lerp(startingPos, finalPos, t); 
            FacePlayer(); // Ensure the camera is facing the player

            yield return null; 
        }

        transitionFinished = true;
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
