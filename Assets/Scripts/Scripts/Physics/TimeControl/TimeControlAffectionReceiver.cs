using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlAffectionReceiver : AffectionReceiver
{
    //Exposed to Editor

    //Exposed to other classes

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

    private float TimeScaleDeltaRaw
    {
        get
        {
            return _timeScaleCache * Time.deltaTime;
        }
    }

    //Internal

    [Tooltip("Current cached time scale value")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleCache;

    [Tooltip("Target time scale value")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleTarget;

    [Tooltip("Affection speed")]
    [SerializeField]
    [ReadOnly]
    private float _affectionDuration;

    [Tooltip("Current affection counter (in real time) seconds")]
    [SerializeField]
    [ReadOnly]
    private float _affectionCounter;


    public override void OnStart()
    {
        base.OnStart();

        //initial values should be set right away
        _timeScaleCache = _timeScaleTarget;
    }

    public override void OnAffectionChanged()
    {
        var affection = ((TimeControlAffectionDefinition)Affection);
        var affectionFromApplicator = ((TimeControlAffectionDefinition)AffectionFromApplicator);

        _affectionCounter = 0.0f;

        //recalculate new target value
        _timeScaleTarget = affection.TimeScale;
        _affectionDuration = affection.AffectionSpeed;

        if (AffectionFromApplicator != null)
        {
            _timeScaleTarget = (_timeScaleTarget + affectionFromApplicator.TimeScale) / 2.0f;
            _affectionDuration = (_affectionDuration + affectionFromApplicator.AffectionSpeed) / 2.0f;
        }

        if( _affectionDuration == 0.0f )
        {
            _timeScaleCache = _timeScaleTarget;
        }
    }

    private float CalculateCache()
    {
        if (_timeScaleCache != _timeScaleTarget)
        {
            _affectionCounter += Time.deltaTime;

            return Mathf.SmoothStep(_timeScaleCache, _timeScaleTarget, _affectionCounter / _affectionDuration);
        }
        else
            return _timeScaleCache;
    }

    private void Update()
    {
        if (_timeScaleCache != _timeScaleTarget)
        {
            _timeScaleCache = CalculateCache();
        }
    }
}
