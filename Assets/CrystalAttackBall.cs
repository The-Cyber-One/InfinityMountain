using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAttackBall : MonoBehaviour
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    GameObject[] crystals;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.KillPlayer();
        }
        else if (collision.gameObject.layer == groundLayer)
        {
            Instantiate(crystals[Random.Range(0, crystals.Length)]);
            Destroy(gameObject);
        }
    }
}
