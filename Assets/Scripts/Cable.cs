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

    void Start()
    {
        if (playerNumber == 1)
        {
            if (cablePoints == null)
            {
                cablePoints = GameObject.FindGameObjectsWithTag("Player1Cables");
            }
        }
        if (playerNumber == 2)
        {
            if (cablePoints == null)
            {
                cablePoints = GameObject.FindGameObjectsWithTag("Player2Cables");
            }
        }

        LineRenderer cable = gameObject.GetComponent<LineRenderer>();
        cable.positionCount = lengthOfLineRenderer;
   
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer cable = gameObject.GetComponent<LineRenderer>();
        var points = new Vector3[lengthOfLineRenderer];
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            Vector3 pos = cablePoints[i].transform.position;
            points[i] = pos;
        }
        cable.SetPositions(points);
    } 
}
