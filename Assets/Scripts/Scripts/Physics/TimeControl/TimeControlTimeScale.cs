using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlTimeScale
{
    /// <summary>
    /// Affection receiver if any
    /// </summary>
    TimeControlAffectionReceiver _receiver;

    [Tooltip("Current time, scaled by effective time scale at the moment")]
    [SerializeField]
    private float _currentTime;
    public float CurrentTime
    {
        get
        {
            return _currentTime;
        }
    }

    //Name of the affection that is currently applied
    public string AffectionName
    {
        get
        {
            if (_receiver != null)
            {
                return _receiver.Affection.Name;
            }
            else
            {
                return "Default"; //No affection, default is applied
            }
        }
    }


    //Current Time Scale value of this affection
    public float AffectionTimeScale
    {
        get
        {
            if (_receiver != null)
            {
                return ((TimeControlAffectionDefinition)_receiver.Affection).TimeScale;
            }
            else
            {
                return TimeControlController.Instance.TimeScale;
            }
        }
    }

    /// <summary>
    /// Retrieves current time scale delta value
    /// </summary>
    private float _timeScaleDelta;
    public float TimeScaleDelta
    {
        get
        {
            if ( _receiver != null )
            {
                //Receive time scale from the receiver
                _timeScaleDelta = _receiver.TimeScaleDelta;
            }
            else
            {
                //No custom receivers specified - revert to default
                _timeScaleDelta = TimeControlController.Instance.TimeScaleDelta;
            }
            return _timeScaleDelta;
        }
    }

    public TimeControlTimeScale(TimeControlAffectionReceiver receiver)
    {
        _receiver = receiver;
    }

    /// <summary>
    /// Call to update time information each frame
    /// </summary>
    public void Update()
    {
        _currentTime += TimeScaleDelta;
    }

    /// <summary>
    /// Resets time back to original value
    /// </summary>
    public void ResetCurrentTime()
    {
        _currentTime = 0.0f;
    }
}
