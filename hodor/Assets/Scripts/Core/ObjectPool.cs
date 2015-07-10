using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : MonoBehaviour 
{
    public GameObject PoolObjectPrefab;
    public int Capacity;

    public List<GameObject> PoolObjects;
    private GameObject objectContainer;

    void Awake()
    {
        PoolObjects = new List<GameObject>();

        objectContainer = new GameObject("ObjectPool");
        objectContainer.transform.SetParent(this.gameObject.transform);

        for (int i = 0; i < Capacity; i++)
        {
            GameObject poolObject = GameObject.Instantiate(PoolObjectPrefab);
            poolObject.SetActive(false);
            poolObject.transform.SetParent(objectContainer.transform);

            PoolObjects.Add(poolObject);
        }
    }

    public GameObject Create()
    {
        foreach (GameObject poolObject in PoolObjects)
        {
            if (!poolObject.activeSelf) return poolObject;
        }

        return null;
    }
}
