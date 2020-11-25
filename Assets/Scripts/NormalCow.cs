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

    [SerializeField] public CowType type;
    [SerializeField] public int scoreValue;     // How much score the cow gives to the player when captured
    [SerializeField] float freezeTimeInSeconds; // If the cow gets frozen, how long it takes until it becomes unfrozen again
    [SerializeField] Material normalMaterial;
    [SerializeField] Material frozenMaterial;

    public bool isPickedUp { get; set; }        // is the cow currently picked up by the claw
    public bool isCaptured { get; set; }                 
    public bool isFrozen { get; set; }
    public bool isDead { get; set; }
    public GameObject baseOwner { get; set; }   // if the cow is captured, this is the owner of the base
    public GameObject clawOwner { get; set; }   // if the cow is picked up, this is the owner of the claw

    protected Rigidbody rb;
    protected ParticleSystem bloodEffect;
    protected float collisionVelocityToDie = 10.0f;       // the velocity required upon collision that kills the cow

    float timeElapsedFrozen = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        type = CowType.NORMAL;
        isPickedUp = false;
        isCaptured = false;
        isFrozen = false;
        isDead = false;

        rb = GetComponent<Rigidbody>();
        if(!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        // NOTE: blood effect must be the 1st child of the parent!
        bloodEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
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
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if(relativeVelocityMagnitude >= collisionVelocityToDie && !isDead)
        {
            // TODO: should we change the cow mesh into a dead cow mesh?

            Debug.Log(gameObject.name + " dies with a relative velocity of " + relativeVelocityMagnitude);
            bloodEffect.Play();
            isDead = true;
        }
    }

    public void HandleDeath()
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
    }

    // NOTE: This function will be called in FreezingCow.cs
    public void Freeze()
    {
        if(!isFrozen)
        {
            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = frozenMaterial;
            rb.isKinematic = true;  // physics will not affect the cow, effectively "freezing" it in place
        }

        isFrozen = true;
        timeElapsedFrozen = 0.0f;   // everytime a new freeze effect happens, timer starts from 0
    }

    void Unfreeze()
    {
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = normalMaterial;
        isFrozen = false;
        rb.isKinematic = false; // enable physics again
    }
}
