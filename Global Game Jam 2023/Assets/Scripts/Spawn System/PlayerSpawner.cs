using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;

    private void OnEnable()
    {
        LevelGenerator.LevelCreated += SpawnPlayer;
    }

    private void OnDisable()
    {
        LevelGenerator.LevelCreated -= SpawnPlayer;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Moves the player to this GameObject position.
    /// </summary>
    public void SpawnPlayer(int[,] level, TileCoord playerSpawn)
    {
        if(player == null) player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
        player.transform.position = transform.position;
    }
}
