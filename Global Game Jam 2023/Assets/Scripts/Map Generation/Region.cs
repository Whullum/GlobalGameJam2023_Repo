using System;
using System.Collections.Generic;
using UnityEngine;

public class Region : IComparable<Region>
{
    /// <summary>
    /// All the regions connected to this one.
    /// </summary>
    public List<Region> ConnectedRegions { get; private set; }
    /// <summary>
    /// Tiles surrounding this region.
    /// </summary>
    public List<TileCoord> BorderTiles { get; private set; }
    /// <summary>
    /// All the tiles of this region.
    /// </summary>
    public List<TileCoord> Tiles { get; private set; }
    /// <summary>
    /// Amount of tiles inside this region.
    /// </summary>
    public int RegionSize { get; private set; }
    /// <summary>
    /// Tells if this region has access to the main region.
    /// </summary>
    public bool IsAccesibleFromMainRegion { get; set; }
    /// <summary>
    /// Tells if this region is the main one.
    /// </summary>
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

    /// <summary>
    /// Calculates the border tiles of this region.
    /// </summary>
    /// <param name="level"></param>
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

    /// <summary>
    /// Checks this regions as accesible from main rooms and updates all regions connected to this one to be accesible from main rooms as well.
    /// </summary>
    private void SetAccesibleFromMainRegion()
    {
        if (IsAccesibleFromMainRegion) return;

        IsAccesibleFromMainRegion = true;

        foreach (Region connectedRegion in ConnectedRegions)
        {
            connectedRegion.IsAccesibleFromMainRegion = true;
        }
    }

    /// <summary>
    /// Connects two regions together.
    /// </summary>
    /// <param name="regionA">First region to connect.</param>
    /// <param name="regionB">Second region to connect.</param>
    public static void ConnectRegions(Region regionA, Region regionB)
    {
        if (regionA.IsAccesibleFromMainRegion)
            regionB.SetAccesibleFromMainRegion();
        else if (regionB.IsAccesibleFromMainRegion)
            regionA.SetAccesibleFromMainRegion();

        regionA.ConnectedRegions.Add(regionB);
        regionB.ConnectedRegions.Add(regionA);
    }

    /// <summary>
    /// Tells if this region is connected to the specified one.
    /// </summary>
    /// <param name="region">Region to check if it's connected.</param>
    /// <returns>If the region is connected.</returns>
    public bool IsConnected(Region region)
    {
        return ConnectedRegions.Contains(region);
    }

    /// <summary>
    /// Used for sort regions.
    /// </summary>
    /// <param name="otherRegion"></param>
    /// <returns></returns>
    public int CompareTo(Region otherRegion)
    {
        return otherRegion.RegionSize.CompareTo(RegionSize);
    }
}
