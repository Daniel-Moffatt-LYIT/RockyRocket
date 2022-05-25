using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    float speed;
    void Update()
    {
        transform.position += transform.forward*Time.deltaTime*speed;    
    }
}
