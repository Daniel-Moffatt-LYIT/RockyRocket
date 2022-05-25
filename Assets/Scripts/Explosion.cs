using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    Material mat;
    GameObject[] cubes;
    [SerializeField]
    int amountOfCubes;
    // Start is called before the first frame update
    void Start()
    {
        cubes = new GameObject[amountOfCubes];
        for (int i = 0; i < amountOfCubes; i++)
        {
            cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes[i].transform.localScale *= 0.001f;
            cubes[i].transform.position = this.transform.position;
            cubes[i].transform.parent = this.transform;
            cubes[i].AddComponent<Rigidbody>();
            cubes[i].AddComponent<TurnOffAfter>();
            cubes[i].GetComponent<MeshRenderer>().material = mat;
            TurnOffAfter tOffA = cubes[i].GetComponent<TurnOffAfter>();
            tOffA.SetLifeLength(1);
            cubes[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        foreach (GameObject obj in cubes)
        {
            obj.transform.position = this.transform.position;
            obj.SetActive(true);
        }
    }
}
