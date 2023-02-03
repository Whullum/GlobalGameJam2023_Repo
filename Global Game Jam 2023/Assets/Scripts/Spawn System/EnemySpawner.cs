using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> enemiesToSpawn = new List<GameObject>(); // List with the enemies to spawn this floor.
    private List<GameObject> enemiesSpawned = new List<GameObject>(); // List containing all the spawneed enemies.

    [Tooltip("Maximum amount of enemies alive on the same floor.")]
    [SerializeField] private int enemyThreshold = 20;
    [Tooltip("Difficulty increase when a new floor is reached. This increases the amount of enemies able to spawn until the maximum threshold is reached.")]
    [Range(1, 10)]
    [SerializeField] private int difficultyIncrease = 2;
    [Tooltip("Starting floor value. More value means more enemies to spawn.")]
    [SerializeField] private int spawnerWallet = 1;
    [Tooltip("Distance at which the enemies will not spawn near the player.")]
    [Range(1, 30)]
    [SerializeField] private float minimumDistanceToPlayer = 1;
    [Tooltip("Enemies to spawn with different cost. This cost is subtracted from EnemyValue.")]
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    private void OnEnable() => LevelGenerator.LevelCreated += GenerateSpawnPoints;

    private void OnDisable() => LevelGenerator.LevelCreated -= GenerateSpawnPoints;

    /// <summary>
    /// Create a different spawn point for each enemy to spawn.
    /// </summary>
    /// <param name="floor">Array containing the level.</param>
    /// <param name="playerSpawn">Player spawn position.</param>
    private void GenerateSpawnPoints(int[,] floor, TileCoord playerSpawn)
    {
        // First, we generate all the enemies.
        GenerateEnemies();

        List<Vector2> spawnPositions = new List<Vector2>();
        int i = enemiesToSpawn.Count;
        int widht = floor.GetLength(0);
        int height = floor.GetLength(1);

        while (i > 0)
        {
            int rndX = Random.Range(0, widht);
            int rndY = Random.Range(0, height);

            float spawnDistance = Mathf.Abs(rndX - playerSpawn.xCoord) + Mathf.Abs(rndY - playerSpawn.yCoord);

            if (spawnDistance > minimumDistanceToPlayer && floor[rndX, rndY] == 0)
            {
                spawnPositions.Add(new Vector2(rndX + 0.5f, rndY + 0.5f)); // We add the tile size to center the position. This will not work properly if we change the tile size from the grid.
                i--;
            }
        }
        SpawnEnemies(spawnPositions);
    }

    /// <summary>
    /// Creates all the enemies substracting the cost from the wallet until the spawner can't "buy" new enemies.
    /// </summary>
    private void GenerateEnemies()
    {
        List<GameObject> choosedEnemies = new List<GameObject>();
        int spawnedEnemies = 0;
        spawnerWallet = DungeonManager.CurrentFloor * difficultyIncrease;

        while (spawnerWallet > 0 || spawnedEnemies >= enemyThreshold)
        {
            int index = Random.Range(0, enemies.Count);
            Enemy rndEnemy = enemies[index];

            if (spawnerWallet - rndEnemy.SpawnCost >= 0)
            {
                choosedEnemies.Add(rndEnemy.EnemyPrefab);
                spawnerWallet -= rndEnemy.SpawnCost;
                spawnedEnemies++;
            }
            else if (spawnerWallet <= 0)
                break;
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = choosedEnemies;
    }

    /// <summary>
    /// Clears previous spawned enemies and instantiates the new ones.
    /// </summary>
    /// <param name="spawnPositions">Positions to spawn all the enemies.</param>
    private void SpawnEnemies(List<Vector2> spawnPositions)
    {
        int i = 0;

        foreach (GameObject enemy in enemiesSpawned)
            Destroy(enemy);

        enemiesSpawned.Clear();

        foreach (Vector2 spawn in spawnPositions)
        {
            enemiesSpawned.Add(Instantiate(enemiesToSpawn[i], spawn, Quaternion.identity));
            enemiesSpawned[i].transform.parent = transform;

            i++;
        }
    }
}
