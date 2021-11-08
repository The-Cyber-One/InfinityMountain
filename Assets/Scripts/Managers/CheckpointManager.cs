using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    static Vector2 RawCheckpointPosition { get; set; }
    static int activeCheckpoint;

    public static void ResetCheckpoint()
    {
        activeCheckpoint = 0;
        RawCheckpointPosition = Vector2.zero;
    }

    public static void MoveToCheckpoint(Transform transform, Sprite sprite)
    {
        transform.position = RawCheckpointPosition + (sprite.bounds.center.y + sprite.bounds.size.y / 2) * Vector2.up;
    }

    public static void SetCheckpoint(Vector2 position, Sprite sprite, int checkpointNumber)
    {
        if (checkpointNumber <= activeCheckpoint) return;

        activeCheckpoint = checkpointNumber;
        RawCheckpointPosition = position - (sprite.bounds.center.y + sprite.bounds.size.y / 2) * Vector2.up;

        GameManager.ResetDeaths();
    }
}
