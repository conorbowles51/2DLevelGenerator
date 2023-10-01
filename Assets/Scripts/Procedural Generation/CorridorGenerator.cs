using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CorridorGenerator
{
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

    public static HashSet<Vector2Int> GetPotentialRoomPositions(List<List<Vector2Int>> corridors)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        foreach(List<Vector2Int> corridor in corridors)
        {
            positions.Add(corridor[0]);
            positions.Add(corridor[corridor.Count - 1]);
        }

        return positions;
    }

    public static List<List<Vector2Int>> ResizeCorridors(List<List<Vector2Int>> corridors, int size)
    {
        List<List<Vector2Int>> resizedCorridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridors.Count; i++)
        {
            resizedCorridors.Add(ResizeCorridor(corridors[i], size));
        }

        return resizedCorridors;
    }

    private static List<Vector2Int> ResizeCorridor(List<Vector2Int> corridor, int size)
    {
        List<Vector2Int> resizedCorridor = new List<Vector2Int>();

        foreach(Vector2Int position in corridor)
        {
            for(int x = -size; x <= size; x++)
            {
                for(int y = -size; y <= size; y++)
                {
                    resizedCorridor.Add(new Vector2Int(position.x + x, position.y + y));
                }
            }
        }

        return resizedCorridor;
    }
}
