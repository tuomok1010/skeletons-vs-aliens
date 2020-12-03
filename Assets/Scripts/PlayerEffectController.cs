using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [System.Serializable] struct SpeedEffectSettings
    {
        public float speedEffectDuration;
        public float speedEffectMultiplier;
    }

    [SerializeField] SpeedEffectSettings speedEffectsSettings;

    ECM.Components.CharacterMovement CM;
    ECM.Controllers.BaseCharacterController BCC;
    TrailRenderer[] speedEffects;

    float playerOldMaxLateralSpeed;
    float playerOldSpeed;
    float playerOldAcceleration;

    public bool hasSpeedBoost = false;
    float timeElapsedInSpeedEffect = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CM = gameObject.GetComponent<ECM.Components.CharacterMovement>();
        if(!CM)
        {
            Debug.Log("Error! Could not find CharacterMovement in " + gameObject.name);
        }

        BCC = gameObject.GetComponent<ECM.Controllers.BaseCharacterController>();
        if(!BCC)
        {
            Debug.Log("Error! Could not find BaseCharacterController in " + gameObject.name);
        }

        // NOTE: speed effect must be the 3rd child of the parent!
        speedEffects = gameObject.transform.GetChild(3).GetComponentsInChildren<TrailRenderer>();
        if(speedEffects.Length <= 0)
        {
            Debug.Log("Error! Could not find speedEffects in " + gameObject.name);
        }

        playerOldMaxLateralSpeed = CM.maxLateralSpeed;
        playerOldSpeed = BCC.speed;
        playerOldAcceleration = BCC.acceleration;

        DisableSpeedBoost();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasSpeedBoost)
        {
            timeElapsedInSpeedEffect += Time.deltaTime;
            if(timeElapsedInSpeedEffect >= speedEffectsSettings.speedEffectDuration)
            {
                DisableSpeedBoost();
            }
        }
    }

    // NOTE: This function will be called in HotCow.cs
    public void EnableSpeedBoost()
    {
        timeElapsedInSpeedEffect = 0.0f;
        hasSpeedBoost = true;

        BCC.speed *= speedEffectsSettings.speedEffectMultiplier;
        BCC.acceleration *= speedEffectsSettings.speedEffectMultiplier;

        if (CM.maxLateralSpeed < BCC.speed)
            CM.maxLateralSpeed = BCC.speed;

        //Debug.Log(gameObject.name + " enabled speed boost! New speed: " + BCC.speed + " New max speed: " + CM.maxLateralSpeed);

        for (int i = 0; i < speedEffects.Length; ++i)
            speedEffects[i].enabled = true;
    }

    void DisableSpeedBoost()
    {
        timeElapsedInSpeedEffect = 0.0f;
        hasSpeedBoost = false;

        CM.maxLateralSpeed = playerOldMaxLateralSpeed;
        BCC.speed = playerOldSpeed;
        BCC.acceleration = playerOldAcceleration;

        //Debug.Log(gameObject.name + " disabled speed boost! New speed: " + BCC.speed + " New max speed: " + CM.maxLateralSpeed);

        for (int i = 0; i < speedEffects.Length; ++i)
            speedEffects[i].enabled = false;
    }
}
