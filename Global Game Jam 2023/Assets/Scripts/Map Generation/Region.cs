using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public List<Region> ConnectedRegions { get; set; }
    public List<TileCoord> BorderTiles { get; set; }

    private List<TileCoord> tiles = new List<TileCoord>();



    public Region() { }

    public Region(List<TileCoord> tiles, int[,] level)
    {
        this.tiles = tiles;
        ConnectedRegions = new List<Region>();
        BorderTiles = new List<TileCoord>();

        CalculateBorderTiles(level);
    }

    private void CalculateBorderTiles(int[,] level)
    {
        foreach (TileCoord tile in tiles)
        {
            for (int x = tile.xCoord - 1; x <= tile.xCoord + 1; x++)
            {
                for (int y = tile.yCoord - 1; y <= tile.yCoord + 1; y++)
                {
                    // We skip the diagonal tiles
                    if (x == tile.xCoord || y == tile.yCoord)
                    {
                        if (level[x, y] == 1)
                            BorderTiles.Add(tile);
                    }
                }
            }
        }
    }

    public static void ConnectRegions(Region regionA, Region regionB)
    {
        regionA.ConnectedRegions.Add(regionB);
        regionB.ConnectedRegions.Add(regionA);
    }

    public bool IsConnected(Region region)
    {
        return ConnectedRegions.Contains(region);
    }
}
