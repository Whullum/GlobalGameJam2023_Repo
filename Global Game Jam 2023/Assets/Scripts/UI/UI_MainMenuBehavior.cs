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

    private Stack<GameObject> canvasStack;

    private void Start()
    {
        // Set up the canvas stack for functionality
        canvasStack = new Stack<GameObject>();
        canvasStack.Push(mainMenuCanvas);

        // TODO: Initialize volume slider values based on saved Wwise bus values
    }

    public void StartGame()
    {
        // Transition to hub menu scene
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

    public void VolumeSlidersBehavior()
    {
        // TODO: Volume slider functionality
        // Get slider name
        // Use slider name to get corresponding wwise bus
        // Change the value of the wwise bus to the value of the slider
    }
}
