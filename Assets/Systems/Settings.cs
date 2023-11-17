using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static int gravity = 1;
    public static int movementSpeed = 1;
    
    public static Dictionary<CameraPosition, (Axis axis, int sign)> cameraPosToRotationAxis =
        new Dictionary<CameraPosition, (Axis axis, int sign)>()
        {
            { CameraPosition.Front, (Axis.Z, 1) },
            { CameraPosition.Right, (Axis.X, 1) },
            { CameraPosition.Back, (Axis.Z, -1) },
            { CameraPosition.Left, (Axis.X, -1) },
        };
}

public enum Axis
{
    Undefined,
    X,
    Y,
    Z,
}
