using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCrystal : MonoBehaviour
{
    public bool destroyOnPickup = false;

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

            if (destroyOnPickup) Destroy(gameObject);
        }
    }
}
