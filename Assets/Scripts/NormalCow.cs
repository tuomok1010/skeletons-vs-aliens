using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCow : MonoBehaviour
{
    public enum CowType
    {
        NORMAL,
        FREEZING,
        HOT,
        NITRO,
        CLOUD
    }

    [SerializeField] public int scoreValue;     // How much score the cow gives to the player when captured
    [SerializeField] protected float freezeTimeInSeconds; // If the cow gets frozen, how long it takes until it becomes unfrozen again
    [SerializeField] protected bool enableBlood;
    [SerializeField] protected bool enableCollisionDamage;
    [SerializeField] protected float collisionVelocityToDie;
    [SerializeField] protected Material normalMaterial;
    [SerializeField] protected Material frozenMaterial;

    public CowType type { get; set; }
    public bool isPickedUp { get; set; }        // is the cow currently picked up by the claw             
    public bool isFrozen { get; set; }
    public bool isDead { get; set; }
    public GameManager.PlayerFaction capturedByFaction { get; set; }
    public GameObject clawOwner { get; set; }   // if the cow is picked up, this is the owner of the claw

    protected Rigidbody rb;
    protected ParticleSystem bloodEffect;
    protected float timeElapsedFrozen = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.NORMAL;
        capturedByFaction = GameManager.PlayerFaction.NONE;
        isPickedUp = false;
        isFrozen = false;
        isDead = false;

        rb = GetComponent<Rigidbody>();
        if(!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        bloodEffect = transform.Find("BloodEffect").gameObject.GetComponent<ParticleSystem>();
        if(!bloodEffect)
        {
            Debug.Log("Error! Could not find bloodEffect on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();

        if(isFrozen)
        {
            timeElapsedFrozen += Time.deltaTime;
            if(timeElapsedFrozen >= freezeTimeInSeconds)
            {
                Unfreeze();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enableCollisionDamage)
        {
            float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
            if (relativeVelocityMagnitude >= collisionVelocityToDie && !isDead)
            {
                if (enableBlood)
                    bloodEffect.Play();
                isDead = true;
            }
        }
    }

    public void HandleDeath()
    {
        if (isDead && bloodEffect.isStopped)
        {
            // NOTE: OnTriggerExit() in BaseController will not be called when a cow is destroyed,
            //       that's why we need to do this here 
            if(capturedByFaction != GameManager.PlayerFaction.NONE)
            {
                // NOTE: negative scoreValue reduces score instead of increasing
                GameManager.IncreaseScore(capturedByFaction, -scoreValue);
            }

            Destroy(gameObject);
        }
    }

    // NOTE: This function will be called in FreezingCow.cs
    public void Freeze()
    {
        if(!isFrozen)
        {
            GetComponent<MeshRenderer>().material = frozenMaterial;
            rb.isKinematic = true;  // physics will not affect the cow, effectively "freezing" it in place
        }

        isFrozen = true;
        timeElapsedFrozen = 0.0f;   // everytime a new freeze effect happens, timer starts from 0
    }

    public void Unfreeze()
    {
        GetComponent<MeshRenderer>().material = normalMaterial;
        isFrozen = false;
        rb.isKinematic = false; // enable physics again
        timeElapsedFrozen = 0.0f;
    }
}
