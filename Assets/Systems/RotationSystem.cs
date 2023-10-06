﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class RotationSystem : ISystem
{
    private List<RotationComponent> _rotationComponentsToUpdate;
    private const float Radians = 90 * Mathf.Deg2Rad;

    public void OnFrameUpdate()
    {
    }

    public void OnSystemUpdate()
    {
        _rotationComponentsToUpdate = Object.FindObjectsOfType<RotationComponent>().ToList();
        var axis = RotationAxis();
        if (axis == Axis.Undefined)
            return;

        foreach (var rotationComponent in _rotationComponentsToUpdate)
        {
            if (!CanMoveBlock(rotationComponent, axis, out var rotatedPositions))
            {
                continue;
            }

            var positionComponents = GetPositionComponents(rotationComponent);
            for (var index = 0; index < positionComponents.Count; index++)
            {
                var positionComponent = positionComponents[index];
                GameWorld.WorldMatrix[positionComponent.position.x, positionComponent.position.y,positionComponent.position.z] = false;
                positionComponent.position = rotatedPositions[index];
                positionComponent.gameObject.transform.position = positionComponent.position;
                GameWorld.WorldMatrix[positionComponent.position.x, positionComponent.position.y,positionComponent.position.z] = true;
            }
        }
    }

    private bool CanMoveBlock(RotationComponent rotationComponent, Axis axis, out List<Vector3Int> rotatedPositions)
    {
        var positionComponents = GetPositionComponents(rotationComponent);
        return CanMove(positionComponents, axis, out rotatedPositions);
    }

    private List<PositionComponent> GetPositionComponents(RotationComponent rotationComponent)
    {
        var positionComponents = new List<PositionComponent>();
        if (rotationComponent.composite)
            positionComponents = rotationComponent.GetComponentsInChildren<PositionComponent>()
                .Where(c => c.gameObject != rotationComponent.gameObject).ToList();
        else
            positionComponents.Add(rotationComponent.GetComponent<PositionComponent>());

        return positionComponents;
    }

    private bool CanMove(List<PositionComponent> relatedPositionComponents, Axis axis, out List<Vector3Int> newPositions)
    {
        newPositions = new List<Vector3Int>();
        var com = Vector3Int.zero;
        var comObj = relatedPositionComponents.FirstOrDefault(c => c.centerOfMass);
        if (comObj != null)
            com = comObj.position;

        for (var index = 0; index < relatedPositionComponents.Count; index++)
        {
            var positionComponent = relatedPositionComponents[index];
            var newPos = GetRotatedPosition(positionComponent, com, axis);
            newPositions.Add(newPos);
            if (!GameWorld.CanMoveTo(relatedPositionComponents, newPos))
            {
                Debug.LogError($"cant move {positionComponent.gameObject.name} with pos:{positionComponent.position} to {newPos}");
                return false;
            }
        }

        return true;
    }

    private Vector3Int GetRotatedPosition(PositionComponent pos, Vector3Int com, Axis axis)
    {
        var intPos = pos.position - com;
        var newPos = Vector3Int.zero;
        switch (axis)
        {
            case Axis.X:
                newPos = RotateAroundX(intPos, Radians);
                break;

            case Axis.Y:
                newPos = RotateAroundY(intPos, Radians);
                break;

            case Axis.Z:
                newPos = RotateAroundZ(intPos, Radians);
                break;
        }

        return newPos + com;
    }

    private Vector3Int RotateAroundX(Vector3Int position, float radians)
    {
        var newX = position.x;
        var newY = position.y * Mathf.Cos(radians) - position.z * Mathf.Sin(radians);
        var newZ = position.y * Mathf.Sin(radians) + position.z * Mathf.Cos(radians);
        return new Vector3Int(newX, (int)newY, (int)newZ);
    }

    private Vector3Int RotateAroundY(Vector3Int position, float radians)
    {
        var newX = position.x * Mathf.Cos(radians) + position.z * Mathf.Sin(radians);
        var newY = position.y;
        var newZ = -position.x * Mathf.Sin(radians) + position.z * Mathf.Cos(radians);
        return new Vector3Int((int)newX, newY, (int)newZ);
    }

    private Vector3Int RotateAroundZ(Vector3Int position, float radians)
    {
        var newX = position.x * Mathf.Cos(radians) - position.y * Mathf.Sin(radians);
        var newY = position.x * Mathf.Sin(radians) + position.y * Mathf.Cos(radians);
        var newZ = position.z;
        return new Vector3Int((int)newX, (int)newY, newZ);
    }

    private Axis RotationAxis()
    {
        return BlackBoard.lastRotationKey.HasValue
            ? Settings.keyToRotationAxis[BlackBoard.lastRotationKey.Value]
            : Axis.Undefined;
    }
}