using System;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public static Action LevelFinished;

    [SerializeField] private GameObject levelFinishCanvas;

    private void Awake()
    {
        levelFinishCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MusicManager.instance.victoryState.SetValue();
            levelFinishCanvas.SetActive(true); 
        }
    }

    /// <summary>
    /// Tell the game state that this level is finished.
    /// </summary>
    public void LoadNextlevel()
    {
        levelFinishCanvas.SetActive(false);
        // Launch event specifying the player has reached the end of the level
        LevelFinished?.Invoke();
    }
}
