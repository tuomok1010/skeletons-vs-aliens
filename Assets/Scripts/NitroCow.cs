using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroCow : NormalCow
{
    [System.Serializable] struct ExplosionEffect
    {
        public float explosionForce;
        public float explosionRadius;
        public float upwardsForceModifier;   
        public float explosionSensitivity;  // NOTE: this will overwrite collisionVelocityToDie, smaller value = explodes easier
    }

    [SerializeField] ExplosionEffect explosionEffect;

    ParticleSystem fireballEffect;
    ParticleSystem sparksEffect;
    NitroCow nitroCow;

    private void Awake()
    {
        type = CowType.NITRO;
        capturedByFaction = GameManager.PlayerFaction.NONE;
        isPickedUp = false;
        isFrozen = false;
        isDead = false;
        isActivated = false;
        enableCollisionDamage = true;
        collisionVelocityToDie = explosionEffect.explosionSensitivity;
        isInFreezeArea = false;
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

        nitroCow = GetComponent<NitroCow>();

        fireballEffect = transform.Find("ExplosionEffect").gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        if (!fireballEffect)
        {
            Debug.Log("Error! Could not find fireballEffect on " + gameObject.name);
        }

        sparksEffect = transform.Find("ExplosionEffect").gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        if (!sparksEffect)
        {
            Debug.Log("Error! Could not find sparksEffect on " + gameObject.name);
        }

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

        if (!isActivated)
        {
            IgnoreSpawnWallCollisions(true);
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

        timeElapsedBetweenMoos += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActivated)
        {         
            if (collision.gameObject.tag == "Ground")
            {
                isActivated = true;
                IgnoreSpawnWallCollisions(false);
                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
            return;
        }

        if (enableCollisionDamage)
        {
            float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
            if (relativeVelocityMagnitude >= collisionVelocityToDie && !isDead)
            {
                if (enableBlood)
                    bloodEffect.Play();

                fireballEffect.Play();
                sparksEffect.Play();

                Explode();

                isDead = true;
            }
        }

        if (collision.gameObject.tag == "Claw")
        {
            if (timeElapsedBetweenMoos >= timeBetweenMoos)
            {
                mooSound.Play();
                timeElapsedBetweenMoos = 0.0f;
            }
        }
    }

    public new void HandleDeath()
    {
        if (isDead && bloodEffect.isStopped && fireballEffect.isStopped && sparksEffect.isStopped)
        {
            // NOTE: OnTriggerExit() in BaseController will not be called when a cow is destroyed,
            //       that's why we need to do this here 
            if (capturedByFaction != GameManager.PlayerFaction.NONE)
            {
                // NOTE: negative scoreValue reduces score instead of increasing
                GameManager.IncreaseScore(capturedByFaction, -scoreValue);
            }

            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider[] objects = Physics.OverlapSphere(gameObject.transform.position, explosionEffect.explosionRadius);

        for(int i = 0; i < objects.Length; ++i)
        {
            Rigidbody objectRb = objects[i].GetComponent<Rigidbody>();
            if(objectRb)
            {
                // NOTE: CowRotator.cs manipulates constraints. We need to make sure there are no constraints
                // when the cow is about to take explosion force
                if(objectRb.gameObject.tag == "Cow")
                {
                    objectRb.constraints = RigidbodyConstraints.None;
                }

                if(objectRb != rb)
                {
                    objectRb.AddExplosionForce(explosionEffect.explosionForce, gameObject.transform.position,
                        explosionEffect.explosionRadius, explosionEffect.upwardsForceModifier);
                }
            }
        }
    }
}
