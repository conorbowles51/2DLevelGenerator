using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Generator")]
    [SerializeField]
    protected TilemapGenerator tilemapGenerator = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    private HashSet<Vector2Int> groundPositions;
    private HashSet<Vector2Int> wallPositions;

    [Header("Corridors")]
    [SerializeField]
    private int corridorLength = 10;
    [SerializeField]
    private int corridorWidth= 1;
    [SerializeField]
    private int corridorCount = 5;
    [SerializeField]
    [Range(0f, 1f)]
    private float roomPercent = 0.8f;

    [Header("Simple Random Walk")]
    [SerializeField]
    private SimpleRandomWalkPreset preset;


    public HashSet<Vector2Int> GenerateDungeon()
    {
        groundPositions = new HashSet<Vector2Int>();
        wallPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CorridorGenerator.CreateCorridors(startPosition, corridorCount, corridorLength, corridorWidth);
        
        HashSet<Vector2Int> potentialRoomPositions = CorridorGenerator.GetPotentialRoomPositions(corridors);

        for (int i = 0; i < corridors.Count; i++)
        {
            groundPositions.UnionWith(corridors[i]);
        }

        HashSet<Vector2Int> deadEndPositions = FindAllDeadEnds(groundPositions);
        corridors = CorridorGenerator.ResizeCorridors(corridors, corridorWidth); // MUST resize corridors AFTER dead ends have been found

        for (int i = 0; i < corridors.Count; i++)
        {
            groundPositions.UnionWith(corridors[i]);
        }
        
        HashSet<Vector2Int> roomPositions = RoomGenerator.CreateRooms(roomPercent, potentialRoomPositions, deadEndPositions, preset);

        groundPositions.UnionWith(roomPositions);

        wallPositions = WallGenerator.GenerateWalls(groundPositions, Direction2D.EightWayDirections);

        GenerateTiles();

        return groundPositions;
    }

    private void GenerateTiles()
    {
        tilemapGenerator.Clear();
        tilemapGenerator.PaintGroundTiles(groundPositions);
        tilemapGenerator.PaintWallTiles(wallPositions);
    } 

    private HashSet<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> positionsToCheck)
    {
        HashSet<Vector2Int> deadEnds = new HashSet<Vector2Int>();

        foreach(Vector2Int position in positionsToCheck)
        {
            int neighbourCount = 0;

            foreach(Vector2Int direction in Direction2D.cardinalDirections)
            {
                if(positionsToCheck.Contains(position + direction))
                    neighbourCount++;
            }

            if(neighbourCount == 1)
                deadEnds.Add(position);
        }

        return deadEnds;
    }
}


// Potential issue:
// Idealy, wall tiles would be placed on the 'Walls' tile map
// and their colliders automatically generated from that.
//
// In order to place the correct wall tile sprite we need the information of
// weather or not there is a ground tile as one of the neighbours of the wall,
// which is only available if we place the wall tiles on the 'Ground' tile map, with the ground tiles.
//
// To get around this the current system places all wall tiles on the 'Ground' tilemap
// and places empty tiles (no sprite) with colliders on the 'Wall' tile map
//
// See 'PaintWallTiles()' in 'TilemapGenerator.cs'

// EDIT: Yes this is indeed an issue. Lights should not light walls the same way they do ground tiles,
// and for this to happen we need the walls on their own layer, in a separate tile map.