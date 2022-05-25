using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float screenWidth;
    [SerializeField]
    float screenHeight;
    [SerializeField]
    float bottomScreen;
    private Touch touch;
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 spawnPoint;
    Rigidbody rb;
    Vector3 targetPosition;
    private void OnEnable()
    {
        if(rb== null)
        {
            rb = GetComponent<Rigidbody>();
        }
        transform.position = spawnPoint;
    }
    void FixedUpdate()
    {
        rb.MovePosition(targetPosition);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            TouchComand();
            if (touch.phase == TouchPhase.Ended)
            {
                rb.velocity = Vector3.zero;
            }
        }
        KeepPlayerOnScreen();
    }

    private void TouchComand()
    {
        if (touch.phase == TouchPhase.Moved)
        {
            targetPosition = new Vector3(
                 transform.position.x + touch.deltaPosition.x * speed * Time.fixedDeltaTime,
                 transform.position.y,
                 transform.position.z + touch.deltaPosition.y * speed * Time.fixedDeltaTime);
        }
    }

    void KeepPlayerOnScreen()
    {
        if (transform.position.x > screenWidth)
        {
            transform.position = new Vector3(screenWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.z > screenHeight)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, screenHeight);
        }
        if (transform.position.x < -screenWidth)
        {
            transform.position = new Vector3(-screenWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.z <bottomScreen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, bottomScreen);
        }
    }
}
