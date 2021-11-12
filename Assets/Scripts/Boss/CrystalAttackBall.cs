using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAttackBall : MonoBehaviour
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    GameObject[] crystals;
    [SerializeField]
    float destroyTime = 2f;

    void Start()
    {
        Invoke(nameof(DestroyCrystal), destroyTime);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.KillPlayer();
            CancelInvoke(nameof(DestroyCrystal));
        }
    }

    void DestroyCrystal()
    {
        GameObject instance = Instantiate(crystals[Random.Range(0, crystals.Length)], transform.position, Quaternion.identity, null);
        instance.GetComponent<HookCrystal>().destroyOnPickup = true;
        Destroy(gameObject);
    }
}
