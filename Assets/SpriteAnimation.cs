using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField]
    int framesPerSecond = 24;
    [SerializeField]
    SpriteRenderer renderer;
    [SerializeField]
    Sprite[] sprites;

    int currentSprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (sprites.Length == 0) return;
        float secondsPerFrame = 1f / framesPerSecond;
        InvokeRepeating(nameof(Animate), 0, secondsPerFrame);
    }

    void Animate()
    {
        currentSprite++;
        if (currentSprite >= sprites.Length) currentSprite = 0;

        renderer.sprite = sprites[currentSprite];
    }
}
