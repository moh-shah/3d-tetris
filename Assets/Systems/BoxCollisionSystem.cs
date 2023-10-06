using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxCollisionSystem : ISystem
{
    private List<BoxCollisionComponent> _componentsToUpdate;
    private LayerMask _layerMask = LayerMask.NameToLayer("Box");
    public void OnFrameUpdate()
    {
        
    }

    public void OnSystemUpdate()
    {
        _componentsToUpdate = Object.FindObjectsOfType<BoxCollisionComponent>().ToList();
        RaycastHit hit;
        foreach (var collisionComponent in _componentsToUpdate)
        {
            if (collisionComponent.isComposition || collisionComponent.IsGrounded)
                continue;
            
            var collider = collisionComponent.GetComponent<Collider>();
            var raycastResult = Physics.BoxCast(
                center: collider.bounds.center,
                halfExtents: collider.transform.localScale / 2f,
                direction: Vector3.down,
                out hit,
                Quaternion.identity,
                maxDistance: 2
                //,layerMask:_layerMask
            );
            if (raycastResult)
            {
                if (hit.transform.gameObject.CompareTag("Ground"))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    collisionComponent.IsGrounded = true;
                }
            }

        }
    }
}