using UnityEngine;

public class InputSystem : ISystem
{
    public const KeyCode RotationUp = KeyCode.UpArrow;
    public const KeyCode RotationDown = KeyCode.DownArrow;
    public const KeyCode RotationRight = KeyCode.RightArrow;
    public const KeyCode RotationLeft = KeyCode.LeftArrow;

    public const KeyCode MoveForward = KeyCode.W;
    public const KeyCode MoveBackward = KeyCode.S;
    public const KeyCode MoveRight = KeyCode.D;
    public const KeyCode MoveLeft = KeyCode.A;
    
    
    private KeyCode? _lastRotationKey = null;
    private KeyCode? _lastMoveKey = null;

    public void OnFrameUpdate()
    {
        if (Input.GetKeyUp(RotationUp)) _lastRotationKey = RotationUp;
        //if (Input.GetKeyUp(RotationDown)) _lastRotationKey = RotationDown;
        if (Input.GetKeyUp(RotationRight)) _lastRotationKey = RotationRight;
        if (Input.GetKeyUp(RotationLeft)) _lastRotationKey = RotationLeft;
        
        if (Input.GetKeyUp(MoveForward)) _lastMoveKey = MoveForward;
        if (Input.GetKeyUp(MoveBackward)) _lastMoveKey = MoveBackward;
        if (Input.GetKeyUp(MoveRight)) _lastMoveKey = MoveRight;
        if (Input.GetKeyUp(MoveLeft)) _lastMoveKey = MoveLeft;
    }

    public void OnSystemUpdate()
    {
        BlackBoard.lastRotationKey = _lastRotationKey;
        BlackBoard.lastMoveKey = _lastMoveKey;
        
        _lastRotationKey = null;
        _lastMoveKey = null;
    }
}