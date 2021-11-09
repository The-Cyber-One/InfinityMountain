using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public float explosionPower;

    [SerializeField]
    [Min(0)]
    float reactivationTime = 1f;
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Collider2D collider;

    public void Explode()
    {
        StartCoroutine(Reactivate());
        renderer.enabled = false;
        collider.enabled = false;
    }

    IEnumerator Reactivate()
    {
        yield return new WaitForSecondsRealtime(reactivationTime);
        collider.enabled = true;
        yield return new WaitWhile(() => collider.IsTouching(PlayerMovement.instance.collider));
        renderer.enabled = true;
    }
}
