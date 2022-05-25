using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [SerializeField]
    GameObject objectToPool;
    public GameObject[] pooledObjects;
    [SerializeField]
    int amountToPool;
    private void Start()
    {
        pooledObjects = new GameObject[amountToPool];
                            
        if (objectToPool != null)
        {
            for(int i = 0; i < amountToPool; i++)
            {
                pooledObjects[i] = SpawnObject();
                
            }
        }
    }
    public GameObject GetGameObject()
    {
        if(pooledObjects == null)
        {
            return null;
        }
        for(int i = 0; i < pooledObjects.Length; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        GrowArray();
        return pooledObjects[pooledObjects.Length - 1];

    }

    void GrowArray()
    {
        GameObject[] newArray = new GameObject[pooledObjects.Length + 10];
        for (int i = 0; i < newArray.Length; i++)
        {
            if (i < pooledObjects.Length)
            {
                newArray[i] = pooledObjects[i];
            }
            else
            {
                newArray[i] = SpawnObject();
            }
        }
        pooledObjects = newArray;
    }

    GameObject SpawnObject()
    {
        GameObject obj = (GameObject)Instantiate(objectToPool);
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.SetParent(this.transform);
        return obj;
    }
}
