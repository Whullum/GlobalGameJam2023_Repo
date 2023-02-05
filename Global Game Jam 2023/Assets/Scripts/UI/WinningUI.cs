using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningUI : MonoBehaviour
{

    public void LoadTown()
    {
        DungeonManager.GameComplete = true;
        SceneManager.LoadScene("UIMockups");
    }
}
