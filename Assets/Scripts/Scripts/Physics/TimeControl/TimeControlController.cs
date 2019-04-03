using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Time control controller is used to control time of individual time control objects
/// </summary>
[Serializable]
public class TimeControlController
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


    [Tooltip("Hidden field from editor, but actually used by all game objects")]
    float _timeScale = 1.0f;
    public float TimeScale
    {
        get { return _timeScale; }
        set
        {
            _timeScale = value;
            OnTimeScaleChanged();
        }
    }


    /// <summary>
    /// Current Time scale multipled by delta
    /// </summary>
    public float TimeScaleDelta
    {
        get
        {
            return TimeScale * Time.deltaTime;
        }
    }


    [Tooltip("References to all time control object, for entire scene")]
    public HashSet<TimeControlObject> TimeControlObjects = new HashSet<TimeControlObject>();

    public delegate void TimeScaleChangedHandler();
    public event TimeScaleChangedHandler TimeScaleChanged;


    private static TimeControlController instance;
    public static TimeControlController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeControlController();
            }

            return instance;
        }
    }

    public TimeControlController()
    {
        ElementsSize = SettingsController.Instance.GetValue<int>(TIME_CONTROL_ELEMENTS_SIZE_NAME);
        ThresholdChangeDifference = SettingsController.Instance.GetValue<float>(TIME_CONTROL_CHANGE_DIFFERENCE);
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
        TimeScaleChanged?.Invoke();
    }
}
