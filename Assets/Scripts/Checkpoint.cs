using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber = 0;

    [SerializeField]
    OnTrigger2DEvent checkpointTrigger;
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

        SetActive();
        CheckpointManager.instance.SetCheckpoint(checkpointNumber);
    }

    public void SetActive()
    {
        light.enabled = true;
        animator.enabled = true;
    }
}
