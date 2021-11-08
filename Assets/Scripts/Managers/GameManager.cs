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
    public int CheckpointDeathCounter { get; private set; }

    bool updateCheckpoint;

    public static void ResetDeaths()
    {
        instance.CheckpointDeathCounter = 0;
    }

    public static void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        instance.updateCheckpoint = true;
        instance.CheckpointDeathCounter++;
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
