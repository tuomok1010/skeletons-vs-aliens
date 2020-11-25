using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStabilizer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float dampenFactor = 0.8f; // this value requires tuning
    [SerializeField]
    float adjustFactor = 0.5f; // this value requires tuning

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);
    }
}
