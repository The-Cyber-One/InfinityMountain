using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooter : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    GameObject hookPrefab;
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float maxShootDistance = 10f;
    [SerializeField]
    LayerMask shootableLayers;

    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    float rechargeTime = 1.5f;
    bool canShoot = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        GameEvents.InputedShootHook += HookShoot;
    }

    void HookShoot(float shootDirection)
    {
        if (canShoot)
        {
            // TODO: only start counting when player is of wall because player can climb easily higher with hook now
            StartCoroutine(HookTimer());
            StartCoroutine(HookMovement(shootDirection));
        }
    }

    IEnumerator HookTimer()
    {
        canShoot = false;
        yield return new WaitForSecondsRealtime(rechargeTime);
        canShoot = true;
    }

    IEnumerator HookMovement(float shootDirection)
    {
        // Detect if hook can hit surface
        Vector3 offset = Vector3.right * (sprite.bounds.size.x / 2 * shootDirection);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, shootDirection * Vector2.right, maxShootDistance, shootableLayers);

        if (hit.collider)
        {
            playerInput.HookShot = true;

            // Instantiate hook prefab
            GameObject hook = Instantiate(hookPrefab, transform.position + offset, Quaternion.identity, transform);

            // Stop player movement
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;

            // Shoot hook
            float neededMagnitude = (hit.point - (Vector2)hook.transform.position).magnitude;
            int stepAmount = Mathf.FloorToInt(neededMagnitude / (speed * Time.fixedDeltaTime));
            float stepSize = speed * Time.fixedDeltaTime * shootDirection;

            int step = 0;
            while (step++ < stepAmount)
            {
                hook.transform.localScale += stepSize * Vector3.right;

                yield return new WaitForFixedUpdate();
            }

            // Set hook size incase of overshooting
            hook.transform.localScale = new Vector2(neededMagnitude * shootDirection, 0.3f);

            step = 0;

            while (step++ < stepAmount)
            {
                hook.transform.localScale -= stepSize * Vector3.right;
                transform.position += stepSize * Vector3.right;

                yield return new WaitForFixedUpdate();
            }

            // Set position incase of overshooting
            transform.position = hit.point - (Vector2)offset;

            playerInput.OnWall = true;
            playerInput.wallSide = shootDirection;

            Destroy(hook);
        }
    }
}
