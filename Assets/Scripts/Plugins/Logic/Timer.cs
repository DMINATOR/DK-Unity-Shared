using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Tooltip("Indicates if timer is currently active")]
    public bool Active;

    [Tooltip("If true, automatically activates on wake up.")]
    public bool ActivateOnEnabled;

    [Tooltip("Indicates that timer should re-activate after it has triggered")]
    public bool ReActivateAfterTrigger;

    [Tooltip("Interval between triggering, taken randomly between min (seconds)")]
    public float IntervalMin;

    [Tooltip("Interval between triggering, taken randomly between max (seconds)")]
    public float IntervalMax;

    [Tooltip("Event to call when triggered")]
    public UnityEvent EventTriggerCallback;

    private void OnEnable()
    {
        if( ActivateOnEnabled && !Active )
        {
            Activate();
        }
    }

    public void Activate()
    {
        if( !Active )
        {
            Active = true;

            InvokeNextTime();
        }
    }

    public void DeActivate()
    {
        Active = false;
    }

    private void InvokeNextTime()
    {
        if( IntervalMin == IntervalMax )
        {
            Invoke(nameof(OnTriggerEnter), IntervalMin);
        }
        else
        {
            Invoke(nameof(OnTriggerEnter), Random.Range(IntervalMin, IntervalMax));
        }
    }

    private void OnTriggerEnter()
    {
        if( Active )
        {
            EventTriggerCallback.Invoke();

            if (ReActivateAfterTrigger)
            {
                InvokeNextTime();
            }
            else
            {
                Active = false;
            }
        }
        else
        {
            //not active, don't execute
        }
    }
}
