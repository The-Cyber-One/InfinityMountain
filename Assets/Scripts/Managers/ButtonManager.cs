using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void LoadLevelFromCheckpoint(ButtonData data)
    {
        GameManager.instance.LoadLevelAtCheckpoint(data.level, data.checkpoint);
    }
}
