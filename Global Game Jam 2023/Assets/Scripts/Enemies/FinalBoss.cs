using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private GameObject winningUI;

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
    }

    private void ActivateVictoryScreen()
    {
        winningUI.SetActive(true);
    }
}
