using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Time control controller is used to control time of individual time control objects
/// </summary>
[Serializable]
public class TimeControlController : MonoBehaviour
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
    public HashSet<TimeControlObject> TimeControlObjects = new HashSet<TimeControlObject>();

    //Public instance to time controller
    public static TimeControlController Instance = null;

    private void Awake()
    {
        if (Override != null)
        {
            Override.AffectionChanged += OnTimeScaleChanged;
        }

        //Create instance
        if (Instance == null)
        {
            ElementsSize = SettingsController.Instance.GetValue<int>(TIME_CONTROL_ELEMENTS_SIZE_NAME);
            ThresholdChangeDifference = SettingsController.Instance.GetValue<float>(TIME_CONTROL_CHANGE_DIFFERENCE);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if( Override != null )
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

    public void Register(TimeControlObject obj)
    {
        TimeControlObjects.Add(obj);
    }

    public void UnRegister(TimeControlObject obj)
    {
        TimeControlObjects.Remove(obj);
    }

    public void OnTimeScaleChanged()
    {
        _timeScale = Override.TimeScale;
    }
}
