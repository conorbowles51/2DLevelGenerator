using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
