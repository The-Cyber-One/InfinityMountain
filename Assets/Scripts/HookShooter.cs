using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooter : MonoBehaviour
{
    [SerializeField]
    GameObject hookPrefab;
    [SerializeField]
    float hookSpeed = 2f;
    [SerializeField]
    float maxShootDistance = 10f;
    [SerializeField]
    LayerMask shootableLayers;

    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GameEvents.PlayerShootsHook += HookShoot;
    }

    private void HookShoot(Vector2 shootDirection)
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;
        GameObject hook = Instantiate(hookPrefab, transform);

        StartCoroutine(HookMovement(hook, shootDirection));
    }

    IEnumerator HookMovement(GameObject hook, Vector2 shootDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(hook.transform.position, shootDirection, maxShootDistance, shootableLayers);
        if (hit.collider)
        {
            Debug.Log($"Hook hitted {hit.collider} at a distance of {hit.distance}. current distance is {Vector3.Distance(hook.transform.position + hook.transform.localScale, hit.collider.ClosestPoint(hook.transform.position))}");
            while (Vector3.Distance(hook.transform.position + hook.transform.localScale, hit.collider.ClosestPoint(hook.transform.position)) > hit.distance)
            {
                hook.transform.localScale += (Vector3)shootDirection * hookSpeed * Time.deltaTime;

                yield return null;
            }
        }

        Destroy(hook);
    }
}
