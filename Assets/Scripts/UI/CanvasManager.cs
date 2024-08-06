using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TMP_Text gameOver_txt;
    [SerializeField] TMP_Text retry_txt;
    [SerializeField] TMP_Text score_txt;
    [SerializeField] TMP_Text highScore_txt;
    [Header("Objects")]
    [SerializeField] GameObject inkDisplay;

    float maxInkSize;

    private void Start()
    {
        maxInkSize = inkDisplay.transform.localScale.x;
    }

    private void Update()
    {
        
    }

    public void DisplayGameOver()
    {
        StartCoroutine(OnOffRoutine(gameOver_txt));
    }

    internal void DisplayInk(float currentInk, float maxInk)
    {
        float newScale = currentInk / maxInk;   
        inkDisplay.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void UpdateScore(int newScore)
    {
        // Update score 
        score_txt.text = newScore.ToString();
    }

    public void UpdateHighScore(int highScore)
    {
        highScore_txt.text = highScore.ToString();
    }

    IEnumerator OnOffRoutine(TMP_Text text)
    {
        retry_txt.enabled = true;

        while (true)
        {
            text.enabled = true;
            yield return new WaitForSecondsRealtime(1f);
            text.enabled = false;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
