using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableCancelVelocity : MonoBehaviour
{
    private void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
