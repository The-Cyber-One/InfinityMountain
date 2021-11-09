using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHookState : State
{
    [SerializeField]
    GameObject hookPrefab;
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float maxShootDistance = 10f;
    [SerializeField]
    LayerMask shootableLayers;
    [SerializeField]
    [TagSelector]
    string[] hookableTags;
    [SerializeField]
    [TagSelector]
    string[] breakableTags;

    Coroutine coroutine;
    GameObject hook;

    public override void Enter()
    {
        coroutine = StartCoroutine(HookMovement(GetContext<PlayerMovement>().playerInput.HookDirection));
    }

    public override void Exit() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enabled) return;
        StopCoroutine(coroutine);
        Destroy(hook);
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
        context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
    }


    IEnumerator HookMovement(float shootDirection)
    {
        // Detect if hook can hit surface
        Vector3 offset = Vector3.right * (GetContext<PlayerMovement>().spriteRenderer.bounds.size.x / 2 * shootDirection);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, shootDirection * Vector2.right, maxShootDistance, shootableLayers);

        if (!hit.collider || hookableTags.Length == 0)
        {
            context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
            yield break;
        }

        bool foundHookable = false;
        for (int i = 0; i < hookableTags.Length; i++)
        {
            if (hit.transform.CompareTag(hookableTags[i]))
            {
                foundHookable = true;
            }
        }

        if (!foundHookable)
        {
            context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
            yield break;
        }


        GetContext<PlayerMovement>().hookData.UseHook();

        // Instantiate hook prefab
        hook = Instantiate(hookPrefab, transform.position + offset, Quaternion.identity, transform);

        // Stop player movement
        //GetContext<PlayerMovement>().rigidbody.velocity = Vector2.zero;
        GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Static;

        // Shoot hook
        float neededMagnitude = (hit.point - (Vector2)hook.transform.position).magnitude;
        int stepAmount = Mathf.FloorToInt(neededMagnitude / (speed * Time.fixedDeltaTime));
        float stepSize = speed * Time.fixedDeltaTime * shootDirection;

        int step = 0;
        while (step++ < stepAmount)
        {
            hook.transform.localScale += stepSize * Vector3.right;

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        // Set hook size incase of overshooting
        hook.transform.localScale = new Vector2(neededMagnitude * shootDirection, 0.3f);

        step = 0;

        while (step++ < stepAmount)
        {
            hook.transform.localScale -= stepSize * Vector3.right;
            transform.position += stepSize * Vector3.right;

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        // Set position incase of overshooting
        transform.position = hit.point - (Vector2)offset;

        GetContext<PlayerMovement>().playerInput.onWall = true;
        GetContext<PlayerMovement>().playerInput.wallSide = shootDirection;

        Destroy(hook);
        if (hit.transform.CompareTag("Enemy"))
        {
            hit.transform.GetComponent<EnemyExplosion>().Explode();
            GetContext<PlayerMovement>().rigidbody.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log(GetContext<PlayerMovement>().rigidbody.velocity);
            GetContext<PlayerMovement>().rigidbody.velocity = new Vector2(shootDirection, 1) * hit.transform.GetComponent<EnemyExplosion>().explosionPower;
            Debug.Log(GetContext<PlayerMovement>().rigidbody.velocity);
            context.TransitionTo((int)PlayerMovement.StateOptions.InAir);
            yield break;
        }

        for (int i = 0; i < breakableTags.Length; i++)
        {
            if (breakableTags[i] == hit.transform.tag)
            {
                Destroy(hit.transform.gameObject);
                //GetContext<PlayerMovement>().TransitionTo((int)PlayerMovement.StateOptions.InAir);
                //yield break;
            }
        }
        GetContext<PlayerMovement>().TransitionTo((int)PlayerMovement.StateOptions.OnWall);

    }
}
