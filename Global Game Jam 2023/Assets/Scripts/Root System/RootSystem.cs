using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

public class RootSystem : MonoBehaviour
{
    private LevelGenerator level;
    private int[,] growTiles;
    private int tileType = 0;
    private int width;
    private int height;

    [SerializeField] private Tile rootTile;
    [Range(0.01f, 3)]
    [SerializeField] private float growStep;
    [Tooltip("Time until the roots start spawning once the level is loaded.")]
    [SerializeField] private float startTime = 3;

    private void Awake()
    {
        level = FindObjectOfType<LevelGenerator>();
    }

    private void OnEnable()
    {
        LevelGenerator.LevelCreated += StartRootGrowing;
    }

    private void OnDisable()
    {
        LevelGenerator.LevelCreated -= StartRootGrowing;
    }

    public void StartRootGrowing(int[,] growTiles, TileCoord startTile)
    {
        this.growTiles = growTiles;
        this.width = growTiles.GetLength(0);
        this.height = growTiles.GetLength(1);

        StartCoroutine(RootGrowing(startTile));
    }

    private IEnumerator RootGrowing(TileCoord startTile)
    {
        yield return new WaitForSeconds(startTime);

        Queue<TileCoord> roots = new Queue<TileCoord>();
        int[,] tileFlags = new int[growTiles.GetLength(0), growTiles.GetLength(1)];

        roots.Enqueue(startTile);
        tileFlags[startTile.xCoord, startTile.yCoord] = 1;
        level.UpdateTile(startTile, rootTile);

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
                            level.UpdateTile(newRoot, rootTile);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(growStep);
        }
    }

    private bool IsInsideLevel(TileCoord coord)
    {
        return (coord.xCoord >= 0 && coord.xCoord <= width && coord.yCoord >= 0 && coord.yCoord <= height);
    }
}
