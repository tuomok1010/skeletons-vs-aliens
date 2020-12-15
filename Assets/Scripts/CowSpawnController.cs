using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawnController : MonoBehaviour
{
    [System.Serializable] struct SpawnSettings
    {
        public int maxCowsInLevel;
        public int cowSpawnRateInSeconds;

        public int normalCowSpawnChancePercentage;
        public int freezingCowSpawnChancePercentage;
        public int hotCowSpawnChancePercentage;
        public int nitroCowSpawnChancePercentage;
        public int cloudCowSpawnChancePercentage;

        public float minLaunchForwardForce;
        public float maxLaunchForwardForce;
        public float minLaunchUpwardForce;
        public float maxLaunchUpwardForce;
        public float minLaunchAngleDegrees;
        public float maxLaunchAngleDegrees;

        public GameObject normalCow;
        public GameObject freezingCow;
        public GameObject hotCow;
        public GameObject nitroCow;
        public GameObject cloudCow;
    }

    [SerializeField] SpawnSettings spawnSettings;

    GameObject spawnedCow;
    Vector3 spawnLocation;
    NormalCow.CowType typeToSpawn;
    int currentNumCows;
    float timeElapsed;

    private void Awake()
    {
        spawnedCow = null;
        spawnLocation = gameObject.transform.position;
        typeToSpawn = NormalCow.CowType.NORMAL;
        currentNumCows = 0;
        timeElapsed = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT: Do we know for sure this gets called AFTER all cows placed in the editor are initialised in the level?
        currentNumCows = GameObject.FindGameObjectsWithTag("Cow").Length;
        Debug.Log("Starting number of cows: " + currentNumCows);

        if(spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            spawnSettings.hotCowSpawnChancePercentage + spawnSettings.nitroCowSpawnChancePercentage +
            spawnSettings.cloudCowSpawnChancePercentage != 100)
        {
            Debug.Log("Warning! In CowSpawnController: Cow spawn chances should add up to 100");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameManager.GameState.GAME)
            return;

        timeElapsed += Time.deltaTime;

        if(timeElapsed >= spawnSettings.cowSpawnRateInSeconds)
        {
            currentNumCows = GameObject.FindGameObjectsWithTag("Cow").Length;

            if(currentNumCows < spawnSettings.maxCowsInLevel)
            {
                typeToSpawn = GetCowTypeToSpawn();
                SpawnCow(ref spawnedCow, typeToSpawn);
                LaunchCow(ref spawnedCow, typeToSpawn);
                timeElapsed = 0.0f;
            }
        }
    }

    NormalCow.CowType GetCowTypeToSpawn()
    {
        int result = Random.Range(1, 101);

        if (result >= 1 && result <= spawnSettings.normalCowSpawnChancePercentage)
        {
            // spawn normal cow
            //Debug.Log("Normal cow spawn percentage is " + spawnSettings.normalCowSpawnChancePercentage);
            //Debug.Log("Result was between " + 1 + " and " + spawnSettings.normalCowSpawnChancePercentage);
            return NormalCow.CowType.NORMAL;
        }
        else if ((result > spawnSettings.normalCowSpawnChancePercentage) &&
            (result <= spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage))
        {
            // spawn freezing cow
            //Debug.Log("Freezing cow spawn percentage is " + spawnSettings.freezingCowSpawnChancePercentage);
            //Debug.Log("Result was between " + (1 + spawnSettings.normalCowSpawnChancePercentage) + " and " +
            //    (spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage));
            return NormalCow.CowType.FREEZING;
        }
        else if ((result > spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage) &&
            (result <= spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            spawnSettings.hotCowSpawnChancePercentage))
        {
            // spawn hot cow
            //Debug.Log("Hot cow spawn percentage is " + spawnSettings.hotCowSpawnChancePercentage);
            //Debug.Log("Result was between " + (1 + spawnSettings.normalCowSpawnChancePercentage +
            //    spawnSettings.freezingCowSpawnChancePercentage) + " and " + (spawnSettings.normalCowSpawnChancePercentage +
            //    spawnSettings.freezingCowSpawnChancePercentage + spawnSettings.hotCowSpawnChancePercentage));
            return NormalCow.CowType.HOT;
        }
        else if ((result > spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            spawnSettings.hotCowSpawnChancePercentage) && (result <= spawnSettings.normalCowSpawnChancePercentage +
            spawnSettings.freezingCowSpawnChancePercentage + spawnSettings.hotCowSpawnChancePercentage +
            spawnSettings.nitroCowSpawnChancePercentage))
        {
            // spawn nitro cow
            //Debug.Log("Nitro cow spawn percentage is " + spawnSettings.nitroCowSpawnChancePercentage);
            //Debug.Log("Result was between " + (1 + spawnSettings.normalCowSpawnChancePercentage +
            //    spawnSettings.freezingCowSpawnChancePercentage + spawnSettings.hotCowSpawnChancePercentage) + " and " +
            //    (spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            //    spawnSettings.hotCowSpawnChancePercentage + spawnSettings.nitroCowSpawnChancePercentage));
            return NormalCow.CowType.NITRO;
        }
        else if ((result > spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            spawnSettings.hotCowSpawnChancePercentage + spawnSettings.nitroCowSpawnChancePercentage) &&
            (result <= spawnSettings.normalCowSpawnChancePercentage + spawnSettings.freezingCowSpawnChancePercentage +
            spawnSettings.hotCowSpawnChancePercentage + spawnSettings.nitroCowSpawnChancePercentage +
            spawnSettings.cloudCowSpawnChancePercentage))
        {
            // spawn cloud cow
            //Debug.Log("Cloud cow spawn percentage is " + spawnSettings.cloudCowSpawnChancePercentage);
            //Debug.Log("Result was between " + (1 + spawnSettings.normalCowSpawnChancePercentage +
            //    spawnSettings.freezingCowSpawnChancePercentage + spawnSettings.hotCowSpawnChancePercentage +
            //    spawnSettings.nitroCowSpawnChancePercentage) + " and " + (spawnSettings.normalCowSpawnChancePercentage +
            //    spawnSettings.freezingCowSpawnChancePercentage + spawnSettings.hotCowSpawnChancePercentage +
            //    spawnSettings.nitroCowSpawnChancePercentage + spawnSettings.cloudCowSpawnChancePercentage));
            return NormalCow.CowType.CLOUD;
        }
        else
        {
            Debug.Log("Warning! In CowSpawnController: void Update(): error calculating cow type to spawn. Spawning a normal cow");
            return NormalCow.CowType.NORMAL;
        }
    }

    void SpawnCow(ref GameObject cow, NormalCow.CowType type)
    {
        switch(type)
        {
            case NormalCow.CowType.NORMAL:
            {
                cow = GameObject.Instantiate(spawnSettings.normalCow, new Vector3(spawnLocation.x, 
                spawnLocation.y + 0.1f, spawnLocation.z), Quaternion.identity);

                cow.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            } break;

            case NormalCow.CowType.FREEZING:
            {
                cow = GameObject.Instantiate(spawnSettings.freezingCow, new Vector3(spawnLocation.x,
                    spawnLocation.y + 0.1f, spawnLocation.z), Quaternion.identity);

                cow.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            } break;

            case NormalCow.CowType.HOT:
            {
                cow = GameObject.Instantiate(spawnSettings.hotCow, new Vector3(spawnLocation.x,
                    spawnLocation.y + 0.1f, spawnLocation.z), Quaternion.identity);

                cow.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            } break;

            case NormalCow.CowType.NITRO:
            {
                cow = GameObject.Instantiate(spawnSettings.nitroCow, new Vector3(spawnLocation.x,
                    spawnLocation.y + 0.1f, spawnLocation.z), Quaternion.identity);

                cow.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            } break;

            case NormalCow.CowType.CLOUD:
            {
                cow = GameObject.Instantiate(spawnSettings.cloudCow, new Vector3(spawnLocation.x,
                    spawnLocation.y + 0.1f, spawnLocation.z), Quaternion.identity);

                cow.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            } break;
        }
    }

    void LaunchCow(ref GameObject cow, NormalCow.CowType type)
    {
        float forwardForce = Random.Range(
            type == NormalCow.CowType.CLOUD ? spawnSettings.minLaunchForwardForce / 2.0f : spawnSettings.minLaunchForwardForce, 
            type == NormalCow.CowType.CLOUD ? spawnSettings.maxLaunchForwardForce / 2.0f : spawnSettings.maxLaunchForwardForce);

        float upwardForce = Random.Range(
            type == NormalCow.CowType.CLOUD ? spawnSettings.minLaunchUpwardForce / 2.0f : spawnSettings.minLaunchUpwardForce, 
            type == NormalCow.CowType.CLOUD ? spawnSettings.maxLaunchUpwardForce / 2.0f : spawnSettings.maxLaunchUpwardForce);

        float rotationAmount = Random.Range(spawnSettings.minLaunchAngleDegrees, spawnSettings.maxLaunchAngleDegrees);
        CowMovementController.Direction dir = (CowMovementController.Direction)Random.Range(0, 2);

        if (dir == CowMovementController.Direction.LEFT)
            rotationAmount = -rotationAmount;

        //Debug.Log("Launching " + type + " with forward force: " + forwardForce + " upward force: " + upwardForce +
        //    " rotation: " + rotationAmount);

        Rigidbody rb = cow.transform.Find("MOOMOO").GetComponent<Rigidbody>();
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAmount));
        rb.MoveRotation(rb.rotation * deltaRotation);

        // FORWARD
        rb.AddRelativeForce(rb.transform.forward * forwardForce);

        // UPWARDS
        rb.AddRelativeForce(rb.transform.up * upwardForce);
    }
}
