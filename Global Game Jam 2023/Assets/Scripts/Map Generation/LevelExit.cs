using System;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public static Action LevelFinished;
    public static Action GameOver;

    [SerializeField] private GameObject levelFinishCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    private void Awake()
    {
        levelFinishCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        GameOver += EnableGameOverUI;
    }

    private void OnDisable()
    {
        GameOver -= EnableGameOverUI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MusicManager.instance.victoryState.SetValue();
            levelFinishCanvas.SetActive(true);

            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Tell the game state that this level is finished.
    /// </summary>
    public void LoadNextlevel()
    {
        // Launch event specifying the player has reached the end of the level
        LevelFinished?.Invoke();
        MusicManager.instance.normalState.SetValue();

        levelFinishCanvas.SetActive(false);

        Time.timeScale = 1;
    }

    public void EnableGameOverUI()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackToTown()
    {
        gameOverCanvas.SetActive(false);
        LevelLoader.RestartGame();
    }
}
