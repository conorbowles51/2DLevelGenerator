using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
