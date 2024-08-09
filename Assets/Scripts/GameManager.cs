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
    public bool IsGameOver { get; private set; }
    public bool IsPause {get; private set;}
    public int Score { get; private set; }

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
        IsPause = false;

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
        Score = 0;
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
        if(Score > highScore)
        {
            highScore = Score;
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

    public void ExitPressed()
    {
        if(IsGameOver)
            Application.Quit();
        else if(IsPause)
        {
            IsPause = false;
            canvasManager.TogglePause();
            Time.timeScale = 1f;
        }
        else
        {
            IsPause = true;
            canvasManager.TogglePause();
            Time.timeScale = 0f;
        }
    }

    IEnumerator Scoring()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);

            if (IsGameOver)
                break;

            Score += 1;
            canvasManager.UpdateScore(Score);
        }
    }
}
