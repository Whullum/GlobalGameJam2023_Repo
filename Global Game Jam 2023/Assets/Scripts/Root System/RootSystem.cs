using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

public class RootSystem : MonoBehaviour
{
    private Transform player;
    private PlayerStat playerHealth;
    private Tilemap rootsTilemap;
    private Coroutine growCoroutine;
    private ParticleSystem rootGrowEffect;
    private int[,] growTiles;
    private int tileType = 0;
    private int width;
    private int height;

    [Range(0.01f, 3)]
    [Tooltip("Speed of the roots spread.")]
    [SerializeField] private float growStep;
    [Tooltip("Time until the roots start spawning once the level is loaded.")]
    [SerializeField] private float startTime = 3;
    [Tooltip("Damage dealt to the player when is inside the roots. This damage value changes depending on the current floor, so for example if player is on the 5th floor, it receives 5 damage points.")]
    [SerializeField] private int rootsDamage = 1;
    [Tooltip("Each tick passes player receives damage.")]
    [Range(0.1f, 5f)]
    [SerializeField] private float damageTick = 0.5f;
    [Tooltip("Tile used to represent the roots.")]
    [SerializeField] private Tile rootTile;

    private void Awake()
    {
        rootsTilemap = GetComponent<Tilemap>();
        rootGrowEffect= GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerStat>();
    }

    private void OnEnable() => LevelGenerator.LevelCreated += StartRootGrowing;

    private void OnDisable() => LevelGenerator.LevelCreated -= StartRootGrowing;

    /// <summary>
    /// Sets the level size, tiles and start tile to start propagating the roots.
    /// </summary>
    /// <param name="growTiles">Array containing the level.</param>
    /// <param name="startTile">Start position for the roots.</param>
    private void StartRootGrowing(int[,] growTiles, TileCoord startTile)
    {
        this.growTiles = growTiles;
        this.width = growTiles.GetLength(0);
        this.height = growTiles.GetLength(1);
        damageTick = DungeonManager.CurrentFloor;

        rootsTilemap.ClearAllTiles();
        StartCoroutine(DamagePlayer());

        if (growCoroutine != null)
            StopCoroutine(growCoroutine);
        growCoroutine = StartCoroutine(RootGrowing(startTile));
    }

    /// <summary>
    /// Flood fill algorithm. Fills a connected area with the specified tile type.
    /// </summary>
    /// <param name="startTile">Start point for the roots.</param>
    /// <returns></returns>
    private IEnumerator RootGrowing(TileCoord startTile)
    {
        // Wait for the start time
        yield return new WaitForSeconds(startTime);

        // Queue containing the tiles that will become roots.
        Queue<TileCoord> roots = new Queue<TileCoord>();
        // Array that keeps track of the already converted tiles.
        int[,] tileFlags = new int[growTiles.GetLength(0), growTiles.GetLength(1)];

        roots.Enqueue(startTile);
        tileFlags[startTile.xCoord, startTile.yCoord] = 1;
        UpdateTile(startTile, rootTile);

        while (roots.Count > 0)
        {
            TileCoord tile = roots.Dequeue();

            for (int iX = tile.xCoord - 1; iX <= tile.xCoord + 1; iX++)
            {
                for (int iY = tile.yCoord - 1; iY <= tile.yCoord + 1; iY++)
                {
                    if (IsInsideLevel(new TileCoord(iX, iY)) && (iX == tile.xCoord || iY == tile.yCoord))
                    {
                        if (tileFlags[iX, iY] == 0 && growTiles[iX, iY] == tileType)
                        {
                            TileCoord newRoot = new TileCoord(iX, iY);

                            roots.Enqueue(newRoot);
                            tileFlags[iX, iY] = 1;
                            UpdateTile(newRoot, rootTile);

                            Vector3 pos = rootsTilemap.GetCellCenterWorld(new Vector3Int(iX, iY));
                            rootGrowEffect.transform.position = pos;
                            rootGrowEffect.Play();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(growStep);
        }
    }

    /// <summary>
    /// Checks if a coordinate is inside level boundaries.
    /// </summary>
    /// <param name="coord">Tile coordinate to check.</param>
    /// <returns>If the tile coordinate is inside the level boundaries.</returns>
    private bool IsInsideLevel(TileCoord coord)
    {
        return (coord.xCoord >= 0 && coord.xCoord <= width && coord.yCoord >= 0 && coord.yCoord <= height);
    }

    /// <summary>
    /// Updates the specified tile on the tilemap with a new tile.
    /// </summary>
    /// <param name="tile">Position of the tile.</param>
    /// <param name="newTile">Tile to spawn.</param>
    private void UpdateTile(TileCoord tile, Tile newTile) => rootsTilemap.SetTile(new Vector3Int(tile.xCoord, tile.yCoord), newTile);

    private IEnumerator DamagePlayer()
    {
        // Check if the player position is on top of a root tile.
        if (rootsTilemap.HasTile(new Vector3Int((int)player.position.x, (int)player.position.y)))
        {
            playerHealth.DealDamage(rootsDamage);
        }

        yield return new WaitForSeconds(damageTick);

        StartCoroutine(DamagePlayer());
    }
}