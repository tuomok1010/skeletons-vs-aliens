using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCow : NormalCow
{
    [SerializeField] bool rotateCloudWithCow;

    ParticleSystem cloudEffect;
    Quaternion originalParticleRotation;

    private void Awake()
    {
        type = CowType.CLOUD;
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

        cloudEffect = transform.Find("CloudEffect").gameObject.GetComponent<ParticleSystem>();
        if (!cloudEffect)
        {
            Debug.Log("Error! Could not find cloudEffect on " + gameObject.name);
        }
        else
        {
            originalParticleRotation = cloudEffect.transform.rotation;
        }
    }

    private void Update()
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

        if(!rotateCloudWithCow)
        {
            cloudEffect.transform.SetPositionAndRotation(cloudEffect.transform.parent.position,
                originalParticleRotation);
        }
    }
}
