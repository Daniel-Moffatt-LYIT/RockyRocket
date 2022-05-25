using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDirection;
    [SerializeField]
    float speed;
    
    void Update()
    {
        transform.Translate(moveDirection * speed*Time.deltaTime);
    }
    public void SetSpeed(float amount)
    {
        speed = amount;
    }
}
