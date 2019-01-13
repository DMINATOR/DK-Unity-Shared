using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveRigidBody3DOnKey : MonoBehaviour
{
    public float Force;
    public Vector3 Direction;

    public InputButton Button;

    Rigidbody rb3d;

    private void Start()
    {
        rb3d = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    private void FixedUpdate()
    {
        if (Input.GetButton(Button.KeyName))
        {
            float horizontal = Input.GetAxis(Button.KeyName);

            Vector3 position = rb3d.position;
            position.Set(
                rb3d.position.x + Direction.x * Force * Time.fixedDeltaTime * horizontal,
                rb3d.position.y + Direction.y * Force * Time.fixedDeltaTime * horizontal,
                rb3d.position.z + Direction.z * Force * Time.fixedDeltaTime * horizontal);

            rb3d.MovePosition(position);
        }

    }
}
