using UnityEngine;

public class LevelExit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // Load next level
        }
    }
}
