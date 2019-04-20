using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlSpeedScaler: MonoBehaviour
{
    [Tooltip("Steps available to scale")]
    [SerializeField]
    public TimeControlSpeedScalarEntry[] Steps;

    [Tooltip("Current position within steps")]
    [SerializeField]
    public int Position;

    [Tooltip("Default position within steps")]
    [SerializeField]
    public int DefaultPosition;

    [Tooltip("Scale up button (ie 0 > 1)")]
    [SerializeField]
    public InputButton ButtonScaleUp;

    [Tooltip("Scale down button (ie 1 > 0)")]
    [SerializeField]
    public InputButton ButtonScaleDown;

    // Start is called before the first frame update
    void Start()
    {
        Position = DefaultPosition;
        //apply default scaling options on start
        OnPositionChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp(ButtonScaleUp.KeyName))
        {
            if( (Position + 1 ) < Steps.Length )
            {
                Position++;
                OnPositionChange();
            }
            //else we reached the last position and can't move further
        }
        else if (Input.GetButtonUp(ButtonScaleDown.KeyName))
        {
            if ((Position - 1) >= 0)
            {
                Position--;
                OnPositionChange();
            }
            //else we reached the last position and can't move further
        }
        //else - no buttons were pressed
    }

    void OnPositionChange()
    {
        var step = Steps[Position];

        foreach( var group in step.Groups)
        {
            group.Affector.TimeScale = group.TimeScale;
        }
    }
}

[Serializable]
public struct TimeControlSpeedScalarEntry
{
    [Tooltip("Affectors that are modified by this step")]
    [SerializeField]
    public TimeControlSpeedScalarEntryAffectorGroup[] Groups;
}


[Serializable]
public struct TimeControlSpeedScalarEntryAffectorGroup
{
    [Tooltip("Time scale value that is applied to this affector")]
    [SerializeField]
    public float TimeScale;

    [Tooltip("Affector that will have their time scale value changed")]
    [SerializeField]
    public TimeControlAffectionDefinition Affector;
}
