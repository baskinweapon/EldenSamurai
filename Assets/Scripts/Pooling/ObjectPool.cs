using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public GameObject objectToPool;
    public int amountToPool;
    private List<GameObject> pooledObjects;
    
    void Start() {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++) {
            tmp = Instantiate(objectToPool);
            tmp.transform.parent = transform;
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject() {
        for(int i = 0; i < amountToPool; i++) {
            if(!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }
        var tmp = Instantiate(objectToPool);
        tmp.transform.parent = transform;
        tmp.SetActive(false);
        pooledObjects.Add(tmp);
        return tmp;
    }
}
