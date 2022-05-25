using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeTimer = 0.0f;
    private Vector3 startPos;
    public float shakeAmount;
    private void Awake()
    {
        startPos = transform.position;
    }
    private void Shake()
    {
        if (shakeTimer > 0.0f)
        {
            transform.position = transform.position + 
                new Vector3(Random.Range(-shakeAmount,shakeAmount), 
                Random.Range(-shakeAmount, shakeAmount), 
                Random.Range(-shakeAmount, shakeAmount));
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            if (transform.position != startPos)
            {
                transform.position = startPos;
            }
        }
    }
    private void Update()
    {
        Shake(); 
    }
}
