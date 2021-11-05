using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += SceneChanged;
    }

    bool updateCheckpoint;

    public static void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        instance.updateCheckpoint = true;
    }

    void SceneChanged(Scene previous, Scene next)
    {
        if (updateCheckpoint)
        {
            CheckpointManager.MoveToCheckpoint(PlayerMovement.instance.transform, PlayerMovement.instance.spriteRenderer.sprite);
            updateCheckpoint = false;
        }
    }
}
