using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawnController : MonoBehaviour
{
    [SerializeField] int maxCowsInLevel;
    [SerializeField] int cowSpawnRateInSeconds;

    NormalCow.CowType typeToSpawn;
    int currentNumCows = 0;
    float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // IMPORTANT: Do we know for sure this gets called AFTER all cows placed in the editor are initialised in the level?
        currentNumCows = GameObject.FindGameObjectsWithTag("Cow").Length;
        Debug.Log("Starting number of cows: " + currentNumCows);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(currentNumCows < maxCowsInLevel && timeElapsed >= cowSpawnRateInSeconds)
        {


            timeElapsed = 0.0f;
        }
    }
}
