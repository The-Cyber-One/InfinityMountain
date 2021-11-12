using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CrystalAttackState : State
{
    [SerializeField]
    Collider2D collider;
    [SerializeField]
    GameObject crystalBall;
    [SerializeField]
    [Min(1)]
    int minBalls, maxBalls;
    [SerializeField]
    float timeBetweenSpawns = 0.3f;

    public override void Enter()
    {
        StartCoroutine(Spawning());
    }

    public override void Exit() { }

    IEnumerator Spawning()
    {
        int count = Random.Range(minBalls, maxBalls + 1);
        while (count > 0)
        {
            count--;
            Instantiate(crystalBall, new Vector2(Random.Range(collider.bounds.min.x, collider.bounds.max.x), Random.Range(collider.bounds.min.y, collider.bounds.max.y)), Quaternion.identity, null);
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
        }

        GetContext<BossManager>().NextState();
    }
}
