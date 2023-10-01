using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
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

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int walkLength)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int direction = Direction2D.GetRandomCardinalDirection();
        Vector2Int previousPosition = startPosition;

        path.Add(startPosition);

        for(int i = 0; i < walkLength; i++)
        {
            Vector2Int newPosition = previousPosition + direction;
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }
}