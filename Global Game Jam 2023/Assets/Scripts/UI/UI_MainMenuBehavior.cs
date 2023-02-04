using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Houses the main menu functionality
/// </summary>
public class UI_MainMenuBehavior : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject mainMenuCanvas;

    [Header("Buttons")]
    public Button startButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button exitButton;

    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundEffectVolumeSlider;

    [Header("Wwise Sounds")]
    public AK.Wwise.Event uiSelection;
    public AK.Wwise.Event uiConfirm;
    public AK.Wwise.Event uiSliderChange;

    [Header("Wwise RTPC")]
    public AK.Wwise.RTPC masterRTPC;
    public AK.Wwise.RTPC musicRTPC;
    public AK.Wwise.RTPC sfxRTPC;

    public AK.Wwise.Event hubMusic;
    public AK.Wwise.Event stopHubMusic;

    private Stack<GameObject> canvasStack;

    private void Start()
    {
        hubMusic.Post(gameObject);

        // Set up the canvas stack for functionality
        canvasStack = new Stack<GameObject>();
        canvasStack.Push(mainMenuCanvas);

        SetVolumeSliderValues();
    }

    public void StopMusic()
    {
        stopHubMusic.Post(gameObject);
    }

    /// <summary>
    /// Transitions to a different UI canvas by pushing it to the top of canvasStack
    /// </summary>
    /// <param name="newCanvas"> The canvas to open </param>
    public void OpenCanvas(GameObject newCanvas)
    {
        canvasStack.Peek().SetActive(false);
        canvasStack.Push(newCanvas);
        canvasStack.Peek().SetActive(true);
    }

    /// <summary>
    /// Goes to the previous canvas by popping current stack
    /// </summary>
    public void PreviousCanvas()
    {
        canvasStack.Peek().SetActive(false);
        canvasStack.Pop();
        canvasStack.Peek().SetActive(true);
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    public void VolumeSlidersBehavior(string value)
    {
        switch (value)
        {
            case "master":
                masterRTPC.SetGlobalValue(masterVolumeSlider.value);
                break;
            case "music":
                musicRTPC.SetGlobalValue(musicVolumeSlider.value);
                break;
            case "sfx":
                sfxRTPC.SetGlobalValue(soundEffectVolumeSlider.value);
                break;
        }
    }

    public void SetVolumeSliderValues()
    {
        masterVolumeSlider.value = masterRTPC.GetValue(gameObject);
        musicVolumeSlider.value = musicRTPC.GetValue(gameObject);
        soundEffectVolumeSlider.value = sfxRTPC.GetValue(gameObject);
    }

    public void PlayHoverSound()
    {
        uiSelection.Post(gameObject);
    }
}
