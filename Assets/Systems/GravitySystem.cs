using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravitySystem : ISystem
{
    private List<GravityComponent> _componentsToUpdate;

    private const int GravityStep = 1;
    
    public void OnFrameUpdate()
    {
        
    }

    public void OnSystemUpdate()
    {
        return;
        _componentsToUpdate = Object.FindObjectsOfType<GravityComponent>().ToList();
        foreach (var gravityComponent in _componentsToUpdate)
        {
            if (gravityComponent.isGrounded)
                continue;
            
            if (!CanMoveBlockDownward(gravityComponent))
            {
                gravityComponent.isGrounded = true;
                continue;
            }
            
            var pos = gravityComponent.transform.position;
            pos.y -= Settings.gravity;
            gravityComponent.transform.position = pos;
            
            UpdateWorldMatrix(gravityComponent);
        }
    }

    private void UpdateWorldMatrix(GravityComponent gravityComponent)
    {
        var positionComponents = GetPositionComponents(gravityComponent);
        foreach (var positionComponent in positionComponents)
        {
            var initPos = positionComponent.position;
            var newPos = positionComponent.position;
            newPos.y -= GravityStep;
            GameWorld.WorldMatrix[initPos.x, initPos.y, initPos.z] = false;
            GameWorld.WorldMatrix[newPos.x, newPos.y, newPos.z] = true;
            positionComponent.position = newPos;
        }
    }

    private bool CanMoveBlockDownward(GravityComponent gravityComponent)
    {
        var positionComponents = GetPositionComponents(gravityComponent);
        return CanMoveDownward(positionComponents);
    }

    private List<PositionComponent> GetPositionComponents(GravityComponent gravityComponent)
    {
        var positionComponents = new List<PositionComponent>();
        if (gravityComponent.composite)
        {
            positionComponents = gravityComponent.GetComponentsInChildren<PositionComponent>()
                .Where(c => c.gameObject != gravityComponent.gameObject).ToList();
        }
        else
        {
            positionComponents.Add(gravityComponent.GetComponent<PositionComponent>());
        }

        return positionComponents;
    }

    private bool CanMoveDownward(List<PositionComponent> positionComponents)
    {
        foreach (var positionComponent in positionComponents)
        {
            var newPos = positionComponent.position;
            newPos.y -= GravityStep;
            if (newPos.y < 0)
                return false;

            if (newPos.y >= GameWorld.H)
                return false;

            if (GameWorld.WorldMatrix[newPos.x, newPos.y, newPos.z])
            {
                if (positionComponents.All(p => p.position != newPos))
                    return false;
            }
        }

        return true;
    }
}