using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlAffectionReceiver : AffectionReceiver
{
    //Exposed to Editor

    //Exposed to other classes

    //Internal

    [Tooltip("Current cached time scale value")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleCache;

    [Tooltip("Cached time from affection")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleAffectionCache;

    [Tooltip("Cached time from affection from Applicator. Only if affected from applicator")]
    [SerializeField]
    [ReadOnly]
    private float? _timeScaleAffectionFromApplicatorCache;

    [Tooltip("Target time scale value")]
    [SerializeField]
    [ReadOnly]
    private float _timeScaleTarget;

    [Tooltip("Affection speed")]
    [SerializeField]
    [ReadOnly]
    private float _affectionSpeed;

    [Tooltip("Affection max scaled value")]
    [SerializeField]
    [ReadOnly]
    private float _affectionMax;

    [Tooltip("Current affection counter (in real time) seconds")]
    [SerializeField]
    [ReadOnly]
    private float _affectionCounter;

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
            if(_affectionCounter < 1.0f)
            {
                _affectionCounter += Time.deltaTime * _affectionSpeed;

                // update time scale value to match
                _timeScaleCache = Mathf.SmoothStep(_timeScaleCache, _timeScaleTarget, _affectionCounter );
            }
            //else we reached the target

            return _timeScaleCache * Time.deltaTime;
        }
    }

    public override void OnStart()
    {
        base.OnStart();

        //initial values should be set right away
        _timeScaleCache = _timeScaleTarget;
        _affectionCounter = _affectionSpeed;
    }

    public override void OnAffectionChanged()
    {
        base.OnAffectionChanged();

        _affectionCounter = 0.0f;
        _timeScaleAffectionCache = ((TimeControlAffectionDefinition)Affection).TimeScale;

        //if another affection is applied, multiply by
        if( AffectionFromApplicator != null )
        {
            _timeScaleAffectionFromApplicatorCache = ((TimeControlAffectionDefinition)AffectionFromApplicator).TimeScale;
            _timeScaleTarget = _timeScaleAffectionCache * _timeScaleAffectionFromApplicatorCache.Value;

            _affectionMax = ((TimeControlAffectionDefinition)AffectionFromApplicator).Speed;
        }
        else
        {
            _timeScaleAffectionFromApplicatorCache = null;

            _timeScaleTarget = _timeScaleAffectionCache;
        }

        //Apply instantly if it's configured
        if (_affectionMax == 0.0f)
        {
            //should be instant
            _timeScaleCache = _timeScaleTarget;
            _affectionCounter = _affectionSpeed;
        }
        else
        {
            _affectionSpeed = 1.0f / _affectionMax;
        }
    }
}
