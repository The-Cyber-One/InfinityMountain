using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class VulnerableState : State
{
    [SerializeField]
    Transform vulnerablePosition, headPosition;
    [SerializeField]
    float smoothing = 0.1f, vulnerableTime = 3f;
    [SerializeField]
    Collider2D collider;

    bool movingBack;

    public override void Enter()
    {
        movingBack = false;
        StartCoroutine(MoveToStart());
        collider.enabled = true;
    }

    public override void Exit() { }

    IEnumerator MoveToStart()
    {
        while (Vector3.Distance(transform.position, vulnerablePosition.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, headPosition.position, smoothing);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(vulnerableTime);

        if (!movingBack) StartCoroutine(MoveBack());
    }

    public void HookHit()
    {
        if (!movingBack) StartCoroutine(MoveBack());

        GetContext<BossManager>().health--;
    }

    IEnumerator MoveBack()
    {
        movingBack = true;
        collider.enabled = false;

        while (Vector3.Distance(transform.position, headPosition.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, headPosition.position, smoothing);
            yield return null;
        }

        GetContext<BossManager>().NextState();
    }
}
