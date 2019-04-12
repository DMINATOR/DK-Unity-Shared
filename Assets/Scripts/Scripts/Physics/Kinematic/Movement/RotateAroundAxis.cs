using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates object around it's axis
/// </summary>
public class RotateAroundAxis : MonoBehaviour
{
    [Tooltip("Rotation speed (degrees/per second)")]
    [SerializeField]
    private float Speed = 0.0f ;

    [Tooltip("Rotation Vector")]
    [SerializeField]
    private Vector3 Rotation = Vector3.zero;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale TimeScaleInstance;

    void Start()
    {
        TimeScaleInstance = TimeControlController.Instance.CreateTimeScaleInstance(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Speed != 0)
        {
            TimeScaleInstance.Update();
            transform.Rotate(Rotation * Speed * TimeScaleInstance.TimeScaleDelta);
        }
    }
}
