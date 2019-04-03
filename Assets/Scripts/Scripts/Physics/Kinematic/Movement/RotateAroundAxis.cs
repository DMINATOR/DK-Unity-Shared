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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Speed != 0)
        {
            transform.Rotate(Rotation * Speed * TimeControlController.Instance.TimeScaleDelta);
        }
    }
}
