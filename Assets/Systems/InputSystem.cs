using UnityEngine;

public class InputSystem : ISystem
{
    public const KeyCode MoveRight = KeyCode.RightArrow;
    public const KeyCode MoveLeft = KeyCode.LeftArrow;
    
    public const KeyCode RotateKey = KeyCode.UpArrow;
    public const KeyCode RotateCameraKey = KeyCode.DownArrow;
    
    private bool _rotationKeyPressed;
    private KeyCode? _lastMoveKey = null;
    private KeyCode? _cameraRotationRequest = null;

    public void OnFrameUpdate()
    {
        if (Input.GetKeyUp(RotateKey)) _rotationKeyPressed = true;
     
        if (Input.GetKeyUp(MoveRight)) _lastMoveKey = MoveRight;
        if (Input.GetKeyUp(MoveLeft)) _lastMoveKey = MoveLeft;
        
        if (Input.GetKeyUp(RotateCameraKey)) _cameraRotationRequest = RotateCameraKey;
    }

    public void OnSystemUpdate()
    {
        BlackBoard.rotationKeyPressed = _rotationKeyPressed;
        BlackBoard.lastMoveKey = _lastMoveKey;
        BlackBoard.cameraRotationKeyPressed = _cameraRotationRequest != null;
        
        _rotationKeyPressed = false;
        _lastMoveKey = null;
        _cameraRotationRequest = null;
    }
}