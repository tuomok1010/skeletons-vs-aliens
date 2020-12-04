using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT: the coordinates of the cow are different from Unity's coordinates. Instead of y being up, the up coordinate
// of the cow is z !!!

public class CowRotator : MonoBehaviour
{
    Rigidbody rb;
    NormalCow cow;
    bool isCollidingWithClaw = false;
    bool isStandingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        cow = GetComponent<NormalCow>();
        if(!cow)
        {
            Debug.Log("Error! Could not find cow script on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isStandingUp = CheckIfStandingUp();

        if (!isStandingUp && !isCollidingWithClaw && !cow.isFrozen && rb.velocity.magnitude <= 0.05f)
        {
            StandUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Claw")
        {
            isCollidingWithClaw = true;
        }

        if(collision.gameObject.tag != "Ground")
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Claw")
        {
            isCollidingWithClaw = false;
        }
    }

    void StandUp()
    {
        // basically all this alligns z axis with world up vector
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Quaternion upRot = Quaternion.FromToRotation(transform.forward, Vector3.up);
        upRot *= transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, upRot, Time.deltaTime);

        // we can't just check if upRot == transform.rotation because the rotation seems to slow down as it
        // gets closer to the correct rotation. This causes a small delay before the rigid body is unfrozen.
        // That's why we add an error margin of 0.06f
        float errorMargin = 0.06f;

        if ((upRot.x >= transform.rotation.x - errorMargin && upRot.x <= transform.rotation.x + errorMargin) &&
            upRot.y >= transform.rotation.y - errorMargin && upRot.y <= transform.rotation.y + errorMargin &&
            upRot.z >= transform.rotation.z - errorMargin && upRot.z <= transform.rotation.z + errorMargin &&
            upRot.w >= transform.rotation.w - errorMargin && upRot.w <= transform.rotation.w + errorMargin)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    bool CheckIfStandingUp()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        int layerMask = 1 << LayerMask.NameToLayer("Ground"); // only the ground will toggle the Raycast

        if (Physics.Raycast(ray, 0.12f, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
