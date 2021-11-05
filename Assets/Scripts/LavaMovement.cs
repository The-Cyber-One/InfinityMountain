using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    [SerializeField]
    OnTriggerEnter2DEvent startTrigger;
    [SerializeField]
    Transform startPosition;
    [SerializeField]
    float smoothing = 0.1f;
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Collider2D collider;

    bool lavaIsMoving = false;

    void OnEnable()
    {
        startTrigger.onTriggerEnter2D += StartTriggerEnter;
    }

    void OnDisable()
    {
        startTrigger.onTriggerEnter2D -= StartTriggerEnter;
    }

    void StartTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lavaIsMoving = true;
            renderer.enabled = true;
            collider.enabled = true;
        }
    }

    void Update()
    {
        if (!lavaIsMoving) return;

        transform.position = Vector3.Lerp(transform.position, startPosition.position, smoothing);
    }
}
