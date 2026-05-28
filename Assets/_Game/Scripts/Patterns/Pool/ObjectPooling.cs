using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoSingleton<ObjectPooling>
{
    public ObjectPoolItems[] itemsToPool;
    private List<GameObject> pooledObjects;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItems item in itemsToPool)
        {
            for (int i = 0; i < item.poolAmount; i++)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    [System.Serializable]
    public class ObjectPoolItems
    {
        public string name;
        public int poolAmount;
        public GameObject poolObject;
        public bool shouldExpand;
    }

    public GameObject GetPooledObject(string name)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false && pooledObjects[i].name == name)
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItems item in itemsToPool)
        {
            if (item.poolObject.name == name)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }
    public GameObject GetPooledObject(string name, Transform Parent)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false && pooledObjects[i].name == name)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItems item in itemsToPool)
        {
            if (item.poolObject.name == name)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = Parent;
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }
    public GameObject GetPooledObject(string name, Transform _Parent, Vector3 _Position, Vector3 _Scale)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false && pooledObjects[i].name == name)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItems item in itemsToPool)
        {
            if (item.poolObject.name == name)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = _Parent;
                obj.transform.position = _Position;
                obj.transform.localScale = _Scale;
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }

    public GameObject InstantiateByName(string name)
    {
        foreach (ObjectPoolItems item in itemsToPool)
        {
            if (item.poolObject.name == name)
            {
                GameObject obj = Instantiate(item.poolObject);
                obj.name = item.name;
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                return obj;
            }
        }
        return null;
    }
}