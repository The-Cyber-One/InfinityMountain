using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LavaMovement : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Light2D light;
    [SerializeField]
    Collider2D collider;
    [SerializeField]
    Rigidbody2D rigidbody;
    [SerializeField]
    [TagSelector]
    string playerTag;

    [Header("Start movement")]
    [SerializeField]
    OnTriggerEnter2DEvent startTrigger;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    float smoothing = 0.1f;

    [Header("Auto movement")]
    [SerializeField]
    OnTriggerEnter2DEvent autoMovementTrigger;
    [SerializeField]
    float speed = 1f;

    bool lavaIsMoving = false;

    void OnEnable()
    {
        startTrigger.onTriggerEnter2D += StartTriggerEnter;
        autoMovementTrigger.onTriggerEnter2D += AutoTriggerEnter;
    }

    void OnDisable()
    {
        startTrigger.onTriggerEnter2D -= StartTriggerEnter;
        autoMovementTrigger.onTriggerEnter2D -= AutoTriggerEnter;
    }

    void StartTriggerEnter(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        renderer.enabled = true;
        collider.enabled = true;
        startTrigger.enabled = false;

        StartCoroutine(LavaToStartPosition());
    }

    void AutoTriggerEnter(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;
        lavaIsMoving = true;
        rigidbody.WakeUp();
        rigidbody.velocity = Vector2.up * speed;
    }

    IEnumerator LavaToStartPosition()
    {
        while (!lavaIsMoving)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition.position, smoothing);
            if (Vector3.Distance(transform.position, startPosition.position) < 0.1f) yield break; // Stop looping

            yield return null; // Wait for next frame
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        Debug.Log("Player hit by lava. Kill player");
    }

    void FixedUpdate()
    {
        if (!lavaIsMoving) return;

    }
}
