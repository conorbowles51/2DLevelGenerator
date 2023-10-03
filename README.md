## Introduction

This project is a tool for Unity's 2D workspace that allows users
to create procedurally generated levels in the Unity editor or at runtime. It accepts a preset
that will define the style of the level and then generates it and places tiles
in the scene.

## Using the tool
![dungeongeneratorSS](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/517061f9-1c21-4067-8670-6fd56332a2f2)

To use the tool, simply attach the 'Dungeon Generator' script to an empty object, configure the settings how you like, and hit the generate button at the bottom.
You can create your own preset or use one of the three already provided.

Here are some of the results with different settings and presets:

![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/3c08cccd-24ec-419a-be19-5930479c7645)


![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/f0f02d36-0718-4c21-81dd-72bfddc4e3da)


![image](https://github.com/conorbowles51/2DLevelGenerator/assets/143211735/7175a6cd-1e6c-44c5-8fb2-83316bde766a)

## How it works

The main algorithm used in this project is the simple random walk algorithm. It takes a start position, moves one unit in a random direction, then repeats
this process for the duration of the walk length.

```cs
public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        Vector2Int previousPosition = startPosition;

        for(int i = 0; i < walkLength; i++)
        {
            Vector2Int newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }
```

The first step is to generate the corridors, which are the hallways that connect the rooms together.

```c#
public static List<List<Vector2Int>> CreateCorridors(Vector2Int startPosition, int corridorCount, int corridorLength, int corridorWidth)
    {
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        Vector2Int currentPosition = startPosition;

        for (int i = 0; i < corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);

            corridors.Add(corridor);

            currentPosition = corridor[corridor.Count - 1];
        }

        return corridors;
    }
```

The intersections (or dead ends) of these corridors are where rooms will be placed afterwards. These are easily found 
as they are just the first and last positions in every corridor. It is important that these corridors are firstly generated with a
width of 1 unit and only scaled up to the desired width after dead ends are found, as the method for doing so relies on the fact that a position has
exactly one neighbouring position.

```c#
        List<List<Vector2Int>> corridors = CorridorGenerator.CreateCorridors(startPosition, corridorCount, corridorLength, corridorWidth);
        
        HashSet<Vector2Int> potentialRoomPositions = CorridorGenerator.GetPotentialRoomPositions(corridors);

        for (int i = 0; i < corridors.Count; i++)
        {
            groundPositions.UnionWith(corridors[i]);
        }

        HashSet<Vector2Int> deadEndPositions = FindAllDeadEnds(groundPositions);
        corridors = CorridorGenerator.ResizeCorridors(corridors, corridorWidth); // MUST resize corridors AFTER dead ends have been found
```

Once the corridors are resized, the rooms will then be generated at the potential positions for them that were found. This is done with
an algorithm very similar to the previous one. It will use the desired walk length and number of iterations set in the preset. 

```c#
        HashSet<Vector2Int> roomPositions = RoomGenerator.CreateRooms(roomPercent, potentialRoomPositions, deadEndPositions, preset);

        groundPositions.UnionWith(roomPositions);
```
```c#
public static class RoomGenerator
{
    public static HashSet<Vector2Int> CreateRoom(SimpleRandomWalkPreset preset, Vector2Int startPosition)
    {
        Vector2Int currentPosition = startPosition;
        HashSet<Vector2Int> groundPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < preset.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, preset.walkLength);
            groundPositions.UnionWith(path); // add without duplicates

            if(preset.useRandomStartPosition)
            {
                currentPosition = groundPositions.ElementAt(Random.Range(0, groundPositions.Count));
            }
        }

        return groundPositions;
    }

    public static HashSet<Vector2Int> CreateRooms(float roomPercent, HashSet<Vector2Int> potentialRoomPositions, HashSet<Vector2Int> deadEndPositions, SimpleRandomWalkPreset preset)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

        int roomsToCreateCount = (int)(roomPercent * potentialRoomPositions.Count) - deadEndPositions.Count;
        
        List<Vector2Int> prpList = potentialRoomPositions.ToList();

        // remove dead ends if they're in this list, and add room at their pos
        foreach(Vector2Int deadEndPos in deadEndPositions)
        {
            if(prpList.Contains(deadEndPos))
            {
                prpList.Remove(deadEndPos);
            }

            roomPositions.UnionWith(RoomGenerator.CreateRoom(preset, deadEndPos));
        }

        for(int i = 0; i < roomsToCreateCount; i++)
        {
            int index = Random.Range(0, prpList.Count);
            roomPositions.UnionWith(RoomGenerator.CreateRoom(preset, prpList[index]));

            prpList.RemoveAt(index);
        }

        return roomPositions;
    }
}
```
The next step is to take the ground positions generated and create positions for the walls.
This is done by simply checking the neighbouring positions of the ground positions, and if the neighbour is
not a ground position, then it should be a wall position.

```c#
        wallPositions = WallGenerator.GenerateWalls(groundPositions, Direction2D.EightWayDirections);

        GenerateTiles();

        return groundPositions;
```
```c#
public static class WallGenerator
{
    public static HashSet<Vector2Int> GenerateWalls(HashSet<Vector2Int> groundPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        foreach(Vector2Int position in groundPositions)
        {
            foreach(Vector2Int direction in directions)
            {
                Vector2Int neighbourPosition = position + direction;
                
                if(!groundPositions.Contains(neighbourPosition))
                {
                    positions.Add(neighbourPosition);
                }
            }
        }

        return positions;
    }
}
```

Finally, these position get passed to the Tilemap Generator which will render the wall and ground tiles
to the scene.

```c#
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

```

