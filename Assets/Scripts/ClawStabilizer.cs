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
    [SerializeField]
    GameObject playerVehicle;
    [SerializeField]
    float stablizingForce= 2f; 

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        StabilizeTorque();
        StabilizeVelocity();

    }

    void StabilizeTorque()
    {
        Quaternion deltaQuat = Quaternion.FromToRotation(rb.transform.up, Vector3.up);

        Vector3 axis;
        float angle;
        deltaQuat.ToAngleAxis(out angle, out axis);

        rb.AddTorque(-rb.angularVelocity * dampenFactor, ForceMode.Acceleration);

        rb.AddTorque(axis.normalized * angle * adjustFactor, ForceMode.Acceleration);
    }

    void StabilizeVelocity()
	{
        Vector3 playerLateral;
        playerLateral = new Vector3(playerVehicle.transform.position.x, this.transform.position.y, playerVehicle.transform.position.z);
        Vector3 dir = (this.transform.position - playerLateral);
        rb.AddForce(dir * -stablizingForce);
        //Debug.DrawLine (this.transform.position, this.transform.position - dir, Color.red, 2f);
	}
}
