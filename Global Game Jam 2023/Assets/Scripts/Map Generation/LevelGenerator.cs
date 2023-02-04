using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
    /// <summary>
    /// Invoked when the level finished loading. Returns the array containing level structure and the player spawn position.
    /// </summary>
    public static Action<int[,], TileCoord> LevelCreated;
    /// <summary>
    /// When the tilemap is finished drawing, this event is invoked.
    /// </summary>
    public static Action<Tilemap> LevelColliderCreated;

    private Grid levelGrid;
    private Tilemap levelTilemap;
    private PlayerSpawner playerSpawner;
    private LevelExit levelExit;
    private TileCoord rootsStartTile;
    private int[,] level;// Bidimensional array containing all the tiles of a level

    [Tooltip("Width of the level.")]
    [SerializeField] private int width;
    [Tooltip("Height of the level.")]
    [SerializeField] private int height;
    [Tooltip("Percentaje of walls inside the tilemap.")]
    [Range(1, 100)]
    [SerializeField] private int tileFillAmount = 50;
    [Tooltip("Number of times the Cellular Automata Algorithm will iterate on this level.")]
    [Range(1, 100)]
    [SerializeField] private int CASteps = 40;
    [Tooltip("Minimun number of tiles for a region to exits.")]
    [SerializeField] private int regionSizeThreshold = 25;
    [Tooltip("Minimun distance from the player spawn and the level exit.")]
    [Range(5, 30)]
    [SerializeField] private int minimunDoorDistance = 25;
    [Tooltip("Size of the passages created between regions.")]
    [Range(1, 4)]
    [SerializeField] private int passageCreationRadius = 2;
    [Tooltip("Seed used for map generation. Leave empty for a random seed.")]
    [SerializeField] private string mapSeed;
    [Tooltip("Generated seed, for visualization purposes only.")]
    [SerializeField] private int generatedSeed;
    [SerializeField] private RuleTile tileTest;
    [SerializeField] private Tile normalTile;
    [SerializeField] private Tile exitTile;

    private void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        levelExit = FindObjectOfType<LevelExit>();
        levelGrid = GetComponentInChildren<Grid>();
        levelTilemap = GetComponentInChildren<Tilemap>();
    }

    public void CreateNewLevel()
    {
        levelTilemap.ClearAllTiles();
        generatedSeed = SeedGenerator.GenerateSeed(mapSeed);
        InitializeRandomTilemap();
        CelullarAutomata(CASteps, false);
        CalculateLevelRegions();
        DrawTilemap();
        levelTilemap.SetTile(
            new Vector3Int(
                (int)levelExit.transform.position.x,
                (int)levelExit.transform.position.y,
                0), 
            exitTile);
        LevelCreated?.Invoke(level, rootsStartTile);
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
                        level[x, y] = UnityEngine.Random.Range(0, 100) < tileFillAmount ? 1 : 0;
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
                else
                    levelTilemap.SetTile(new Vector3Int(x, y, 0), normalTile);
            }
        }
        LevelColliderCreated?.Invoke(levelTilemap);
    }

    /// <summary>
    /// Calculates all the regions inside this level, removes the smaller ones and sorts them from bigger to smaller. Finally, connects all of them together.
    /// </summary>
    private void CalculateLevelRegions()
    {
        List<List<TileCoord>> regions = GetAllRegions(0);// We get a list with all the walkable regions
        List<Region> finalRegions = new List<Region>(); // List containing the surviving regions

        foreach (List<TileCoord> region in regions)
        {
            // Remove the smaller regions
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
        finalRegions.Sort();
        finalRegions[0].IsMainRegion = true; // The biggest one is the main region.
        finalRegions[0].IsAccesibleFromMainRegion = true; // And it is also accesible to itself.

        CreateLevelDoors(finalRegions);
        ConnectAllRegions(finalRegions);
    }

    /// <summary>
    /// Creates the player spawn position and the level exit position.
    /// </summary>
    /// <param name="regions">List containing all the regions of the map.</param>
    private void CreateLevelDoors(List<Region> regions)
    {
        // We choose different regions if avaliable
        int rndPlayerSpawnRegion = UnityEngine.Random.Range(0, regions.Count);
        int rndExitRegion = UnityEngine.Random.Range(0, regions.Count);

        if (rndPlayerSpawnRegion == rndExitRegion && regions.Count > 1)
        {
            CreateLevelDoors(regions);
            return;
        }

        TileCoord spawnTile = new(); // Tile used for positioning the player spawner
        TileCoord exitTile = new(); // Tile used for positioning the level exit
        bool spawnTileFound = false;
        bool exitTileFound = false;

        // Search for a correct player spawn tile
        while (!spawnTileFound)
        {
            int rndTile = UnityEngine.Random.Range(0, regions[rndPlayerSpawnRegion].Tiles.Count);
            TileCoord selectedTile = regions[rndPlayerSpawnRegion].Tiles[rndTile];

            if (level[selectedTile.xCoord, selectedTile.yCoord] == 0)
            {
                foreach (TileCoord tile in regions[rndPlayerSpawnRegion].Tiles)
                {
                    if (selectedTile.xCoord == tile.xCoord && selectedTile.yCoord == tile.yCoord)
                    {
                        spawnTile = tile;
                        spawnTileFound = true;
                    }
                }
            }
        }

        // Search for a correct player level exit tile
        while (!exitTileFound)
        {
            int rndTile = UnityEngine.Random.Range(0, regions[rndExitRegion].Tiles.Count);
            TileCoord selectedTile = regions[rndExitRegion].Tiles[rndTile];

            if (level[selectedTile.xCoord, selectedTile.yCoord] == 0)
            {
                foreach (TileCoord tile in regions[rndExitRegion].Tiles)
                {
                    if (selectedTile.xCoord == tile.xCoord && selectedTile.yCoord == tile.yCoord)
                    {
                        exitTile = tile;
                        exitTileFound = true;

                    }
                }
            }
        }
        float distanceBetweenDoors = GetDistanceBetweenTiles(spawnTile, exitTile);

        // If the distance between the two doors is less than the minimum distance, we calculate again the positions
        if (distanceBetweenDoors < minimunDoorDistance)
        {
            CreateLevelDoors(regions);
            return;
        }

        // Set the player spawner and level exit position on the newly created map.
        playerSpawner.transform.position = CoordToWorldPoint(spawnTile);
        levelExit.transform.position = CoordToWorldPoint(exitTile);

        levelTilemap.SetTile(new Vector3Int(
            (int)CoordToWorldPoint(exitTile).x, (int)CoordToWorldPoint(exitTile).y, 0), 
            this.exitTile
            );

        rootsStartTile = spawnTile;
    }

    /// <summary>
    /// Calculates the distance between two tile coordinates.
    /// </summary>
    /// <param name="tileA">Tile A.</param>
    /// <param name="tileB">Tile B.</param>
    /// <returns>Distance between tiles.</returns>
    private float GetDistanceBetweenTiles(TileCoord tileA, TileCoord tileB)
    {
        Vector3 a = CoordToWorldPoint(tileA);
        Vector3 b = CoordToWorldPoint(tileB);

        return Vector3.Distance(a, b);
    }

    /// <summary>
    /// Connects all regions together.
    /// </summary>
    /// <param name="regions">List containing the regions to connect.</param>
    /// <param name="forceAccessFromMainRegion">Force the access to the main region.</param>
    private void ConnectAllRegions(List<Region> regions, bool forceAccessFromMainRegion = false)
    {
        // All this regions aren't accesible from the main region
        List<Region> regionsA = new List<Region>();
        // All this regions are accesible from the main region
        List<Region> regionsB = new List<Region>();

        if (forceAccessFromMainRegion)
        {
            foreach (Region r in regions)
            {
                if (r.IsAccesibleFromMainRegion)
                    regionsB.Add(r);
                else
                    regionsA.Add(r);
            }
        }
        else
        {
            regionsA = regions;
            regionsB = regions;
        }

        TileCoord closestTileA = new TileCoord();
        TileCoord closestTileB = new TileCoord();
        Region closestRegionA = new Region();
        Region closestRegionB = new Region();
        bool connectionFound = false;
        int closestDistance = 0;

        foreach (Region regionA in regionsA)
        {
            if (!forceAccessFromMainRegion)
            {
                connectionFound = false;

                if (regionA.ConnectedRegions.Count > 0)
                    continue;
            }

            foreach (Region regionB in regionsB)
            {
                if (regionA == regionB || regionA.IsConnected(regionB))
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
            }
            if (connectionFound && !forceAccessFromMainRegion)
                CreatePassage(closestRegionA, closestRegionB, closestTileA, closestTileB);
        }
        if (connectionFound && forceAccessFromMainRegion)
        {
            CreatePassage(closestRegionA, closestRegionB, closestTileA, closestTileB);

        }

        if (!forceAccessFromMainRegion)
            ConnectAllRegions(regions, true);
    }

    /// <summary>
    /// Creates passages between regions.
    /// </summary>
    /// <param name="regionA">Start region.</param>
    /// <param name="regionB">End regions.</param>
    /// <param name="tileA">Start tile.</param>
    /// <param name="tileB">End tile.</param>
    private void CreatePassage(Region regionA, Region regionB, TileCoord tileA, TileCoord tileB)
    {
        Region.ConnectRegions(regionA, regionB);
        //Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.green, 100f);

        List<TileCoord> line = CreateLine(tileA, tileB);

        foreach (TileCoord tile in line)
            RemoveCircle(tile, passageCreationRadius);
    }

    /// <summary>
    /// Removes the tiles around the specified tile coordinates.
    /// </summary>
    /// <param name="coord">Middle tile.</param>
    /// <param name="radius">Radious to remove.</param>
    private void RemoveCircle(TileCoord coord, int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                // If we are inside the circle
                if (x * x + y * y <= radius * radius)
                {
                    int removeX = coord.xCoord + x;
                    int removeY = coord.yCoord + y;

                    if (TileInsideLevel(removeX, removeY))
                        level[removeX, removeY] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Calculates a line between two Tile Coordinates.
    /// </summary>
    /// <param name="from">Start tile.</param>
    /// <param name="to">End tile.</param>
    /// <returns>List containing all the tiles coordinates inside the level.</returns>
    private List<TileCoord> CreateLine(TileCoord from, TileCoord to)
    {
        List<TileCoord> line = new List<TileCoord>();
        int x = from.xCoord;
        int y = from.yCoord;

        int dx = to.xCoord - from.xCoord;
        int dy = to.yCoord - from.yCoord;

        bool inverted = false;
        int step = (int)Mathf.Sign(dx);
        int gradientStep = (int)Mathf.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = (int)Mathf.Sign(dy);
            gradientStep = (int)Mathf.Sign(dx);
        }
        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new TileCoord(x, y));

            if (inverted)
                y += step;
            else
                x += step;

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                    x += gradientStep;
                else
                    y += gradientStep;
            }
        }
        return line;
    }

    /// <summary>
    /// Converts a tile coordinate to a world Vector3.
    /// </summary>
    /// <param name="coordinate">Tile coordinate.</param>
    /// <returns>Vector3 containing the world position.</returns>
    private Vector3 CoordToWorldPoint(TileCoord coordinate)
    {
        return levelTilemap.GetCellCenterWorld(new Vector3Int(coordinate.xCoord, coordinate.yCoord));
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
    /// Flood fill algorithm. Checks for the same tile near the initial tile and creates a list containing all the coordinates of each tile.
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
        CreateNewLevel();
    }
}
