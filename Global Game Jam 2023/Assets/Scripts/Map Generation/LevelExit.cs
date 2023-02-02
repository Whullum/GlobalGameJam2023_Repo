using System;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public static Action LevelFinished;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // Launch event specifying the plaeyr has reached the end of the level
            LevelFinished?.Invoke();
        }
    }
}
