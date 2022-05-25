using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBullets : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerBullet")
        {
            other.gameObject.SetActive(false);
        }
    }
}
