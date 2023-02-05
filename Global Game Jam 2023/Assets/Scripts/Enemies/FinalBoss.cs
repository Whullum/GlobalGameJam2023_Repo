using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private GameObject winningUI;
    public AK.Wwise.Event winningMusic;
    public AK.Wwise.Event stopWinningMusic;
    public AK.Wwise.State completeState;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        GetComponent<Animator>().SetTrigger("BossDeath");
        Invoke("ActivateVictoryScreen", 3f);

        MusicManager.instance.stopDungeonMusic.Post(MusicManager.instance.gameObject);
    }

    private void ActivateVictoryScreen()
    {
        stopWinningMusic.Post(gameObject);

        DungeonManager.GameComplete = true;
        winningUI.SetActive(true);

        completeState.SetValue();
        winningMusic.Post(gameObject);

        Time.timeScale = 0;
    }
}
