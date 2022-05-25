using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelect : MonoBehaviour
{
    [SerializeField]
    public GameObject[] ships;

    public static int skinNumber = 0;
    private void Start()
    {
        SetShip();
    }
    public void SetShip()
    {
        foreach(GameObject ship in ships)
        {
            ship.SetActive(false);
        }
       
        ships[skinNumber].SetActive(true);
    }
    public void selectNextShip()
    {
        skinNumber++;
        if (skinNumber > ships.Length-1)
        {
            skinNumber = 0;
        }
        PlayerPrefs.SetInt("skinNumber", skinNumber);
        SetShip();
    }
    public void selectPreviousShip()
    {
        skinNumber--;
        if (skinNumber < 0)
        {
            skinNumber = ships.Length-1;
        }
        PlayerPrefs.SetInt("skinNumber", skinNumber);
        SetShip();
    }
}
