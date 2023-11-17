using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

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
            var gravity = movementComponent.GetComponent<GravityComponent>();
            if (gravity != null && gravity.isGrounded)
                continue;
            
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
        if (BlackBoard.lastMoveKey.HasValue == false)
            return Vector3Int.zero;

        switch (BlackBoard.CameraPosition)
        {
            case CameraPosition.Front:
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveRight)
                    return Vector3Int.right;
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveLeft)
                    return Vector3Int.left;
                break;
            
            case CameraPosition.Right:
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveRight)
                    return Vector3Int.forward;
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveLeft)
                    return Vector3Int.back;
                break;
            
            case CameraPosition.Back:
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveRight)
                    return -Vector3Int.right;
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveLeft)
                    return -Vector3Int.left;
                break;
            
            case CameraPosition.Left:
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveRight)
                    return -Vector3Int.forward;
                if (BlackBoard.lastMoveKey.Value == InputSystem.MoveLeft)
                    return -Vector3Int.back;
                break;
        }
        
     
        
        return Vector3Int.zero;
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