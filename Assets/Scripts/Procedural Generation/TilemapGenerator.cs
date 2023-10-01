using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap wallTilemap;
    [SerializeField]
    private TileBase groundTile;
    [SerializeField]
    private TileBase wallTile;

    public void PaintGroundTiles(IEnumerable<Vector2Int> tilePositions)
    {
        PaintTiles(tilePositions, groundTilemap, groundTile);
    }

    public void PaintWallTiles(IEnumerable<Vector2Int> wallPositions)
    {
        Tile empty = Tile.CreateInstance<Tile>();
        empty.colliderType = Tile.ColliderType.Grid;

        PaintTiles(wallPositions, groundTilemap, wallTile);
        PaintTiles(wallPositions, wallTilemap, empty);
    }

    private void PaintTiles(IEnumerable<Vector2Int> tilePositions, Tilemap tilemap, TileBase tile)
    {
        foreach(Vector2Int position in tilePositions)
        {
            PaintTile(position, tilemap, tile);
        }
    }

    private void PaintTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int) position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
