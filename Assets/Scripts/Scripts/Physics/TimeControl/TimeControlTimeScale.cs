using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlTimeScale
{
    /// <summary>
    /// Affection receiver if any
    /// </summary>
    TimeControlAffectionReceiver _receiver;

    public float TimeScaleDelta
    {
        get
        {
            if( _receiver != null )
            {
                //Receive time scale from the receiver
                return _receiver.TimeScaleDelta;
            }
            else
            {
                //No custom receivers specified - revert to default
                return TimeControlController.Instance.TimeScaleDelta;
            }
        }
    }

    public TimeControlTimeScale(TimeControlAffectionReceiver receiver)
    {
        _receiver = receiver;
    }
}
