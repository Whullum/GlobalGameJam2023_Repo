using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    public static ResumeGame instance { get; set; }

    public GameObject pauseMenu;
    private bool isOpen;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
        isOpen = false;
    }

    public void OpenPauseMenu()
    {
        if (!isOpen)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        isOpen = !isOpen;
    }
}
