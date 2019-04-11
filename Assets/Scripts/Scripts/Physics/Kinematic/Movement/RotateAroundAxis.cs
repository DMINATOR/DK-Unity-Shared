using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates object around body
/// </summary>
public class RotateAroundAxis : MonoBehaviour
{
    [Tooltip("Rotation speed")]
    public float Speed;

    [Tooltip("Rotation Vector")]
    public Vector3 Rotation;

    [Tooltip("Current Time Scale Instance assigned for this")]
    public TimeControlTimeScale TimeScaleInstance;

    void Start()
    {
        TimeScaleInstance = TimeControlController.Instance.CreateTimeScaleInstance(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Speed != 0)
        {
            transform.Rotate(Rotation * Speed * TimeScaleInstance.TimeScaleDelta);
        }
    }
}
