using System;
using System.Diagnostics.SymbolStore;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxCollisionComponent : MonoBehaviourComponent
{
    public bool isComposition;


    [SerializeField] private bool isGrounded;
    public bool isIntersectingWithWall;
    
    public bool isCollidingWithGround;
    public bool isCollidingWithWall;

    public bool IsGrounded
    {
        get
        {
            if (isComposition)
            {
                var childrenComponents = GetComponentsInChildren<BoxCollisionComponent>();
                if (childrenComponents!=null)
                {
                    var childrenResult =  childrenComponents.Any(c => c != this && c.isGrounded);
                    return childrenResult;
                }
            }

            return isGrounded;
        }
        set => isGrounded = value;
    }
}