using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TMP_Text gameOver_txt;
    [SerializeField] TMP_Text score_txt;
    [SerializeField] TMP_Text record_txt;
    [Header("Objects")]
    [SerializeField] GameObject inkDisplay;

    float maxInkSize;

    private void Start()
    {
        maxInkSize = inkDisplay.transform.localScale.x;
        score_txt.text = "0";
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

    IEnumerator OnOffRoutine(TMP_Text text)
    {
        while (true)
        {
            text.enabled = true;
            yield return new WaitForSecondsRealtime(1f);
            text.enabled = false;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
