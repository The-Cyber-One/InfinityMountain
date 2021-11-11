using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    Vector2 rawCheckpointPosition;
    public int ActiveCheckpoint { get; private set; }

    Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public static void FindCheckpoints()
    {
        foreach (Checkpoint checkpoint in FindObjectsOfType(typeof(Checkpoint)))
        {
            instance.checkpoints.Add(checkpoint.checkpointNumber, checkpoint);
        }
    }

    public void ResetCheckpoint()
    {
        ActiveCheckpoint = 0;
        rawCheckpointPosition = Vector2.zero;
    }

    public void MoveToCheckpoint()
    {
        Sprite sprite = PlayerMovement.instance.GetComponent<SpriteRenderer>().sprite;
        PlayerMovement.instance.transform.position = rawCheckpointPosition + (sprite.bounds.center.y + sprite.bounds.size.y / 2) * Vector2.up;
    }

    public void SetCheckpoint(int checkpointNumber)
    {
        if (checkpointNumber <= ActiveCheckpoint) return;

        if (checkpoints.Count == 0) FindCheckpoints();

        ActiveCheckpoint = checkpointNumber;
        Sprite sprite = checkpoints[checkpointNumber].GetComponent<SpriteRenderer>().sprite;
        rawCheckpointPosition = (Vector2)checkpoints[checkpointNumber].transform.position - (sprite.bounds.center.y + sprite.bounds.size.y / 2) * Vector2.up;

        checkpoints[checkpointNumber].SetActive();

        GameManager.instance.ResetDeaths();
    }
}
