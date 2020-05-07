using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{

    [Tooltip("Event is triggered when collider is triggered")]
    public UnityEvent ColliderTriggeredCallback;


    void OnTriggerEnter(Collider other)
    {
        ColliderTriggeredCallback.Invoke();
    }
}
