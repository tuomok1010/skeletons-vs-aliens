using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotCow : NormalCow
{
    [System.Serializable] struct speedBoost
    {
        public float speedAbilityCooldownInSeconds;
    }

    [SerializeField] speedBoost speedBoostAbility;

    bool effectReady = true;
    float cooldownTimeElapsed = 0.0f;
    HotCow hotCow;
    ParticleSystem flamesEffect;
    ParticleSystem emberEffect;

    private void Awake()
    {
        type = CowType.HOT;
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

        hotCow = GetComponent<HotCow>();

        flamesEffect = transform.Find("FireEffect").gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (!flamesEffect)
        {
            Debug.Log("Error! Could not find flamesEffect on " + gameObject.name);
        }

        emberEffect = transform.Find("FireEffect").gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        if (!emberEffect)
        {
            Debug.Log("Error! Could not find emberEffect on " + gameObject.name);
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
            if (flamesEffect.isStopped)
                flamesEffect.Play();

            if (emberEffect.isStopped)
                emberEffect.Play();

            if (isPickedUp)
            {
                effectReady = false;

                if (clawOwner)
                {
                    PlayerEffectController playerEffectController = clawOwner.GetComponent<PlayerEffectController>();

                    if (playerEffectController && !playerEffectController.hasSpeedBoost)
                        playerEffectController.EnableSpeedBoost();
                    else
                        Debug.Log("Error! HotCow.cs in function void Update(): Could not find PlayerEffectController");
                }
                else
                {
                    Debug.Log("Error! HotCow.cs void Update(): clawOwner not set!");
                }
            }
        }
        else
        {
            flamesEffect.Stop();
            emberEffect.Stop();
        }

        if (cooldownTimeElapsed >= speedBoostAbility.speedAbilityCooldownInSeconds)
        {
            effectReady = true;
            cooldownTimeElapsed = 0.0f;
        }

        cooldownTimeElapsed += Time.deltaTime;
    }
}
