using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameWorld
{
    public static readonly bool[,,] WorldMatrix = new bool[W,H,D];
 
    public const int W = 5;
    public const int H = 10;
    public const int D = 5;
    
    public static bool CanMoveTo(List<PositionComponent> relatedPositionComponents, Vector3Int newPos)
    {
        if (newPos.x < 0 || newPos.y < 0 || newPos.z < 0)
            return false;

        if (newPos.x >= W || newPos.y >= H || newPos.z >= D)
            return false;

        if (WorldMatrix[newPos.x, newPos.y, newPos.z])
        {
            if (relatedPositionComponents.All(p => p.position != newPos))
                return false;
        }

        return true;
    }
}
