using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleSettings : MonoBehaviour
{
    // REMEMBER: toogle true = false 

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    private void Start()
    {
        SetupAudio();
    }

    void SetupAudio()
    {
        // Get volumes
        float musicVolume = PlayerPrefs.GetFloat(PlayerPrefsStrings.MusicVolume);
        float sfxVolume = PlayerPrefs.GetFloat(PlayerPrefsStrings.SfxVolume);

        // Set volumes
        audioMixer.SetFloat(PlayerPrefsStrings.MusicVolume, musicVolume);
        audioMixer.SetFloat(PlayerPrefsStrings.SfxVolume, sfxVolume);

        // Set toggles state
        musicToggle.isOn = musicVolume < 0 ? true : false;
        sfxToggle.isOn = sfxVolume < 0 ? true : false;
    }

    public void OnMusicChange(bool value)
    {
        if(value)
        {
            // Off
            audioMixer.SetFloat(PlayerPrefsStrings.MusicVolume, -80f);
            PlayerPrefs.SetFloat(PlayerPrefsStrings.MusicVolume, -80f);
        }
        else
        {
            // On
            audioMixer.SetFloat(PlayerPrefsStrings.MusicVolume, 0f);
            PlayerPrefs.SetFloat(PlayerPrefsStrings.MusicVolume, 0f);
        }
    }

    public void OnSfxChange(bool value)
    {
        if (value)
        {
            // Off
            audioMixer.SetFloat(PlayerPrefsStrings.SfxVolume, -80f);
            PlayerPrefs.SetFloat(PlayerPrefsStrings.SfxVolume, -80f);
        }
        else
        {
            // On
            audioMixer.SetFloat(PlayerPrefsStrings.SfxVolume, 0f);
            PlayerPrefs.SetFloat(PlayerPrefsStrings.SfxVolume, 0f);
        }
    }
}
