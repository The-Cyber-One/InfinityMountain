using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    [SerializeField]
    float time;
    [SerializeField]
    float speed;
    [SerializeField]
    TextMeshPro text;

    void Start()
    {
        Invoke(nameof(EndText), time);
        text.text = GameManager.instance.CurrentLevelTime;
    }

    void FixedUpdate()
    {
        transform.position += Vector3.up * speed * Time.fixedDeltaTime;
    }

    void EndText()
    {
        Destroy(gameObject);
    }
}
