using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] heartImages;

    private void Start()
    {
        PlayerCollisions.healthBarHandler = this;
        setImages(PlayerCollisions.health);
    }
    public void setImages(int amountOfHearts)
    {
        for(int i = 0; i < heartImages.Length; i++)
        {
            if (i <= amountOfHearts - 1)
            {
                heartImages[i].SetActive(true);
            }
            else
            {
                heartImages[i].SetActive(false);
            }
        }
    }
}
