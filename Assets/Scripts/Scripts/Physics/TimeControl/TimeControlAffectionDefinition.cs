using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlAffectionDefinition : AffectionDefinitionBase
{
    [Tooltip("Scale of time for this specific affection")]
    [Range(-5f, 5f)]
    [SerializeField]
    float _timeScale = 1.0f;
    public float TimeScale
    {
        get { return _timeScale; }
        set
        {
            _timeScale = value;
            OnValidate();
        }
    }
}
