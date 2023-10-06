using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : ISystem
{
    private List<MovementComponent> _componentsToUpdate;

    public void OnFrameUpdate()
    {
    }

    public void OnSystemUpdate()
    {
        _componentsToUpdate = Object.FindObjectsOfType<MovementComponent>().ToList();
        var movementVector = MovementVector();
        if (movementVector == Vector3Int.zero)
            return;
        
        foreach (var movementComponent in _componentsToUpdate)
        {
            if (!CanMoveBlock(movementComponent, movementVector))
                continue;

            var pos = movementComponent.transform.position + movementVector;
            movementComponent.transform.position = pos;

            UpdateWorldMatrix(movementComponent, movementVector);
        }
    }

    private void UpdateWorldMatrix(MovementComponent gravityComponent, Vector3Int movementVector)
    {
        var positionComponents = GetPositionComponents(gravityComponent);
        foreach (var positionComponent in positionComponents)
        {
            var initPos = positionComponent.position;
            var newPos = positionComponent.position + movementVector;
            GameWorld.WorldMatrix[initPos.x, initPos.y, initPos.z] = false;
            GameWorld.WorldMatrix[newPos.x, newPos.y, newPos.z] = true;
            positionComponent.position = newPos;
        }
    }

    private Vector3Int MovementVector()
    {
        return BlackBoard.lastMoveKey.HasValue
            ? Settings.keyToMovementDirection[BlackBoard.lastMoveKey.Value] * Settings.movementSpeed
            : Vector3Int.zero;
    }

    private bool CanMoveBlock(MovementComponent movementComponent, Vector3Int dir)
    {
        var positionComponents = GetPositionComponents(movementComponent);
        return CanMove(positionComponents, dir);
    }

    private List<PositionComponent> GetPositionComponents(MovementComponent movementComponent)
    {
        var positionComponents = new List<PositionComponent>();
        if (movementComponent.composite)
            positionComponents = movementComponent.GetComponentsInChildren<PositionComponent>()
                .Where(c => c.gameObject != movementComponent.gameObject).ToList();
        else
            positionComponents.Add(movementComponent.GetComponent<PositionComponent>());

        return positionComponents;
    }

    private bool CanMove(List<PositionComponent> relatedPositionComponents, Vector3Int direction)
    {
        foreach (var positionComponent in relatedPositionComponents)
        {
            var newPos = positionComponent.position;
            newPos += direction;
            if (!GameWorld.CanMoveTo(relatedPositionComponents, newPos)) 
                return false;
        }

        return true;
    }
}