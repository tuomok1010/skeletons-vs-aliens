using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField]
    GameObject claw;
    [SerializeField]
    int playerNumber = 1;
    int lengthOfLineRenderer = 2;
    
    GameObject[] cablePoints;
    Vector3[] pointPositions;
    LineRenderer cable;

    void Start()
    {
        if (playerNumber == 1)
        {
            if (cablePoints == null)
            {
                cablePoints = GameObject.FindGameObjectsWithTag("Player1CablePoints");
            }
        }
        if (playerNumber == 2)
        {
            if (cablePoints == null)
            {
                cablePoints = GameObject.FindGameObjectsWithTag("Player2CablePoints");
            }
        }

        pointPositions = new Vector3[lengthOfLineRenderer];
        cable = gameObject.GetComponent<LineRenderer>();
        cable.positionCount = lengthOfLineRenderer; 
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            Vector3 pos = cablePoints[i].transform.position;
            pointPositions[i] = pos;
        }
        System.Array.Reverse(pointPositions);
        cable.SetPositions(pointPositions);
    }
}
