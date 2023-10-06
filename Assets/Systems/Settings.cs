using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Settings
{
    public static int gravity = 1;
    public static int movementSpeed = 1;
    public static int boxScale = 2;
    public static Dictionary<KeyCode, Axis> keyToRotationAxis = new Dictionary<KeyCode, Axis>()
    {
        { InputSystem.RotationUp, Axis.X },
        { InputSystem.RotationLeft, Axis.Y },
        { InputSystem.RotationRight, Axis.Z },
    };
    
    public static Dictionary<KeyCode, Vector3Int> keyToMovementDirection = new Dictionary<KeyCode, Vector3Int>()
    {
        {InputSystem.MoveForward,Vector3Int.forward },
        { InputSystem.MoveBackward, Vector3Int.back },
        { InputSystem.MoveRight, Vector3Int.right },
        { InputSystem.MoveLeft, Vector3Int.left },
    };
    
    
}

public enum Axis
{
    Undefined,
    X,
    Y,
    Z
}
