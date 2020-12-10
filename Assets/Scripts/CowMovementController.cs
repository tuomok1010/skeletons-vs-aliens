using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT: the coordinates of the cow are different from Unity's coordinates. Instead of y being up, the up coordinate
// of the cow is z !!!

public class CowMovementController : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    [System.Serializable] struct MovementSettings
    {
        public int minHopsPerDirection;
        public int maxHopsPerDirection;

        public float minTurnRotationInDegrees;
        public float maxTurnRotationInDegrees;
    }

    [SerializeField] MovementSettings movementSettings;

    Rigidbody rb;
    NormalCow cow;
    CowRotator rotator;

    float timeBetweenHops;
    float timeElapsedBetweenHops;
    int currentHop;
    int numHops;

    float hopDuration;
    float timeElapsedHopping;
    float forwardSpeed;
    float upwardSpeed;

    // NOTE: changing turnDuration value will cause minTurnRotationInDegrees and maxTurnRotationInDegrees to NOT be in degrees
    float turnDuration;
    float timeElapsedTurning;
    float turnForce;

    Direction turnDirection;

    public bool isMoving { get; set; } = false;
    public bool isTurning { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Error! Could not find rigid body component on " + gameObject.name);
        }

        cow = GetComponent<NormalCow>();
        if(!cow)
        {
            Debug.Log("Error! Could not find cow script on " + gameObject.name);
        }

        rotator = GetComponent<CowRotator>();
        if(!rotator)
        {
            Debug.Log("Error! Could not find CowRotator on " + gameObject.name);
        }

        switch (cow.type)
        {
            // NOTE: cloud cow moves much faster than the others, and doesn't hop as high
            case NormalCow.CowType.CLOUD:
            {
                timeBetweenHops = 0.5f;
                timeElapsedBetweenHops = 0.0f;
                currentHop = 0;
                hopDuration = 0.25f;
                timeElapsedHopping = 0.0f;
                forwardSpeed = 3.0f;    
                upwardSpeed = 0.5f;     
                turnDuration = 1.0f;
                timeElapsedTurning = 0.0f;
                turnForce = 150.0f;
            } break;

            default:
            {
                timeBetweenHops = 0.5f;
                timeElapsedBetweenHops = 0.0f;
                currentHop = 0;
                hopDuration = 0.25f;
                timeElapsedHopping = 0.0f;
                forwardSpeed = 0.75f;
                upwardSpeed = 1.5f;
                turnDuration = 1.0f;
                timeElapsedTurning = 0.0f;
                turnForce = 150.0f;
            } break;
        }

        RandomizeMovementValues(movementSettings.minTurnRotationInDegrees, movementSettings.maxTurnRotationInDegrees,
            movementSettings.minHopsPerDirection, movementSettings.maxHopsPerDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cow.isActivated)
            return;

        if (cow.isFrozen)
            return;

        // NOTE: we do not want to move captured cows
        if (cow.capturedByFaction != GameManager.PlayerFaction.NONE)
            return;

        if (rotator.isCollidingWithClaw)
            return;

        timeElapsedBetweenHops += Time.deltaTime;
        bool isFacingObstacle = IsFacingObstacle();

        if (timeElapsedBetweenHops >= timeBetweenHops && !isTurning && !isFacingObstacle && numHops != 0)
        {
            if (rotator.isStandingUp)
                isMoving = true;

            timeElapsedBetweenHops = 0.0f;
        }
        else if (currentHop >= numHops || isFacingObstacle)
        {
            if (rotator.isStandingUp)
                isTurning = true;
        }
    }

    private void FixedUpdate()
    {
        if (!cow.isActivated)
            return;

        if (cow.isFrozen)
            return;

        // NOTE: we do not want to move captured cows
        if (cow.capturedByFaction != GameManager.PlayerFaction.NONE)
            return;

        if (rotator.isCollidingWithClaw)
            return;

        if (isMoving)
            Hop();
        else if (isTurning)
            Turn();
    }

    void Hop()
    {
        timeElapsedHopping += Time.deltaTime;
        if (timeElapsedHopping <= hopDuration)
        {
            Vector3 pos = rb.transform.position + (rb.transform.forward * upwardSpeed * Time.deltaTime);
            pos = pos + (-rb.transform.up * forwardSpeed * Time.deltaTime);
            rb.MovePosition(pos);
        }
        else
        {
            timeElapsedHopping = 0.0f;
            isMoving = false;
            ++currentHop;
        }
    }

    void Turn()
    {
        timeElapsedTurning += Time.deltaTime;
        if (timeElapsedTurning <= turnDuration)
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, turnForce) * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        else
        {
            timeElapsedTurning = 0.0f;
            currentHop = 0;
            isTurning = false;

            RandomizeMovementValues(movementSettings.minTurnRotationInDegrees, movementSettings.maxTurnRotationInDegrees,
                movementSettings.minHopsPerDirection, movementSettings.maxHopsPerDirection);
        }
    }

    bool IsFacingObstacle()
    {
        Ray rayR = new Ray(rb.transform.position + rb.transform.right * 0.4f, -rb.transform.up);
        Ray rayM = new Ray(rb.transform.position, -rb.transform.up);
        Ray rayL = new Ray(rb.transform.position - rb.transform.right * 0.4f, -rb.transform.up);

        if (Physics.Raycast(rayR, forwardSpeed * 1.2f) || 
            Physics.Raycast(rayM, forwardSpeed * 1.2f) || 
            Physics.Raycast(rayL, forwardSpeed * 1.2f))
        {
            //Debug.Log("Detected an obstacle!");
            return true;
        }
        else
        {
            return false;
        }
    }

    void RandomizeMovementValues(float minTurnForce, float maxTurnForce, int minHopsPerDirection, 
        int maxHopsPerDirection)
    {
        turnDirection = (Direction)Random.Range(0, 2);

        turnForce = Random.Range(minTurnForce, maxTurnForce);

        if (turnDirection == Direction.LEFT)
            turnForce = -turnForce;

        numHops = Random.Range(minHopsPerDirection, maxHopsPerDirection + 1);

        //Debug.Log("New direction: " + turnDirection + " New turn force: " + turnForce + " New num hops: " + numHops);
    }
}
