using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<GameObject> enemiesSpawned = new List<GameObject>();

    [Tooltip("Maximum amount of enemies alive on the same floor.")]
    [SerializeField] private int enemyThreshold = 20;
    [Tooltip("Difficulty increase when a new floor is reached. This increases the amount of enemies able to spawn until the maximum threshold is reached.")]
    [Range(1, 10)]
    [SerializeField] private int difficultyIncrease = 2;
    [Tooltip("Starting floor value. More value means more enemies to sapwn.")]
    [SerializeField] private int enemyValue = 1;
    [Tooltip("Distance at which the enemies will not spawn near the player.")]
    [Range(1,30)]
    [SerializeField] private float minimumDistanceToPlayer = 1;
    [Tooltip("Enemies to spawn with different cost. This cost is subtracted from EnemyValue.")]
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    private void OnEnable()
    {
        LevelGenerator.LevelCreated += GenerateSpawnPoints;
    }

    private void OnDisable()
    {
        LevelGenerator.LevelCreated -= GenerateSpawnPoints;
    }

    private void GenerateSpawnPoints(int[,] floor, TileCoord playerSpawn)
    {
        GenerateEnemies();

        List<Vector2> spawnPositions = new List<Vector2>();
        int i = enemiesToSpawn.Count;
        int widht = floor.GetLength(0);
        int height = floor.GetLength(1);

        while(i > 0) 
        {
            int rndX = Random.Range(0,widht);
            int rndY = Random.Range(0,height);

            float spawnDistance = Mathf.Abs(rndX - playerSpawn.xCoord) + Mathf.Abs(rndY - playerSpawn.yCoord);

            if (spawnDistance > minimumDistanceToPlayer && floor[rndX,rndY] == 0)
            {
                spawnPositions.Add(new Vector2(rndX, rndY));
                i--;
            }
        }
        SpawnEnemies(spawnPositions);
    }

    private void GenerateEnemies()
    {
        List<GameObject> choosedEnemies = new List<GameObject>();
        int spawnedEnemies = 0;
        enemyValue = DungeonManager.CurrentFloor * difficultyIncrease;
        
        while (enemyValue > 0 || spawnedEnemies >= enemyThreshold)
        {
            int index = Random.Range(0, enemies.Count);
            Enemy rndEnemy = enemies[index];

            if (enemyValue - rndEnemy.SpawnCost >= 0)
            {
                choosedEnemies.Add(rndEnemy.EnemyPrefab);
                enemyValue -= rndEnemy.SpawnCost;
                spawnedEnemies++;
            }
            else if (enemyValue <= 0)
                break;
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = choosedEnemies;
    }

    private void SpawnEnemies(List<Vector2> spawnPositions)
    {
        int i = 0;

        foreach (GameObject enemy in enemiesSpawned)
            Destroy(enemy);

        enemiesSpawned.Clear();

        foreach(Vector2 spawn in spawnPositions)
        {
            enemiesSpawned.Add(Instantiate(enemiesToSpawn[i], spawn, Quaternion.identity));
            i++;
        }
    }
}
