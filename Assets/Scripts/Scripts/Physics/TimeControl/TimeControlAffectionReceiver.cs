using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlAffectionReceiver : AffectionReceiver
{
    [Tooltip("Current cached time scale value")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleCache;

    /// <summary>
    /// Current Time scale multipled by delta, always positive
    /// </summary>
    public float TimeScaleDelta
    {
        get
        {
            if (_timeScaleCache < 0)
            {
                //Anything that is slower than 0 is reversed and should be using Reverse logic, freeze actual logic for this object instead
                return 0;
            }
            else
            {
                //Only positive time scale returned
                return TimeScaleDeltaRaw;
            }
        }
    }

    public float TimeScaleDeltaRaw
    {
        get
        {
            return _timeScaleCache * Time.deltaTime;
        }
    }

    protected override void OnAffectionChanged()
    {
        base.OnAffectionChanged();

        _timeScaleCache = ((TimeControlAffectionDefinition)Affection).TimeScale;
    }
}
