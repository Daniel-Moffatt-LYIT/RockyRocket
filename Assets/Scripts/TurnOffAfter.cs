using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAfter : MonoBehaviour
{
    [SerializeField]
    float lifeLength;

    private void OnEnable()
    {
        Invoke("TurnOff", lifeLength);
    }

    void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void SetLifeLength(float setTo)
    {
        lifeLength = setTo;
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
