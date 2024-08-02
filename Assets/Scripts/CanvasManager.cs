using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TMP_Text gameOver_txt;
    [SerializeField] GameObject inkDisplay;

    float maxInkSize;

    private void Start()
    {
        maxInkSize = inkDisplay.transform.localScale.x;
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
