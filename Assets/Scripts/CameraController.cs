using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset = new Vector3(0, 5, -2);
    public Boolean test = true;
    private float shakeDuration = 0;
    private float shakeIntensity = 0;
    private bool gradualShake = false;

    private void Start()
    {
        transform.position = Player.position + offset;
    }

    void LateUpdate()
    {
        transform.position = Player.position + offset;
        if (test && Input.GetKey(KeyCode.Space))
        {
            setCameraShake(0.2f, 0.5f, true);
        }
        cameraShake();
    }

    public void changeOffset(Vector3 newOffset)
    {
        this.offset = newOffset;
    }

    private void cameraShake()
    {
        if (shakeDuration > 0)
        {
            // Transform camera within a shakeIntensity radius sphere around itself
            transform.position += UnityEngine.Random.insideUnitSphere * shakeIntensity;
            shakeDuration -= Time.deltaTime;

            if (gradualShake == true)
            {
                shakeIntensity -= shakeIntensity * Time.deltaTime * 3;
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