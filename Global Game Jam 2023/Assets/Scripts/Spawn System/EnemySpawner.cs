using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    [SerializeField] private int enemyThreshold = 20;
    [Range(1, 10)]
    [SerializeField] private int difficultyIncrease = 2;
    [SerializeField] private int enemyValue = 1;
    [Range(1,30)]
    [SerializeField] private float minimumDistanceToPlayer = 1;
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
            Debug.Log(spawnDistance);
            if (spawnDistance > minimumDistanceToPlayer && floor[rndX,rndY] == 0)
            {
                spawnPositions.Add(new Vector2(rndX, rndY));
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector2(rndX, rndY);
                i--;
            }
        }
        SpawnEnemies(spawnPositions);
    }

    private void GenerateEnemies()
    {
        int spawnedEnemies = 0;
        enemyValue = DungeonManager.CurrentFloor * difficultyIncrease;
        
        while (enemyValue > 0 || spawnedEnemies >= enemyThreshold)
        {
            int index = Random.Range(0, enemies.Count);
            Enemy rndEnemy = enemies[index];

            if (enemyValue - rndEnemy.SpawnCost >= 0)
            {
                enemiesToSpawn.Add(rndEnemy.EnemyPrefab);
                enemyValue -= rndEnemy.SpawnCost;
                spawnedEnemies++;
            }
        }
    }

    private void SpawnEnemies(List<Vector2> spawnPositions)
    {
        int i = 0;
        foreach(Vector2 spawn in spawnPositions)
        {
            Instantiate(enemiesToSpawn[i], spawn, Quaternion.identity);
            i++;
        }
    }
}
