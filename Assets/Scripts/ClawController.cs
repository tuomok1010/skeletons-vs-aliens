using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    bool clawOpen = false;
    Animator anim;
    float currentClawWeight;
    float newClawWeight;
    float clawWeightTarget;
    float clawVelocity; 
    float smoothTime = 0.1f; 

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        currentClawWeight = anim.GetLayerWeight(1);

        if (Input.GetButton("Fire1") && (!clawOpen))
        {
            
            //anim.SetLayerWeight(1, newClawWeight);
            clawWeightTarget = 1f;
            clawOpen = true;
        }

        if (!Input.GetButton("Fire1") && (clawOpen))
        {
            //anim.SetLayerWeight(1, newClawWeight);
            clawWeightTarget = 0f;
            clawOpen = false;
        }
        MoveClaw();
    }
    
    void MoveClaw()
    {
        newClawWeight = Mathf.SmoothDamp(currentClawWeight, clawWeightTarget, ref clawVelocity, smoothTime);
        anim.SetLayerWeight(1, newClawWeight);
    }
}
