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
        UpdatePlayerUIValues();
        player.transform.position = transform.position;
    }

    private void UpdatePlayerUIValues()
    {
        UI_PlayerDungeon.Instance.ChangeWewaponText("No weapon");
        UI_PlayerDungeon.Instance.ChangeSeedsCount(SeedWallet.Seeds);
        UI_PlayerDungeon.Instance.ChangeAblityName("No ability");
        UI_PlayerDungeon.Instance.SetAbilityText("No ability");
    }
}
