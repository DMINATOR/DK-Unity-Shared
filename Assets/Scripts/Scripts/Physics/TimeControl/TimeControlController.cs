using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Time control controller is used to control time of individual time control objects
/// </summary>
[Serializable]
public class TimeControlController : SingletonInstance<TimeControlController>
{
    [Header("Settings")]

    [ReadOnly]
    [Tooltip("Settings to specify number of time control elements per item")]
    public SettingsConstants.Name TIME_CONTROL_ELEMENTS_SIZE_NAME = SettingsConstants.Name.TIME_CONTROL_ELEMENTS_SIZE;

    [ReadOnly]
    [Tooltip("Settings to specify threshold when time control changes are recorded")]
    public SettingsConstants.Name TIME_CONTROL_CHANGE_DIFFERENCE = SettingsConstants.Name.TIME_CONTROL_CHANGE_DIFFERENCE;


    [Header("Loaded Settings")]

    [ReadOnly]
    [Tooltip("Global size of all elements per single Time Control Object")]
    public int ElementsSize;

    [ReadOnly]
    [Tooltip("Global size of all elements per single Time Control Object")]
    public float ThresholdChangeDifference;


    [Tooltip("Actual Time Scale value used by most objects")]
    [SerializeField]
    [ReadOnly]
    float _timeScale = 1.0f;
    public float TimeScale
    {
        get { return _timeScale; }
    }


    //Debug

    [Header("Debug")]

    [Tooltip("Shows control objects")]
    public bool DebugShowControlObjects;

    [Tooltip("Shows control objects full path")]
    public bool DebugShowControlObjectsFullPath;

    [Tooltip("For how long to show the game objects")]
    public float DebugShowControlObjectsTime = 10f;



    /// <summary>
    /// Current Time scale multipled by delta, always positive
    /// </summary>
    public float TimeScaleDelta
    {
        get
        {
            if( TimeScale < 0 )
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

    [Header("Time Scale control")]

    [Tooltip("Override for default Time scale values (if specified).")]
    public TimeControlAffectionDefinition Override;

    /// <summary>
    /// Current Time scale multipled by delta, can be negative
    /// </summary>
    public float TimeScaleDeltaRaw
    {
        get
        {
            return TimeScale * Time.deltaTime;
        }
    }


    [Tooltip("References to all time control object, for entire scene")]
    public HashSet<TimeControlReverse> TimeControlObjects = new HashSet<TimeControlReverse>();

    public override void OnCreated()
    {
        base.OnCreated();

        //Create initial objects
        ElementsSize = SettingsController.Instance.GetValue<int>(TIME_CONTROL_ELEMENTS_SIZE_NAME);
        ThresholdChangeDifference = SettingsController.Instance.GetValue<float>(TIME_CONTROL_CHANGE_DIFFERENCE);

        if (Override != null)
        {
            Override.AffectionChanged += OnTimeScaleChanged;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if ( Override != null )
        {
            Override.AffectionChanged -= OnTimeScaleChanged;
        }
    }

    public TimeControlTimeScale CreateTimeScaleInstance(MonoBehaviour obj)
    {
        var affectionReceiver = obj.GetComponent<TimeControlAffectionReceiver>();

        var timeScale = new TimeControlTimeScale(affectionReceiver);

        return timeScale;
    }

    public void Register(TimeControlReverse obj)
    {
        TimeControlObjects.Add(obj);
    }

    public void UnRegister(TimeControlReverse obj)
    {
        TimeControlObjects.Remove(obj);
    }

    public void OnTimeScaleChanged()
    {
        _timeScale = Override.TimeScale;
    }
}
