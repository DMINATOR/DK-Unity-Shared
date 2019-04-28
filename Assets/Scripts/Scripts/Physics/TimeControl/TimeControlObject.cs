using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Single record - recorded by
/// </summary>
[System.Serializable]
public struct TimeControlElements
{
    public Vector3 Position;
    public Quaternion Rotation;
    public float DeltaTime;
}


public class TimeControlObject : MonoBehaviour, ITranslationWrapper
{
    [Tooltip("Current index within Elements")]
    [SerializeField]
    private int CurrentPosition;

    [Tooltip("Last actual position and rotation")]
    [SerializeField]
    TimeControlElements LastElementActual;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale _instance;

    [Tooltip("Time control records (deltas)")]
    [SerializeField]
    private TimeControlElements[] Elements;

    /// <summary>
    /// Previous position for the last object
    /// </summary>
    public int PreviousPosition
    {
        get
        {
            if (CurrentPosition <= 0)
            {
                return Elements.Length - 1;
            }
            else
            {
                return (CurrentPosition - 1) % Elements.Length;
            }
        }
    }

    private void Awake()
    {
        Elements = new TimeControlElements[TimeControlController.Instance.ElementsSize];
    }

    private void Start()
    {
        _instance = TimeControlController.Instance.CreateTimeScaleInstance(this);
        //TimeControlController.Instance.Register(this);

        RememberPosition(0);
    }

    private void OnDestroy()
    {
        //if( !TimeControlController.Destroyed )
        //{
        //    TimeControlController.Instance.UnRegister(this);
        //}
    }

    private void Update()
    {
        _instance.Update();
    }

    private void DebugDrawPositions()
    {
        if (TimeControlController.Instance.DebugShowControlObjects)
        {
            var divider = 1.0f / Elements.Length;

            if(TimeControlController.Instance.DebugShowControlObjectsFullPath )
            {
                var v1 = LastElementActual.Position;
                var v2 = LastElementActual.Position;

                var cntr = 0;
                for (var i = CurrentPosition; cntr < Elements.Length; cntr++, i--)
                {
                    var correction = i;

                    if (correction < 0)
                    {
                        correction = correction + Elements.Length;
                    }

                    var currentElement = Elements[correction % Elements.Length];

                    v2 = v1 - currentElement.Position;

                    Debug.DrawLine(
                        v1,
                        v2,
                        new Color(0, divider * cntr, 0),
                        TimeControlController.Instance.DebugShowControlObjectsTime);

                    v1 = v2;
                }
            }
            else
            {
                var value = divider * CurrentPosition;

                var v1 = LastElementActual.Position - Elements[CurrentPosition].Position;
                var v2 = LastElementActual.Position;
                //Log.Instance.Info("TimeControl", $"{PreviousPosition} ({v1.x},{v1.y},{v1.z}) => {CurrentPosition} ({v2.x},{v2.y},{v2.z})");

                Debug.DrawLine(
                    v1,
                    v2,
                    new Color(0, value, 0),
                    TimeControlController.Instance.DebugShowControlObjectsTime);
            }
        }
    }

    private void RememberPosition(int index)
    {
        //Record changes (deltas against current values)
        Elements[index].Position = transform.position - LastElementActual.Position;
        Elements[index].Rotation = LastElementActual.Rotation * Quaternion.Inverse(transform.rotation);
        Elements[index].DeltaTime = _instance.CurrentTime;

        //remember new position and rotation
        LastElementActual.Position = transform.position;
        LastElementActual.Rotation = transform.rotation;

        DebugDrawPositions();
    }

    private bool AreDifferent(float a, float b)
    {
        if (a >= b - TimeControlController.Instance.ThresholdChangeDifference && a <= b + TimeControlController.Instance.ThresholdChangeDifference)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RotateAndPositionInstant(Vector3 position, Quaternion rotation)
    {
        //Apply transform
        transform.position = position;
        transform.rotation = rotation;

        CurrentPosition = (CurrentPosition + 1) % Elements.Length;

        //remember position and rotation so that Delta won't be calculated
        LastElementActual.Position = transform.position;
        LastElementActual.Rotation = transform.rotation;

        RememberPosition(CurrentPosition);
    }

    public void RotateAndPosition(Vector3 position, Quaternion rotation)
    {
        //Apply transform
        transform.position = position;
        transform.rotation = rotation;

        if (
            //Position
            AreDifferent(LastElementActual.Position.x, position.x) ||
            AreDifferent(LastElementActual.Position.y, position.y) ||
            AreDifferent(LastElementActual.Position.z, position.z) ||

            //Rotation
            AreDifferent(LastElementActual.Rotation.x, rotation.x) ||
            AreDifferent(LastElementActual.Rotation.y, rotation.y) ||
            AreDifferent(LastElementActual.Rotation.z, rotation.z)
            )
        {
            CurrentPosition = (CurrentPosition + 1) % Elements.Length;

            RememberPosition(CurrentPosition);
        }
    }
}
