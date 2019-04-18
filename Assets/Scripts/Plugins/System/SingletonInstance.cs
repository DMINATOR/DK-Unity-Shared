using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This was taken from http://www.unitygeek.com/unity_c_singleton/

public class SingletonInstance<T> : MonoBehaviour where T : Component
{
    public static bool Destroyed { get; set; }

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
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
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
        }
        else if (_instance != this)
        {
            //already created but no the current item
            Destroy(gameObject);
        }
        //else already created and the same item, nothing to do
    }
}

public class SingletonInstanceDontDestroy<T> : MonoBehaviour where T : Component
{
    public static bool Destroyed { get; set; }

    private static T _instance;
    public static T Instance
    {
        get
        {
            if( Destroyed )
            {
                throw new System.Exception("Should not call destroyed Singleton instance!");
            }
            else
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
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
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            //already created but no the current item
            Destroy(gameObject);
        }
        //else already created and the same item, nothing to do
    }
}