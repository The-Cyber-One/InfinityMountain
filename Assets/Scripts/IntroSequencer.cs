using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IntroSequencer : MonoBehaviour
{
    [Serializable]
    struct TextCard {
        public GameObject text;
        public float time;
    }

    [SerializeField]
    string nextSceneName;
    [SerializeField]
    bool useSceneName = false;
    [SerializeField]
    Slider skippingProgress;
    [SerializeField]
    Image background, fill;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    private float skippingTime = 2f, fadeTime = 0.5f;
    [SerializeField]
    TextCard[] textCards;

    private float touchTimer = 0;

    void Start()
    {
        StartCoroutine(ShowIntroText());
    }

    private void FixedUpdate()
    {
        // Update skipping timer
        if (Input.touchCount > 0 || Input.anyKey) touchTimer += Time.fixedDeltaTime;
        else touchTimer = 0;

        // Update slider
        float fadePercentage = touchTimer / fadeTime;
        background.color = new Color(background.color.r, background.color.g, background.color.b, fadePercentage); ;
        fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, fadePercentage);
        text.alpha = fadePercentage;
        skippingProgress.value = touchTimer / (skippingTime + fadeTime);


        if (touchTimer >= (skippingTime + fadeTime)) LoadNextScene();
    }

    IEnumerator ShowIntroText()
    {
        for (int i = 0; i < textCards.Length; i++)
        {
            TextCard textCard = textCards[i];
            textCard.text.SetActive(true);
            if (i != 0) textCards[i - 1].text.SetActive(false);
            yield return new WaitForSecondsRealtime(textCard.time);
        }

        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (useSceneName) SceneManager.LoadScene(nextSceneName);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
