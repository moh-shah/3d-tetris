using System;
using UnityEngine;

public class UpdateSystem : MonoBehaviour
{
    public float updateDelayInSeconds;
    
    private float _updateSecondsCounter;
    
    private ISystem _inputSystem;
    private ISystem _gravitySystem;
    private ISystem _rotationSystem;
    private ISystem _movementSystem;
    private ISystem _boxCollisionSystem;

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _gravitySystem = new GravitySystem();
        _rotationSystem = new RotationSystem();
        _movementSystem = new MovementSystem();
        _boxCollisionSystem = new BoxCollisionSystem();
    }

    private void Update()
    {
        _inputSystem.OnFrameUpdate();
        
        _updateSecondsCounter += Time.deltaTime;
        if (_updateSecondsCounter >= updateDelayInSeconds)
        {
            _updateSecondsCounter = 0;
            UpdateSystems();
        }
    }

    //systems update frequency must be different (eg: movement should be updated more frequently than gravity)
    private void UpdateSystems()
    {
        _inputSystem.OnSystemUpdate();
        _rotationSystem.OnSystemUpdate();
        _movementSystem.OnSystemUpdate();
        ///_gravitySystem.OnSystemUpdate();
    }
}