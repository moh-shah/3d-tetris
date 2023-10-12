using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDestructionSystem : ISystem
{
    private readonly bool[,] _floor = new bool[GameWorld.W,GameWorld.D];
    
    public void OnFrameUpdate()
    {
        
    }
    
    public void OnSystemUpdate()
    {
        for (var y = 0; y < GameWorld.H; y++)
        {
            for (var x = 0; x < GameWorld.W; x++)
            {
                for (var z = 0; z < GameWorld.D; z++)
                {
                    _floor[x,z] = GameWorld.WorldMatrix[x, y, z];
                }
            }

            var isFloorFull = true;
            for (var x = 0; x < GameWorld.W; x++)
                for (var z = 0; z < GameWorld.D; z++)
                    if (_floor[x, z] == false)
                        isFloorFull = false;
                
            
            if (isFloorFull)
            {
                var allGroundedBlocks = Object.FindObjectsOfType<GravityComponent>().Where(g => g.isGrounded).ToList();
                var allGroundedPositionComponents = new List<PositionComponent>();
                foreach (var groundedBlock in allGroundedBlocks)
                    allGroundedPositionComponents.AddRange(groundedBlock.GetComponentsInChildren<PositionComponent>());
                
                var blocksToDestroy = allGroundedPositionComponents.Where(p=>p.position.y == y).ToList();
                foreach (var positionComponent in blocksToDestroy)
                {
                    GameWorld.WorldMatrix[positionComponent.position.x, positionComponent.position.y, positionComponent.position.z] = false;
                    Object.Destroy(positionComponent.gameObject);
                }
                var allBlocksAbove = allGroundedPositionComponents.Where(p=>p.position.y > y).ToList();
                foreach (var positionComponent in allBlocksAbove)
                {
                    GameWorld.WorldMatrix[positionComponent.position.x, positionComponent.position.y, positionComponent.position.z] = false;
                    positionComponent.position.y -= 1;
                    GameWorld.WorldMatrix[positionComponent.position.x, positionComponent.position.y, positionComponent.position.z] = true;
                    positionComponent.gameObject.transform.position = positionComponent.position;
                }
            }
        }
    }
}