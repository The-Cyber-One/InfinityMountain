using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequencer : MonoBehaviour
{
    [Serializable]
    struct TextCard {
        public GameObject text;
        public float time;
    }

    [SerializeField]
    TextCard[] textCards;

    void Start()
    {
        StartCoroutine(ShowIntroText());
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
