using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;

    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        //SpawnPlayer();
    }

    /// <summary>
    /// Instantiates a player prefab on this GameObject position.
    /// </summary>
    public void SpawnPlayer()
    {
        if(player != null) Destroy(player);

        player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
}
