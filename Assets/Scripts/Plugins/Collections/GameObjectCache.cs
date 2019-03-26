using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectCache
{
    public static string LOG_SOURCE = "GameObjectCache";

    [ReadOnly]
    [Tooltip("Currently cached objects")]
    public GameObject[] CachedObjects;

    [ReadOnly]
    [Tooltip("Container that will be used to create elements at")]
    public readonly GameObject SceneParentContainer;

    [ReadOnly]
    [Tooltip("Default size of the object cache")]
    public readonly int CacheSize;


    public GameObjectCache(GameObject container, int cacheSize)
    {
        SceneParentContainer = container;

        CacheSize = cacheSize;

        CachedObjects = new GameObject[cacheSize];
    }

    public void SetActive(bool active)
    {
        for (var i = 0; i < CachedObjects.Length; i++)
        {
            if (CachedObjects[i] != null)
            {
                CachedObjects[i].SetActive(active);
            }
        }
    }

    private int FindAvailableIndex()
    {
        for (var i = 0; i < CachedObjects.Length; i++)
        {
            if (CachedObjects[i] == null)
            {
                //empty - return right away
                return i;
            }
            else if (!CachedObjects[i].activeSelf)
            {
                //return only if it's not active anymore
                return i;
            }
            //else - active, skip these
        }

        //if we reached here, there were no available indexes. Return first one and write a warning
        Log.Instance.Warning(LOG_SOURCE, "Forced to re-use block at spot 0, there are no spots available!");
        return 0;
    }

    public void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var availableIndex = FindAvailableIndex();
        var gameObject = CachedObjects[availableIndex];

        if( gameObject == null )
        {
            //create new instance
            gameObject = GameObject.Instantiate(
                 prefab,
                 position,
                 rotation,
                 SceneParentContainer.transform);

            CachedObjects[availableIndex] = gameObject;
        }
        else
        {
            //update instance

            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;

            gameObject.SetActive(true); //activate it again
        }
    }
}
