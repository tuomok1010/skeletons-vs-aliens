using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingCow : NormalCow
{
    [SerializeField] float freezeAbilityCooldownInSeconds;

    bool effectReady = true;
    float cooldownTimeElapsed = 0.0f;
    FreezingCow freezingCow;
    ParticleSystem frostEffect;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.FREEZING;

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

        freezingCow = gameObject.GetComponent<FreezingCow>();

        // NOTE: frost effect must be the 2nd child of the parent!
        frostEffect = transform.GetChild(2).GetComponent<ParticleSystem>();
        if (!frostEffect)
        {
            Debug.Log("Error! Could not find frostEffect on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();
     
        if(effectReady)
        {
            if (frostEffect.isStopped)
                frostEffect.Play();

            if(isPickedUp)
            {
                effectReady = false;

                if (clawOwner.tag == "Player1")
                {
                    GameObject ownerBase = GameObject.FindGameObjectWithTag("Player1Base");
                    FreezeCowsInBase(ref ownerBase);
                }
                else if (clawOwner.tag == "Player2")
                {
                    GameObject ownerBase = GameObject.FindGameObjectWithTag("Player2Base");
                    FreezeCowsInBase(ref ownerBase);
                }
            }
        }
        else
        {
            frostEffect.Stop();
        }

        if(cooldownTimeElapsed >= freezeAbilityCooldownInSeconds)
        {
            effectReady = true;
            cooldownTimeElapsed = 0.0f;
            isPickedUp = false;
        }

        cooldownTimeElapsed += Time.deltaTime;
    }

    void FreezeCowsInBase(ref GameObject baseToFreeze)
    {
        // TODO: instead of looping through all of the cows, find a better solution!

        Debug.Log("Freezing all cows in base " + baseToFreeze.name);

        GameObject baseOwner = baseToFreeze.GetComponent<BaseController>().GetOwner();
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");

        for(int i = 0; i < cows.Length; ++i)
        {
            NormalCow cow = cows[i].GetComponent<NormalCow>();
            if (cow.isCaptured && cow.baseOwner == baseOwner)
            {
                cow.Freeze();
            }
        }
    }
}
