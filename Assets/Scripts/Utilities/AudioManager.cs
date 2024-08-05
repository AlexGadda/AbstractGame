using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public AudioManager Instance { get; private set; } // Singleton

        ObjectPool<AudioSource> pool;

        private void Awake()
        {
            // Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            pool = new ObjectPool<AudioSource>(CreateNewPoolEntry);
        }

        AudioSource CreateNewPoolEntry()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            return source;
        }

        public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixerGroup)
        {
            if (clip == null)
                return;

            AudioSource audioSource = pool.Get();
            audioSource.clip = clip;
            if (mixerGroup != null)
                audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
            StartCoroutine(DestroyAudioSourceOnFinish(audioSource));
        }

        IEnumerator DestroyAudioSourceOnFinish(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            pool.Release(source);
        }
    }
}
