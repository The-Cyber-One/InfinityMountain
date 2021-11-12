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
        Transform[] transforms = new Transform[] { BottomLava, RightLava, LeftLava };
        while (transforms.Length > 0) lavas.Enqueue(transforms[Random.Range(0, transforms.Length)]);
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
