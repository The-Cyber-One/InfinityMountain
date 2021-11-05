using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnter2DEvent : MonoBehaviour
{
    public Action<Collider2D> onTriggerEnter2D;

    void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter2D?.Invoke(collision);
    }
}
