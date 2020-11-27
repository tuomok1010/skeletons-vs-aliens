﻿using System.Collections;
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

        bloodEffect = transform.Find("BloodEffect").gameObject.GetComponent<ParticleSystem>();
        if (!bloodEffect)
        {
            Debug.Log("Error! Could not find bloodEffect on " + gameObject.name);
        }

        freezingCow = gameObject.GetComponent<FreezingCow>();

        frostEffect = transform.Find("FrostEffect").gameObject.GetComponent<ParticleSystem>();
        if (!frostEffect)
        {
            Debug.Log("Error! Could not find frostEffect on " + gameObject.name);
        }
        else
        {
            frostEffect.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();

        if (isFrozen)
        {
            timeElapsedFrozen += Time.deltaTime;
            if (timeElapsedFrozen >= freezeTimeInSeconds)
            {
                Unfreeze();
            }
        }

        if (effectReady)
        {
            if(capturedByFaction != GameManager.PlayerFaction.NONE)
            {
                effectReady = false;
                FreezeAllCowsBelongingToFaction(capturedByFaction);
            }
        }

        if(cooldownTimeElapsed >= freezeAbilityCooldownInSeconds)
        {
            effectReady = true;
            cooldownTimeElapsed = 0.0f;
        }

        cooldownTimeElapsed += Time.deltaTime;
    }

    void FreezeAllCowsBelongingToFaction(GameManager.PlayerFaction faction)
    {
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");

        for (int i = 0; i < cows.Length; ++i)
        {
            NormalCow cow = cows[i].GetComponent<NormalCow>();
            if(cow.capturedByFaction == faction)
            {
                cow.Freeze();
            }
        }
    }
}
