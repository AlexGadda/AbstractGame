using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    [Header("Audio")]
    [SerializeField] AudioClip gameOverSfx;
    [SerializeField] float gameOverVolume;
    [SerializeField] AudioMixerGroup mixerGroup;
    [SerializeField] AudioMixer audioMixer;

    public static GameManager Instance { get; private set; }
    public bool IsGameOver {get; private set;}
    public int score { get; private set; }

    ProjectileSpawner spawner;
    int highScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    void Start()
    {
        IsGameOver = false;
        spawner = GetComponent<ProjectileSpawner>();

        // Audio Setup 
        audioMixer.SetFloat(PlayerPrefsStrings.MusicVolume, PlayerPrefs.GetFloat(PlayerPrefsStrings.MusicVolume));

        // Start spawning 
        spawner.StartSpawning();

        // Get High Score
        highScore = PlayerPrefs.GetInt(PlayerPrefsStrings.HighScore, 0);

        // Set score and high-score
        canvasManager.UpdateScore(0);
        canvasManager.UpdateHighScore(highScore);

        // Start scoring 
        score = 0;
        //score = 200; // DEBUG
        StartCoroutine(Scoring());
    }

    [ContextMenu("GameOver()")]
    public void GameOver()
    {
        Debug.Log("Game Over!");

        IsGameOver = true;
        Time.timeScale = 0f;
        spawner.StopSpawning();

        // Audio
        AudioManager.Instance.PlayAudioClip(gameOverSfx, mixerGroup, gameOverVolume);
        audioMixer.SetFloat(PlayerPrefsStrings.MusicVolume, -15f);

        // Check high-score
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(PlayerPrefsStrings.HighScore, highScore);
        }

        canvasManager.UpdateHighScore(highScore);
        canvasManager.DisplayGameOver();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Scoring()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (IsGameOver)
                break;

            score += 1;
            canvasManager.UpdateScore(score);
        }
    }
}
