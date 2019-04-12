﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves object between two vector points
/// </summary>
public class MoveBetweenPoints : MonoBehaviour
{
    public enum MoveOption
    {
        PingPong,
        OneWayReturn
    }

    //Exposed to Editor
    [Tooltip("Movement option")]
    [SerializeField]
    private MoveOption Option = MoveOption.PingPong;

    //Exposed to Editor
    [Tooltip("Initial point")]
    [SerializeField]
    private Vector3 TargetPointDelta = Vector3.zero;

    [Tooltip("Movement speed (units/per second)")]
    [SerializeField]
    private float Speed = 0.0f;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale TimeScaleInstance;

    //Exposed to other classes

    //Internal

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    //length between points
    private float _length;

    void Start()
    {
        TimeScaleInstance = TimeControlController.Instance.CreateTimeScaleInstance(this);

        SetStartEnd(this.gameObject.transform.position, this.gameObject.transform.position + TargetPointDelta);

        // Calculate the journey length.
        _length = Vector3.Distance(_startPosition, _targetPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Speed != 0)
        {
            TimeScaleInstance.Update();
            float distCovered = TimeScaleInstance.CurrentTime * Speed;

            float fracJourney = (distCovered / _length);

            transform.position = Vector3.Lerp(_startPosition, _targetPosition, fracJourney);

            if( fracJourney >= 1.0f )
            {

                switch (Option)
                {
                    case MoveOption.OneWayReturn:
                        TimeScaleInstance.ResetCurrentTime();
                        break;

                    case MoveOption.PingPong:
                        SetStartEnd(_targetPosition, _startPosition);
                        break;

                    default:
                        throw new System.Exception($"Unknown type {Option}");
                }
            }
        }
    }


    public void SetStartEnd(Vector3 start, Vector3 end)
    {
        TimeScaleInstance.ResetCurrentTime();

        _startPosition = start;
        _targetPosition = end;
    }
}
