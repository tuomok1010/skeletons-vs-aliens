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
    public bool isInFreezeArea { get; set; }
    public bool isDead { get; set; }
    public bool isActivated { get; set; }       // cow gets activated when it first collides with ground after being launched from spawn
    public GameManager.PlayerFaction capturedByFaction { get; set; }
    public GameObject clawOwner { get; set; }   // if the cow is picked up, this is the owner of the claw

    protected Rigidbody rb;
    protected ParticleSystem bloodEffect;
    protected ParticleSystem cowFreezeEffect;
    protected float timeElapsedFrozen = 0.0f;

    // these are the global bounds of the field where the cows are. If an active cow is out of bounds, it will be destroyed.
    protected float minLevelXCoord = -12.6f;
    protected float maxLevelXCoord = 12.6f;
    protected float minLevelZCoord = -8.9f;
    protected float maxLevelZCoord = 8.9f;

    
    private void Awake()
    {
        type = CowType.NORMAL;
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
        if(!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        bloodEffect = transform.Find("BloodEffect").gameObject.GetComponent<ParticleSystem>();
        if(!bloodEffect)
        {
            Debug.Log("Error! Could not find bloodEffect on " + gameObject.name);
        }

        cowFreezeEffect = transform.Find("CowFreezeEffect").Find("ColdVapour").gameObject.GetComponent<ParticleSystem>();
        if(!cowFreezeEffect)
        {
            Debug.Log("Error! Could not find cowFreezeEffect on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfLevelBounds(minLevelXCoord, maxLevelXCoord, minLevelZCoord, maxLevelZCoord) && isActivated)
            isDead = true;

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
        if(!isActivated)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isActivated = true;
                IgnoreSpawnWallCollisions(false);
                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
            return;
        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FreezeArea")
        {
            //Debug.Log("isInFreezeArea set to true");
            isInFreezeArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FreezeArea")
        {
            //Debug.Log("isInFreezeArea set to false");
            isInFreezeArea = false;
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
            cowFreezeEffect.Play();
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

    protected void IgnoreSpawnWallCollisions(bool ignore)
    {
        int cowCollisionLayer = LayerMask.NameToLayer("Cow");
        int cowHookCollisionLayer = LayerMask.NameToLayer("CowHookCollision");

        int spawnWallCollisionLayer = LayerMask.NameToLayer("SpawnWallCollider");
        //int playerWallCollisionLayer = LayerMask.NameToLayer("Player");

        Physics.IgnoreLayerCollision(cowCollisionLayer, spawnWallCollisionLayer, ignore);
        Physics.IgnoreLayerCollision(cowHookCollisionLayer, spawnWallCollisionLayer, ignore);

        //Physics.IgnoreLayerCollision(cowCollisionLayer, playerWallCollisionLayer, ignore);
        //Physics.IgnoreLayerCollision(cowHookCollisionLayer, playerWallCollisionLayer, ignore);
    }

    protected bool IsOutOfLevelBounds(float minLevelXCoord, float maxLevelXCoord, float minLevelZCoord, float maxLevelZCoord)
    {
        if(transform.position.x < minLevelXCoord || transform.position.x > maxLevelXCoord || 
            transform.position.z < minLevelZCoord || transform.position.z > maxLevelZCoord)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
