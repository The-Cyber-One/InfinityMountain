using System;
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

    public int score = 0;

    public int CheckpointDeathCounter { get; private set; }
    public string CurrentLevelTime
    {
        get
        {
            TimeSpan span = DateTime.Now - levelStartTime;
            return $"{span.Hours.ToString("00")}:{span.Minutes.ToString("00")}:{span.Seconds.ToString("00")}";
        }
    }

    DateTime levelStartTime;
    int nextCheckpoint = 0;
    bool updateCheckpoint;

    public void ResetDeaths()
    {
        CheckpointDeathCounter = 0;
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        updateCheckpoint = true;
        nextCheckpoint = CheckpointManager.instance.ActiveCheckpoint;
        CheckpointDeathCounter++;
    }

    public void LoadNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level"))
        {
            int level = int.Parse(sceneName.TrimStart("Level".ToCharArray())) + 1;
            SceneManager.LoadScene($"Level{level}");
        }
    }

    public void LoadLevelAtCheckpoint(int level, int checkpoint)
    {
        SceneManager.LoadScene($"Level{level}");
        nextCheckpoint = checkpoint;
        if (checkpoint > 0)
            updateCheckpoint = true;
    }

    void SceneChanged(Scene previous, Scene next)
    {
        if (updateCheckpoint)
        {
            CheckpointManager.instance.SetCheckpoint(nextCheckpoint);
            CheckpointManager.instance.MoveToCheckpoint();
            updateCheckpoint = false;
        }
        if (next.name == "Level1" || next.name == "Level2")
        {
            levelStartTime = DateTime.Now;
        }
    }
}
