using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLavaMovement : MonoBehaviour
{
    [SerializeField] 
    Transform startPosition, attackPosition, endPosition;
    [SerializeField] 
    float smoothing = 0.1f, chargeAttackTime = 0.5f, attackTime = 0.2f;

    void OnEnable()
    {
        StartCoroutine(MoveToStart());
    }

    IEnumerator MoveToStart()
    {
        while (Vector3.Distance(transform.position, startPosition.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition.position, smoothing);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(chargeAttackTime);

        while (Vector3.Distance(transform.position, attackPosition.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, attackPosition.position, smoothing);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(attackTime);

        while (Vector3.Distance(transform.position, endPosition.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition.position, smoothing);
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameManager.instance.KillPlayer();
    }
}
