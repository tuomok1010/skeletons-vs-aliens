using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingCow : NormalCow
{
    [SerializeField] float freezeAbilityCooldownInSeconds;

    public static float freezeTimeInSeconds;

    bool effectReady = true;
    float cooldownTimeElapsed = 0.0f;
    FreezingCow freezingCow;
    ParticleSystem frostEffect;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.FREEZING;
        freezeTimeInSeconds = 10.0f;

        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        // NOTE: blood effect must be the 0th child of the parent!
        bloodEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (!bloodEffect)
        {
            Debug.Log("Error! Could not find bloodEffect on " + gameObject.name);
        }

        freezingCow = gameObject.GetComponent<FreezingCow>();

        // NOTE: frost effect must be the 1st child of the parent!
        frostEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        if (!frostEffect)
        {
            Debug.Log("Error! Could not find frostEffect on " + gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead && bloodEffect.isStopped)
        {
            // NOTE: OnTriggerExit() in BaseController will not be called when a cow is destroyed,
            //       that's why we need to do this here 
            if (isCaptured)
            {
                if (baseOwner.tag == "Player1")
                {
                    GameManager.player1.score -= scoreValue;
                }
                else if (baseOwner.tag == "Player2")
                {
                    GameManager.player2.score -= scoreValue;
                }
            }
            Destroy(gameObject);
        }
     
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
        }

        cooldownTimeElapsed += Time.deltaTime;
    }

    // IMPORTANT: NOT TESTED !!!
    void FreezeCowsInBase(ref GameObject baseToFreeze)
    {
        // TODO: instead of looping through all of the cows, find a better solution!

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
