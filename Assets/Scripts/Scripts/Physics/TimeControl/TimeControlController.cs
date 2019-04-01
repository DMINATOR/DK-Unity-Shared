using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Time control controller is used to control time of individual time control objects
/// </summary>
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

    [Tooltip("References to all time control object, for entire scene")]
    public HashSet<TimeControlObject> TimeControlObjects = new HashSet<TimeControlObject>();


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
}
