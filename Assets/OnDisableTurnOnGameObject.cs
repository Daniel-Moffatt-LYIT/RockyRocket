using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisableTurnOnGameObject : MonoBehaviour
{
    [SerializeField]
    GameObject turnThisOn;
    private void OnDisable()
    {
        turnThisOn.SetActive(true);
    }
}
