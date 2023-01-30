using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    private Grid levelGrid;
    private Tilemap levelTilemap;
    // Bidimensional array containing all the tiles of a level
    private int[,] level;

    [Tooltip("Width of the level.")]
    [SerializeField] private int width;
    [Tooltip("Height of the level.")]
    [SerializeField] private int height;
    [Tooltip("Percentaje of walls inside the tilemap.")]
    [Range(1, 100)]
    [SerializeField] private int tileFillAmount = 50;
    [Tooltip("Number of times the Cellular Automata Algorithm will iterate on this level.")]
    [Range(1, 100)]
    [SerializeField] private int CASteps = 50;
    [Tooltip("Minimun number of tiles for a region to exits.")]
    [SerializeField] private int regionSizeThreshold = 50;
    [Tooltip("Seed used for map generation. Leave empty for a random seed.")]
    [SerializeField] private string mapSeed;
    [SerializeField] private Tile tileTest;

    private void Awake()
    {
        levelGrid = GetComponentInChildren<Grid>();
        levelTilemap = GetComponentInChildren<Tilemap>();

        SeedGenerator.GenerateSeed(mapSeed);
        InitializeRandomTilemap();
        CelullarAutomata(CASteps, false);
        CalculateLevelRegions();
        DrawTilemap();
    }

    /// <summary>
    /// Initializes the tilemap with random noise.
    /// </summary>
    private void InitializeRandomTilemap()
    {
        // Middle of the tilemap
        int levelMiddle = height / 2;
        level = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // If this tile is a border tile, it stays as a wall
                if (x == 0 | x == width - 1 || y == 0 || y == height - 1)
                    level[x, y] = 1;
                else
                {
                    // We keep the middle tiles empty because CA performs better this way.
                    if (x == levelMiddle)
                        level[x, y] = 0;
                    else
                        level[x, y] = Random.Range(0, 100) < tileFillAmount ? 1 : 0;
                }
            }
        }
    }

    /// <summary>
    /// Main Cellular Automata algorithm.
    /// </summary>
    /// <param name="strenght">Number of steps to execute.</param>
    /// <param name="clean">False to create more narrow passages. true if we want more open spaces.</param>
    private void CelullarAutomata(int strenght, bool clean = false)
    {
        int i = 0;

        while (i < strenght)
        {
            int[,] newLevel = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighborWalls1 = GetSurroundingWallCount(x, y, 1);
                    int neighborWalls2 = GetSurroundingWallCount(x, y, 2);

                    if (IsWall(x, y))
                    {
                        if (neighborWalls1 > 3)
                            newLevel[x, y] = 1;
                        else
                            newLevel[x, y] = 0;
                    }
                    else
                    {
                        if (!clean)
                        {
                            if (neighborWalls1 >= 5 || neighborWalls2 <= 2)
                                newLevel[x, y] = 1;
                            else
                                newLevel[x, y] = 0;
                        }
                        else
                        {
                            if (neighborWalls1 >= 5)
                                newLevel[x, y] = 1;
                            else
                                newLevel[x, y] = 0;
                        }
                    }
                }
            }
            level = newLevel;
            i++;
        }
    }

    /// <summary>
    /// Gets all the neighbor tiles of a specifies tile and return the number of walls around it.
    /// </summary>
    /// <param name="x">Tile X coordinate.</param>
    /// <param name="y">Tile Y coordinate.</param>
    /// <param name="radius">Radious around the target coordinate to look at when counting walls.</param>
    /// <returns>Number of walls aorund a tile.</returns>
    private int GetSurroundingWallCount(int x, int y, int radius)
    {
        int wallCount = 0;

        for (int iX = x - radius; iX <= x + radius; iX++)
        {
            for (int iY = y - radius; iY <= y + radius; iY++)
            {
                if (iX != x || iY != y)
                    wallCount += IsWall(iX, iY) ? 1 : 0;
            }
        }
        return wallCount;
    }

    /// <summary>
    /// Checks if a specific coordinate is a wall.
    /// </summary>
    /// <param name="x">Tile X coordinate.</param>
    /// <param name="y">Tile Y coordinate.</param>
    /// <returns>True if it is a wall. False if not.</returns>
    private bool IsWall(int x, int y)
    {
        if (x < 0 || y < 0)
            return true;
        if (x > width - 1 || y > height - 1)
            return true;
        return level[x, y] == 1;
    }

    /// <summary>
    /// Places a placeholder tile for showing purposes. This needs to be replaced when we have the final art.
    /// </summary>
    private void DrawTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (level[x, y] == 1)
                    levelTilemap.SetTile(new Vector3Int(x, y, 0), tileTest);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CalculateLevelRegions()
    {
        List<List<TileCoord>> regions = GetAllRegions(0);
        List<Region> finalRegions = new List<Region>();

        foreach (List<TileCoord> region in regions)
        {
            tileTest.color = Random.ColorHSV();
            if (region.Count < regionSizeThreshold)
            {
                foreach (TileCoord tile in region)
                {
                    level[tile.xCoord, tile.yCoord] = 1;
                }
            }
            else
                finalRegions.Add(new Region(region, level));
        }
        ConnectAllRegions(finalRegions);
    }

    private void ConnectAllRegions(List<Region> regions)
    {
        TileCoord closestTileA = new TileCoord();
        TileCoord closestTileB = new TileCoord();
        Region closestRegionA = new Region();
        Region closestRegionB = new Region();
        bool connectionFound = false;
        int closestDistance = 0;

        foreach (Region regionA in regions)
        {
            connectionFound = false;

            foreach (Region regionB in regions)
            {
                if (regionA == regionB)
                    continue;

                if (regionA.IsConnected(regionB))
                {
                    connectionFound = false;
                    break;
                }


                for (int tileIndexA = 0; tileIndexA < regionA.BorderTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < regionB.BorderTiles.Count; tileIndexB++)
                    {
                        TileCoord tileA = regionA.BorderTiles[tileIndexA];
                        TileCoord tileB = regionB.BorderTiles[tileIndexB];
                        int distanceBetweenRegions = (int)(Mathf.Pow(tileA.xCoord - tileB.xCoord, 2) + Mathf.Pow(tileA.yCoord - tileB.yCoord, 2));

                        if (distanceBetweenRegions < closestDistance || !connectionFound)
                        {
                            closestDistance = distanceBetweenRegions;
                            connectionFound = true;
                            closestTileA = tileA;
                            closestTileB = tileB;
                            closestRegionA = regionA;
                            closestRegionB = regionB;
                        }
                    }
                }
                if (connectionFound)
                {
                    CreatePassage(closestRegionA, closestRegionB, closestTileA, closestTileB);
                    continue;
                }

            }
        }
    }

    private void CreatePassage(Region regionA, Region regionB, TileCoord tileA, TileCoord tileB)
    {
        Region.ConnectRegions(regionA, regionB);
        //Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100f);
    }

    private Vector3 CoordToWorldPoint(TileCoord coordinate)
    {
        return levelTilemap.CellToWorld(new Vector3Int(coordinate.xCoord, coordinate.yCoord));
    }

    /// <summary>
    /// Iterates over all the level and calculates each separated region of it.
    /// </summary>
    /// <param name="tileType">The tile type to search for. 0 is empty space and 1 is wall.</param>
    /// <returns>List containing lists of coordinates (regions).</returns>
    private List<List<TileCoord>> GetAllRegions(int tileType)
    {
        // List containing all regions.
        List<List<TileCoord>> allRegions = new List<List<TileCoord>>();
        // Cache array used to check if a tile is already visited
        int[,] levelFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Check if the tile is not yet visited and is of the same type that the tile type
                if (levelFlags[x, y] == 0 && level[x, y] == tileType)
                {
                    // Create a new region specifiying the start tile
                    List<TileCoord> region = GetRegionTiles(x, y);
                    // Add the new region to the list
                    allRegions.Add(region);

                    // Now, for each tile in the new region, we set them to be already visited
                    foreach (TileCoord tile in region)
                    {
                        levelFlags[tile.xCoord, tile.yCoord] = 1;
                    }
                }
            }
        }
        return allRegions;
    }

    /// <summary>
    /// Flodd fill algorithm. Checks for the same tile near the initial tile and creates a list containing all the coordinates of each tile.
    /// </summary>
    /// <param name="startX">Tile X coordinate.</param>
    /// <param name="startY">Tile Y coordinate.</param>
    /// <returns>List containing all the tiles coordinates inside a region.</returns>
    private List<TileCoord> GetRegionTiles(int startX, int startY)
    {
        // Initialize the list containing all the coordinates
        List<TileCoord> regionTiles = new List<TileCoord>();
        // We keep a cache array containing the already visited tiles
        int[,] regionFlags = new int[width, height];
        // Tile type to search for
        int tileType = level[startX, startY];
        // Queue for the Flood Fill algorithim containing coordinates of the tiles to check
        Queue<TileCoord> tileQueue = new Queue<TileCoord>();

        // Add the start tile to the queue.
        tileQueue.Enqueue(new TileCoord(startX, startY));
        // Add the start tile to the cache
        regionFlags[startX, startY] = 1;

        // While there are tiles to check, we kept running the algorithm
        while (tileQueue.Count > 0)
        {
            // Return first tile and remove it from the queue
            TileCoord tile = tileQueue.Dequeue();
            // Add the tile to the list containing all the tiles in this region
            regionTiles.Add(tile);

            for (int x = tile.xCoord - 1; x <= tile.xCoord + 1; x++)
            {
                for (int y = tile.yCoord - 1; y <= tile.yCoord + 1; y++)
                {
                    // If the tile is inside the level and is not a diagonal tile.
                    if (TileInsideLevel(x, y) && (y == tile.yCoord || x == tile.xCoord))
                    {
                        // If we haven't looked to this tile yet and is the right tile (the same that the start tile)
                        if (regionFlags[x, y] == 0 && level[x, y] == tileType)
                        {
                            // We set this tile to already looked.
                            regionFlags[x, y] = 1;

                            // Add to the queue the new coordinates
                            tileQueue.Enqueue(new TileCoord(x, y));
                        }
                    }
                }
            }
        }
        return regionTiles;
    }

    /// <summary>
    /// Checks if a specified tile is inside the level.
    /// </summary>
    /// <param name="x">Tile X coordinate.</param>
    /// <param name="y">Tile Y coordinate.</param>
    /// <returns>True if it is inside the level. False if not.</returns>
    private bool TileInsideLevel(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    [ContextMenu("New level")]
    private void GenerateNewRandomMap()
    {
        levelTilemap.ClearAllTiles();
        SeedGenerator.GenerateSeed(mapSeed);
        InitializeRandomTilemap();
        CelullarAutomata(CASteps, true);
        CalculateLevelRegions();
        DrawTilemap();
    }
}
