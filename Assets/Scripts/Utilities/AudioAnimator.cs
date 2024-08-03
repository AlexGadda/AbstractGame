using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSpectrum))]
public class AudioAnimator : MonoBehaviour
{
    [SerializeField] [Tooltip("Higher values means the maximum volume is higher.")] float volumeRange;
    [SerializeField] float maxScale;

    Vector2 baseScale, newScale;
    AudioSpectrum audioSpectrum;
    float maxLevel, scaleMod;

    private void Start()
    {
        audioSpectrum = GetComponent<AudioSpectrum>();

        baseScale = new Vector2(1f, 1f);
    }

    private void Update()
    {
        maxLevel = Mathf.Max(Mathf.Max(audioSpectrum.MeanLevels));
        scaleMod = maxLevel / volumeRange;
        newScale = baseScale + new Vector2(scaleMod, scaleMod);
        if (newScale.x > maxScale)
            newScale = new Vector2(maxScale, maxScale);
        this.transform.localScale = newScale;
    }
}
