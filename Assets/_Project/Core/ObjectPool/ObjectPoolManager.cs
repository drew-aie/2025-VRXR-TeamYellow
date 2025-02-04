using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> _objectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;
    private static GameObject _gameObjectsEmpty;


    public static PoolType _poolingType;

    public enum PoolType
    {
        GAMEOBJECT,
        NONE
    }

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _gameObjectsEmpty = new GameObject("GameObjects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.NONE)
    {
        PooledObjectInfo pool = _objectPools.Find(p => p._lookupString == objectToSpawn.name);

        //If the pool doesn't exist, create it
        if (pool == null)
        {
            pool = new PooledObjectInfo() { _lookupString = objectToSpawn.name };
            _objectPools.Add(pool);
        }

        //Check if there are any inactive objects in the pool
        GameObject spawnableObject = pool._inactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            //Find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);

            //If there are no inactive objects, create a new one
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            //If there is an inactive object, reactivate it
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool._inactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Subtracting 7 chars to remove (Clone) from the name of the object passed in

        PooledObjectInfo pool = _objectPools.Find(p => p._lookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool._inactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GAMEOBJECT:
                return _gameObjectsEmpty;
            case PoolType.NONE:
                return null;
            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string _lookupString;
    public List<GameObject> _inactiveObjects = new List<GameObject>();
}
