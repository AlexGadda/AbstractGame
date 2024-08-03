using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnimator : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float updateStep = 0.1f;
    [SerializeField] int sampleDataLength = 1024;
    
    float currentUpdateTime = 0f;

    public float clipLoudness;




}
