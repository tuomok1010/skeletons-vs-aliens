using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchController : MonoBehaviour
{
    
    SpringJoint winch;
    float winchLength;
    [SerializeField] float winchMaxLengh = 11f;
    [SerializeField] float reelOutSpeed = 10f;
    [SerializeField] float reelInSpeed = 5f;
    void Awake()
    {
        if (winch == null)
        {
            winch = gameObject.GetComponent<SpringJoint>();
        }
    }

    void Update()
    {
        if (gameObject.tag=="Player1")
        {
            if (Input.GetButton("P1Winch"))
            {
                winchLength += (reelOutSpeed * Time.deltaTime);
            }
            else
            {
                winchLength -= (reelInSpeed * Time.deltaTime);
            }
            winchLength = Mathf.Clamp(winchLength, 0f, winchMaxLengh);
            winch.maxDistance = winchLength;
        }
        if (gameObject.tag=="Player2")
        {
            if (Input.GetButton("P2Winch"))
            {
                winchLength += (reelOutSpeed * Time.deltaTime);
            }
            else
            {
                winchLength -= (reelInSpeed * Time.deltaTime);
            }
            winchLength = Mathf.Clamp(winchLength, 0f, winchMaxLengh);
            winch.maxDistance = winchLength;
        }
    }
}
