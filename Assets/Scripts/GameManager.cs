using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameOver {get; private set;}

    [SerializeField] CanvasManager canvasManager;

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
    }

    public void Quit()
    {
        Application.Quit();
    }
}
