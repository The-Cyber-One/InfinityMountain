using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

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
    AudioSource audioSource;
    [SerializeField]
    AudioClip audioclip;
    [SerializeField]
    GameObject textObject;
    [SerializeField]
    [TagSelector]
    string playerTag;
    [SerializeField]
    bool finalCheckpoint = false;
    [SerializeField]
    [Tooltip("Only active if finalCheckpoint is true")]
    bool loadNextScene = false;
    [SerializeField]
    [Tooltip("Only active if finalCheckpoint is true and loadNextLevel is false")]
    string nextSceneToLoad;

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
        if (textObject != null) textObject.SetActive(true);
        audioSource.Play();
        StartCoroutine(WaitForAudio());
        if (finalCheckpoint) StartCoroutine(LevelCleared());
    }

    IEnumerator WaitForAudio()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.clip = audioclip;
        audioSource.loop = true;
        audioSource.Play();
    }

    IEnumerator LevelCleared()
    {
        PlayerMovement.instance.playerInput.MovePlayerToPosition(transform.position, 0.5f);
        yield return new WaitUntil(() => !PlayerMovement.instance.playerInput.BlockInput);

        if (loadNextScene) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadScene(nextSceneToLoad);
    }
}
