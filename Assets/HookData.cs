using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookData : MonoBehaviour
{
    public int AvailableHooks { get; private set; }

    [SerializeField]
    int startingHooks = 0;
    [SerializeField]
    int maxHooks = 3;

    void Start()
    {
        AvailableHooks = startingHooks;
    }

    public bool CanShootHook()
    {
        return AvailableHooks > 0;
    }

    public void UseHook()
    {
        if (CanShootHook()) AvailableHooks--;
    }

    public bool AddHook(int amount)
    {
        if (AvailableHooks == maxHooks) return false;
        AvailableHooks += amount;
        if (AvailableHooks > maxHooks) AvailableHooks = maxHooks;
        return true;
    }
}
