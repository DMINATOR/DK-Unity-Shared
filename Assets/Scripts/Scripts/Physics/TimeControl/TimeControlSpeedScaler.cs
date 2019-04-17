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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
