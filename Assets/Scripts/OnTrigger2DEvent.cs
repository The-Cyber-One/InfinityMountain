using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger2DEvent : MonoBehaviour
{
    public Action<Collider2D> onTriggerEnter2D;
    public Action<Collider2D> onTriggerStay2D;

    void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter2D?.Invoke(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        onTriggerStay2D?.Invoke(collision);
    }
}
