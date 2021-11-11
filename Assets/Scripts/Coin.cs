using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    AudioSource audio;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.score++;
            StartCoroutine(AudioAwaiter());
        }
    }

    IEnumerator AudioAwaiter()
    {
        audio.Play();
        yield return new WaitWhile(() => audio.isPlaying);
        Destroy(gameObject);
    }
}
