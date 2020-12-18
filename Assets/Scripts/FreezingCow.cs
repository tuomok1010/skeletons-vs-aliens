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
    AudioSource frostSound;

    private void Awake()
    {
        type = CowType.FREEZING;
        capturedByFaction = GameManager.PlayerFaction.NONE;
        isPickedUp = false;
        isFrozen = false;
        isDead = false;
        isActivated = false;
        isInFreezeArea = false;

        if (!isActivated)
        {
            IgnoreSpawnWallCollisions(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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

        /*         
        frostEffect = transform.Find("FrostEffect").gameObject.GetComponent<ParticleSystem>();
        if (!frostEffect)
        {
            Debug.Log("Error! Could not find frostEffect on " + gameObject.name);
        }
        else
        {
            frostEffect.Play();
        } 
        */

        cowFreezeEffect = transform.Find("CowFreezeEffect").Find("ColdVapour").gameObject.GetComponent<ParticleSystem>();
        if (!cowFreezeEffect)
        {
            Debug.Log("Error! Could not find cowFreezeEffect on " + gameObject.name);
        }

        mooSound = transform.Find("MooSound").gameObject.GetComponent<AudioSource>();
        if (!mooSound)
        {
            Debug.Log("Error! Could not find mooSound on " + gameObject.name);
        }

        frostSound = transform.Find("FrostSound").gameObject.GetComponent<AudioSource>();
        if(!frostSound)
        {
            Debug.Log("Error! Could not find frostSound on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfLevelBounds(minLevelXCoord, maxLevelXCoord, minLevelZCoord, maxLevelZCoord) && isActivated)
            isDead = true;

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
            if(capturedByFaction != GameManager.PlayerFaction.NONE && isInFreezeArea)
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
        timeElapsedBetweenMoos += Time.deltaTime;
    }

    public void FreezeAllCowsBelongingToFaction(GameManager.PlayerFaction faction)
    {
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");

        if(!frostSound.isPlaying)
            frostSound.Play();

        for (int i = 0; i < cows.Length; ++i)
        {
            NormalCow cow = cows[i].GetComponent<NormalCow>();
            if(cow && cow.capturedByFaction == faction && cow.isInFreezeArea)
            {
                cow.Freeze();
            }
        }
    }

    public static void UnfreezeAllCowsBelongingToFaction(GameManager.PlayerFaction faction)
    {
        GameObject[] cows = GameObject.FindGameObjectsWithTag("Cow");

        for (int i = 0; i < cows.Length; ++i)
        {
            NormalCow cow = cows[i].GetComponent<NormalCow>();
            if (cow && cow.capturedByFaction == faction && cow.isInFreezeArea)
            {
                cow.Unfreeze();
            }
        }
    }
}
