using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public GameObject target;
    float distToTarget;

    Light spotLight;

    void Start()
    {
        spotLight = GetComponent<Light>();
    }

    void LateUpdate()
    {
        transform.LookAt(target.transform);
        distToTarget = Vector3.Distance(target.transform.position, transform.position);
        spotLight.intensity = distToTarget * 7.5f - 10f;
        spotLight.intensity = Mathf.Clamp(spotLight.intensity, 0f, 6f);
    }
}
