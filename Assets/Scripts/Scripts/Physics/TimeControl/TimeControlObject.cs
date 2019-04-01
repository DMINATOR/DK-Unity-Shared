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


public class TimeControlObject : MonoBehaviour
{
    [Tooltip("Current index within Elements")]
    [SerializeField]
    private int CurrentPosition;

    [Tooltip("Time control records")]
    [SerializeField]
    private TimeControlElements[] Elements;

    /// <summary>
    /// Previous position for the last object
    /// </summary>
    public int PreviousPosition
    {
        get
        {
            if( CurrentPosition <= 0 )
            {
                return Elements.Length - 1;
            }
            else
            {
                return (CurrentPosition - 1) % Elements.Length;
            }
        }
    }

    public TimeControlObject()
    {
        Elements = new TimeControlElements[TimeControlController.Instance.ElementsSize];
    }

    private void Start()
    {
        TimeControlController.Instance.Register(this);

        RememberPosition(0);
    }

    private void OnDestroy()
    {
        TimeControlController.Instance.UnRegister(this);
    }

    private void DebugDrawPositions()
    {
        if (GameController.Instance.DebugShowControlObjects)
        {
            var divider = 1.0f / Elements.Length;
            var value = divider * CurrentPosition;

            Debug.DrawLine(
                Elements[PreviousPosition].Position,
                Elements[CurrentPosition].Position,
                new Color(value, value, value),
                GameController.Instance.DebugShowControlObjectsTime);
        }
    }

    private void RememberPosition(int index)
    {
        //Record changes
        Elements[index].Position = transform.position;
        Elements[index].Rotation = transform.rotation;
        Elements[index].DeltaTime = Time.realtimeSinceStartup;

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

    public void LogAndTranslateTo(Vector3 position, Quaternion rotation)
    {
        //Apply transform
        transform.position = position;
        transform.rotation = rotation;

        var lastElement = Elements[CurrentPosition];

        if (
            //Position
            AreDifferent(lastElement.Position.x, position.x) ||
            AreDifferent(lastElement.Position.y, position.y) ||
            AreDifferent(lastElement.Position.z, position.z) ||

            //Rotation
            AreDifferent(lastElement.Rotation.x, rotation.x) ||
            AreDifferent(lastElement.Rotation.y, rotation.y) ||
            AreDifferent(lastElement.Rotation.z, rotation.z)
            )
        {
            CurrentPosition = (CurrentPosition + 1) % Elements.Length;

            RememberPosition(CurrentPosition);
        }
    }
}
