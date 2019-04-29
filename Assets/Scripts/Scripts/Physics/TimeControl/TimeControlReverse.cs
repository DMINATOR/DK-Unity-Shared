using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Single record - recorded by
/// </summary>
[System.Serializable]
public struct TimeControlElement
{
    public Vector3 DeltaPosition;
    public Quaternion DeltaRotation;
    public float Time;
}

public struct TimeControlElementActual
{
    public TimeControlElement Element;

    public Vector3 Position;
    public Quaternion Rotation;
}

/// <summary>
/// Records object position and state, reverses them back if time scale changes to negative
/// </summary>
public class TimeControlReverse : MonoBehaviour, ITranslationWrapper
{
    [Tooltip("Current index within Elements")]
    [SerializeField]
    private int CurrentPosition;

    [Tooltip("Last actual position and rotation")]
    [SerializeField]
    TimeControlElementActual LastElementActual;

    [Tooltip("Last time when scaling was positive")]
    [SerializeField]
    private float LastPositiveTime;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale _instance;

    [Tooltip("Time control records (deltas)")]
    [SerializeField]
    private TimeControlElement[] Elements;

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
        Elements = new TimeControlElement[TimeControlController.Instance.ElementsSize];
    }

    private void Start()
    {
        _instance = TimeControlController.Instance.CreateTimeScaleInstance(this);

        RememberPosition(0);
    }

    private void Update()
    {
        _instance.Update();

        if( _instance.AffectionTimeScale < 0.0f )
        {
            ReverseToPreviousElementIfNeeded();

            if ( LastElementActual.Element.Time > 0.0f )
            {
                var mult = LastPositiveTime - LastElementActual.Element.Time;
                var journey = (_instance.CurrentTime - LastPositiveTime) / mult;

                //calculate and move position of current object
                transform.position = Vector3.Lerp(LastElementActual.Position, transform.position, journey);
                transform.rotation = Quaternion.Lerp(LastElementActual.Rotation, transform.rotation, journey);
            }
            //else - can't reverse back anymore as there is nowhere to go
        }
        else
        {
            //remember time when we had last positive value, use it to scale back
            LastPositiveTime = _instance.CurrentTime; 
        }
    }

    /// <summary>
    /// Reverses back and finds last element before current time
    /// </summary>
    private void ReverseToPreviousElementIfNeeded()
    {
        while (true)
        {
            if( LastElementActual.Element.Time > 0.0f )
            {
                if( LastElementActual.Element.Time > _instance.CurrentTime )
                {
                    //consume and reverse that element
                    ReverseOneElement();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
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

                    v2 = v1 - currentElement.DeltaPosition;

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

                var v1 = LastElementActual.Position - Elements[CurrentPosition].DeltaPosition;
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

    private void ReverseOneElement()
    {
        LastPositiveTime = LastElementActual.Element.Time;

        //cleanup this element so it can't be used anymore
        LastElementActual.Element.Time = 0.0f;

        //go back to previous element
        CurrentPosition = PreviousPosition;

        //assign
        LastElementActual.Element.Time = Elements[CurrentPosition].Time;
        LastElementActual.Position = transform.position - Elements[CurrentPosition].DeltaPosition;
        LastElementActual.Rotation = Elements[CurrentPosition].DeltaRotation * Quaternion.Inverse(transform.rotation);
    }

    private void RememberPosition(int index)
    {
        //Record changes (deltas against current values)
        Elements[index].DeltaPosition = transform.position - LastElementActual.Position;
        Elements[index].DeltaRotation = LastElementActual.Rotation * Quaternion.Inverse(transform.rotation);
        Elements[index].Time = _instance.CurrentTime;

        //remember new position and rotation
        LastElementActual.Element.Time = _instance.CurrentTime;
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
        LastElementActual.Element.Time = _instance.CurrentTime;
        LastElementActual.Position = transform.position;
        LastElementActual.Rotation = transform.rotation;

        RememberPosition(CurrentPosition);
    }

    public void RotateAndPosition(Vector3 position, Quaternion rotation)
    {
        if( _instance.AffectionTimeScale > 0.0f )
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
        //else - should apply reverse instead
    }
}
