using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UpdateSystem : MonoBehaviour
{
    public float updateDelayInSeconds;
    
    private float _updateSecondsCounter;
    
    [FormerlySerializedAs("_blockGenerationSystem")] [SerializeField] private BlockGenerationSystem blockGenerationSystem;
    private ISystem _inputSystem;
    private ISystem _gravitySystem;
    private ISystem _rotationSystem;
    private ISystem _movementSystem;
    private ISystem _blockDestructionSystem;
 
    private void Awake()
    {
        _inputSystem = new InputSystem();
        _gravitySystem = new GravitySystem();
        _rotationSystem = new RotationSystem();
        _movementSystem = new MovementSystem();
        _blockDestructionSystem = new BlockDestructionSystem();
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
        _gravitySystem.OnSystemUpdate();
        blockGenerationSystem.OnSystemUpdate();
        _blockDestructionSystem.OnSystemUpdate();
    }
}