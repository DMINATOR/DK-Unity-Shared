using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Invokes a method one time. 
/// 
/// This works with time control scaling as well
/// </summary>
public class TimeControlInvoker
{
    // Definition for delegate to invoke
    public delegate void InvokeMethodDelegate();

    // Current delegate provided that should be called
    private InvokeMethodDelegate _delegate;

    // Time when delegate should be triggered, this is affected by time scale instance
    private float _targetTime;

    // Time scale instance that provides time scaling for invoker
    private TimeControlTimeScale _instance;

    public TimeControlInvoker(MonoBehaviour behavior)
    {
        _instance = TimeControlController.Instance.CreateTimeScaleInstance(behavior);
    }

    public void Invoke(InvokeMethodDelegate method, float time)
    {
        _delegate = method;
        _targetTime = time;

        _instance.ResetCurrentTime();
    }

    public void Update()
    {
        _instance.Update();

        if( _instance.CurrentTime >= _targetTime)
        {
            //time to trigger
            _delegate.Invoke();
        }
    }
}
