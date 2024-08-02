using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;

    public static GameManager Instance { get; private set; }
    public bool IsGameOver {get; private set;}
    public int score { get; private set; }

    ProjectileSpawner spawner;

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

        // Start spawning 
        spawner.StartSpawning();

        // Start scoring 
        score = 0;
        StartCoroutine(Scoring());
    }

    [ContextMenu("GameOver()")]
    public void GameOver()
    {
        Debug.Log("Game Over!");
        IsGameOver = true;
        spawner.StopSpawning();
        canvasManager.DisplayGameOver();
        Time.timeScale = 0f;
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
        while(!IsGameOver)
        {
            yield return new WaitForSecondsRealtime(1f);
            score += 1;
            canvasManager.UpdateScore(score);
        }
    }
}
