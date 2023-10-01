using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class WallRuleTile : RuleTile<WallRuleTile.Neighbor> 
{
    public TileBase groundTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor 
    {
        public const int Null = 3;
        public const int NotNull = 4;
        public const int Ground = 5;
        public const int NotGround = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) 
    {
        switch (neighbor) 
        {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
            case Neighbor.Ground: return tile == groundTile;
            case Neighbor.NotGround: return tile != groundTile;
        }
        return base.RuleMatch(neighbor, tile);
    }
}