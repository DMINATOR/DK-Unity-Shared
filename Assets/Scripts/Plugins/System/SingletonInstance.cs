using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This was taken from http://www.unitygeek.com/unity_c_singleton/

public interface ISingletonInstanceBase
{
    void OnCreated();
}

public class SingletonInstance<T> : MonoBehaviour, ISingletonInstanceBase where T : MonoBehaviour, ISingletonInstanceBase
{
    public static bool Destroyed { get; set; }

    public static bool Created { get; set; }

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (Destroyed)
            {
                throw new System.Exception("Should not call destroyed Singleton instance!");
            }
            else
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    //first run, create new game object
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            if (!Created)
            {
                ((ISingletonInstanceBase)_instance).OnCreated();
            }

            return _instance;
        }
    }

    public virtual void OnCreated()
    {
        Created = true;
    }

    public virtual void OnDestroy()
    {
        Destroyed = true;
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            //first run, assign as current instance
            _instance = this as T;

            if( !Created )
            {
                OnCreated();
            }
        }
        else if (_instance != this)
        {
            //already created but no the current item
            Destroy(gameObject);
        }
        //else already created and the same item, nothing to do
    }
}