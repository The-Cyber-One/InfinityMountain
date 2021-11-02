using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCrystal : MonoBehaviour
{
    [SerializeField]
    int hookAmount = 1;

    HookData hookData;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hookData == null) hookData = collision.gameObject.GetComponent<HookData>();
            if (hookData.AvailableHooks < hookAmount)
            hookData.AddHook(hookAmount - hookData.AvailableHooks);
            Debug.Log(hookData.AvailableHooks);
        }
    }
}
