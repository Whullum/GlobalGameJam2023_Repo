using System;
using System.Collections.Generic;
using UnityEngine;

public class Region : IComparable<Region>
{
    public List<Region> ConnectedRegions { get; private set; }
    public List<TileCoord> BorderTiles { get; private set; }
    public List<TileCoord> Tiles { get; private set; }
    public int RegionSize { get; private set; }
    public bool IsAccesibleFromMainRegion { get; set; }
    public bool IsMainRegion { get; set; }
    

    public Region() { }

    public Region(List<TileCoord> tiles, int[,] level)
    {
        this.Tiles = tiles;
        ConnectedRegions = new List<Region>();
        BorderTiles = new List<TileCoord>();
        RegionSize = tiles.Count;

        CalculateBorderTiles(level);
    }

    private void CalculateBorderTiles(int[,] level)
    {
        foreach (TileCoord tile in Tiles)
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

    public void SetAccesibleFromMainRegion()
    {
        if (IsAccesibleFromMainRegion) return;

        IsAccesibleFromMainRegion = true;

        foreach (Region connectedRegion in ConnectedRegions)
        {
            connectedRegion.IsAccesibleFromMainRegion = true;
        }
    }

    public static void ConnectRegions(Region regionA, Region regionB)
    {
        if (regionA.IsAccesibleFromMainRegion)
            regionB.IsAccesibleFromMainRegion = true;
        else if (regionB.IsAccesibleFromMainRegion)
            regionA.IsAccesibleFromMainRegion = true;

        regionA.ConnectedRegions.Add(regionB);
        regionB.ConnectedRegions.Add(regionA);
    }

    public bool IsConnected(Region region)
    {
        return ConnectedRegions.Contains(region);
    }

    public int CompareTo(Region otherRegion)
    {
        return otherRegion.RegionSize.CompareTo(RegionSize);
    }
}
