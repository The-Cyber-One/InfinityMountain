using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    int checkpointNumber = 0;
    [SerializeField]
    OnTriggerEnter2DEvent checkpointTrigger;
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Light2D light;
    [SerializeField]
    Animator animator;
    [SerializeField]
    [TagSelector]
    string playerTag;

    void OnEnable()
    {
        checkpointTrigger.onTriggerEnter2D += CheckPointTrigger;
    }

    void OnDisable()
    {
        checkpointTrigger.onTriggerEnter2D -= CheckPointTrigger;   
    }

    void CheckPointTrigger(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        light.enabled = true;
        animator.enabled = true;
        CheckpointManager.SetCheckpoint(transform.position, renderer.sprite, checkpointNumber);
    }
}
