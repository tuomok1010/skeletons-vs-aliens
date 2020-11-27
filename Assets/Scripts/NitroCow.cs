using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroCow : NormalCow
{
    [System.Serializable] struct ExplosionEffect
    {
        public float explosionForce;
        public float upwardsForceModifier;
        public float explosionSensitivity; // NOTE: this will overwrite collisionVelocityToDie
    }

    [SerializeField] ExplosionEffect explosionEffect;

    ParticleSystem fireballEffect;
    ParticleSystem sparksEffect;
    NitroCow nitroCow;
    Transform explosionArea;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.NITRO;
        enableCollisionDamage = true;
        collisionVelocityToDie = explosionEffect.explosionSensitivity;

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

        explosionArea = transform.parent.Find("ExplosionForceArea").gameObject.transform;
        if(!explosionArea)
        {
            Debug.Log("Error! Could not find ExplosionForceArea on " + gameObject.name);
        }
        else if (explosionArea.transform.localScale.x != explosionArea.transform.localScale.y &&
            explosionArea.transform.localScale.x != explosionArea.transform.localScale.z)
        {
            Debug.Log("Warning! ExplosionForceArea should be a perfect sphere!");
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
    }

    private void OnCollisionEnter(Collision collision)
    {
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
        Collider[] objects = Physics.OverlapSphere(explosionArea.position, explosionArea.localScale.x);

        for(int i = 0; i < objects.Length; ++i)
        {
            Rigidbody objectRb = objects[i].GetComponent<Rigidbody>();
            if(objectRb)
            {
                if(objectRb != rb)
                {
                    objectRb.AddExplosionForce(explosionEffect.explosionForce, explosionArea.position,
                        explosionArea.localScale.x, explosionEffect.upwardsForceModifier);
                }
            }
        }
    }
}
