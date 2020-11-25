using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotCow : NormalCow
{
    [SerializeField] float speedAbilityCooldownInSeconds;

    bool effectReady = true;
    float cooldownTimeElapsed = 0.0f;
    HotCow hotCow;
    ParticleSystem flamesEffect;
    ParticleSystem emberEffect;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.HOT;

        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        // NOTE: blood effect must be the 1st child of the parent!
        bloodEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        if (!bloodEffect)
        {
            Debug.Log("Error! Could not find bloodEffect on " + gameObject.name);
        }

        hotCow = gameObject.GetComponent<HotCow>();

        // NOTE: fire effect must be the 2nd child of the parent!
        flamesEffect = transform.GetChild(2).transform.GetChild(0).GetComponent<ParticleSystem>();
        if (!flamesEffect)
        {
            Debug.Log("Error! Could not find flamesEffect on " + gameObject.name);
        }

        emberEffect = transform.GetChild(2).transform.GetChild(1).GetComponent<ParticleSystem>();
        if (!emberEffect)
        {
            Debug.Log("Error! Could not find emberEffect on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();

        if (effectReady)
        {
            if (flamesEffect.isStopped)
                flamesEffect.Play();

            if (emberEffect.isStopped)
                emberEffect.Play();

            if (isPickedUp)
            {
                effectReady = false;
                GameObject player = clawOwner;
                GiveSpeedBoost(ref player);
            }
        }
        else
        {
            flamesEffect.Stop();
            emberEffect.Stop();
        }

        if (cooldownTimeElapsed >= speedAbilityCooldownInSeconds)
        {
            effectReady = true;
            cooldownTimeElapsed = 0.0f;
            isPickedUp = false;
        }

        cooldownTimeElapsed += Time.deltaTime;
    }

    void GiveSpeedBoost(ref GameObject player)
    {

    }
}
