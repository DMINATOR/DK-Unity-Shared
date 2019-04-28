using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnKey : MonoBehaviour
{
    [Tooltip("Rotation speed (degrees/per second)")]
    [SerializeField]
    public float RotationForce;

    [Tooltip("Movement speed (units/per second)")]
    [SerializeField]
    public float MovementForce;

    [Tooltip("Movement button to use for horizontal direction")]
    [SerializeField]
    public InputButton ButtonMoveHorizontal;

    [Tooltip("Movement button to use for vertical direction")]
    [SerializeField]
    public InputButton ButtonMoveVertical;

    [Tooltip("Rotation button to use for rotating object around its axis")]
    [SerializeField]
    public InputButton ButtonRotation;

    [Tooltip("Current Time Scale Instance assigned for this object")]
    [SerializeField]
    private TimeControlTimeScale _instance;

    void Start()
    {
        _instance = TimeControlController.Instance.CreateTimeScaleInstance(this);
    }

    void Update()
    {
        var vector = new Vector3();
        var horizontal = 0.0f;
        var vertical = 0.0f;

        _instance.Update();

        if (Input.GetButton(ButtonMoveHorizontal.KeyName))
        {
            horizontal = Input.GetAxis(ButtonMoveHorizontal.KeyName) * MovementForce * _instance.TimeScaleDelta;

            vector = Vector3.right * horizontal;
        }

        if (Input.GetButton(ButtonMoveVertical.KeyName))
        {
            vertical = Input.GetAxis(ButtonMoveVertical.KeyName) * MovementForce * _instance.TimeScaleDelta;

            vector += Vector3.forward * vertical;
        }

        float rotation = Input.GetAxis(ButtonRotation.KeyName) * RotationForce * _instance.TimeScaleDelta * Mathf.PI;

        if (horizontal != 0.0f || vertical != 0.0f || rotation != 0.0f)
        {
            var rotationQ = transform.rotation * Quaternion.Euler(Vector3.up * rotation);

            OnPositionChange(transform.position + transform.rotation * vector, rotationQ);
        }
    }

    protected virtual void OnPositionChange(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
