using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public AK.Wwise.Event stopHubMusic;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public static void RestartGame() => SceneManager.LoadScene("UIMockups");
}
