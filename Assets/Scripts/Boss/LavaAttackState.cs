using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LavaAttackState : State
{
    [SerializeField]
    Transform BottomLava, RightLava, LeftLava;
    Queue<Transform> lavas = new Queue<Transform>();

    public override void Enter()
    {
        List<Transform> transforms = new List<Transform>() { BottomLava, RightLava, LeftLava };
        while (transforms.Count > 0)
        {
            int index = Random.Range(0, transforms.Count);
            lavas.Enqueue(transforms[index]);
            transforms.RemoveAt(index);
        }

        StartCoroutine(LavaMovement());
    }

    public override void Exit() { }

    IEnumerator LavaMovement()
    {
        while (lavas.Count > 0)
        {
            Transform lava = lavas.Dequeue();
            lava.gameObject.SetActive(true);
            yield return new WaitWhile(() => lava.gameObject.activeInHierarchy);
        }

        GetContext<BossManager>().NextState();
    }
}
