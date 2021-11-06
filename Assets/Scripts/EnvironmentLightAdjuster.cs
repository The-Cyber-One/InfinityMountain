using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvironmentLightAdjuster : MonoBehaviour
{
    [SerializeField]
    [TagSelector]
    string playerTag;
    [SerializeField]
    Collider2D collider;
    [SerializeField]
    Light2D light;
    [SerializeField]
    [Min(0)]
    float min, max;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        float intensity = Mathf.InverseLerp(collider.bounds.min.y, collider.bounds.max.y, collision.transform.position.y);
        light.intensity = Mathf.Lerp(min, max, intensity);
    }
}
