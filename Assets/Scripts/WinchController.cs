using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchController : MonoBehaviour
{
    
    SpringJoint winch;
    float winchLength;
    float winchMaxLengh = 11f;
    float reelOutSpeed = 10f;
    float reelInSpeed = 5f;
    void Awake()
    {
        if (winch == null)
        {
            winch = gameObject.GetComponent<SpringJoint>();
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
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
