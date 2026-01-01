using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementHandler : PlayerHandler
{
    private readonly Camera _playerCamera;
    private readonly CharacterController _characterController;

    private PlayerPhysicsConfig _playerPhysicsConfig;
    private PlayerHandlerContext _playerHandlerContext;
    private MoveContext _moveContext;

    private Vector3 _rollVelocity;
    private Quaternion _rollDirection;
    private Vector3 _externalVelocity;
    private Vector3 _gravityVelocity;
    private float _currentSpeedMultiplier;

    public PlayerMovementHandler(PlayerEvents playerEvents, Camera playerCamera,
        CharacterController characterController,
        PlayerPhysicsConfig playerPhysicsConfig) : base(playerEvents)
    {
        PlayerEvents = playerEvents;
        _playerCamera = playerCamera;
        _characterController = characterController;
        _playerPhysicsConfig = playerPhysicsConfig;
    }

    #region Handler LifeCycle

    public override void HandlerFixedUpdate()
    {
        ApplyGravity();
        ApplyExternalMovement();
        ApplyInclineMovement();
        ApplyRollMovement();
        ApplyRotate();
        ApplyMovement();
    }

    #endregion

    public void SetMoveContext(MoveContext context)
    {
        _moveContext = context;
    }

    public void SetPlayerHandlerContext(PlayerHandlerContext context)
    {
        _playerHandlerContext = context;
    }

    public void SetPlayerRotation(Vector2 inputDir)
    {
        if (inputDir == Vector2.zero)
        {
            _rollDirection = _characterController.transform.rotation;
        }
        else
        {
            _rollDirection = GetRotateDir(inputDir);
            _characterController.transform.rotation = _rollDirection;
        }
        
        _rollVelocity = _characterController.transform.forward * _playerPhysicsConfig.rollSpeed;
    }
    
    private void ApplyGravity()
    {
        if (!_playerPhysicsConfig.useGravity) return;
        if (_characterController.isGrounded) _gravityVelocity.y = -2f;

        _gravityVelocity.y -= _playerPhysicsConfig.gravity * Time.fixedDeltaTime;
        _gravityVelocity.y = Mathf.Max(_gravityVelocity.y, -_playerPhysicsConfig.terminalVelocityY);
        _characterController.Move(_gravityVelocity * Time.fixedDeltaTime);

        // if (_characterController.isGrounded)
        // {
        //     if (_isJump)
        //     {
        //         _gravityVelocity.y = 15f;
        //         _isJump = false;
        //     }
        //     else
        //     {
        //         _gravityVelocity.y = -2f;
        //     }
        // }
        // else
        // {
        //     _gravityVelocity.y -= gravity * Time.fixedDeltaTime;
        //     _gravityVelocity.y = Mathf.Max(_gravityVelocity.y, -terminalVelocityY);
        // }
    }

    private void ApplyInclineMovement()
    {
        if(!_characterController.isGrounded) return;

        if (Physics.Raycast(_characterController.transform.position, Vector3.down,
                out RaycastHit hit, 1f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle > _characterController.slopeLimit)
            {
                var velcity = hit.normal;
                velcity.y = -0.1f;
                
                _characterController.Move(velcity * Time.fixedDeltaTime);
            }
        }
    }
    
    private void ApplyMovement()
    {
        var currentState = _playerHandlerContext.playerStatsHandler.CurrentPlayerState;
        var inputHandler = _playerHandlerContext.playerInputHandler;

        if (currentState == null || inputHandler == null) return;

        if (!currentState.CanMove)
        {
            _currentSpeedMultiplier = 0f;
            return;
        }

        if (inputHandler.InputDir.sqrMagnitude < 0.01f)
        {
            _currentSpeedMultiplier = Mathf.Lerp(_currentSpeedMultiplier, 0f,
                _playerPhysicsConfig.decelerationSpeed * Time.fixedDeltaTime);


            if (_currentSpeedMultiplier < 0.01f)
            {
                _currentSpeedMultiplier = 0f;
                return;
            }
        }
        else
        {
            float targetMultiplier = inputHandler.IsSprintBuffer ? _moveContext.SprintMultiplier : 1f;
            float accelerationRate = inputHandler.IsSprintBuffer
                ? _playerPhysicsConfig.sprintAcceleration
                : _playerPhysicsConfig.normalAcceleration;

            _currentSpeedMultiplier = Mathf.Lerp(_currentSpeedMultiplier, targetMultiplier,
                accelerationRate * Time.fixedDeltaTime);
        }

        var inputDirToVelocity = _characterController.transform.forward * _moveContext.MoveSpeed;
        inputDirToVelocity *= _currentSpeedMultiplier;
        _characterController.Move(inputDirToVelocity * Time.fixedDeltaTime);
    }

    private void ApplyExternalMovement()
    {
        if (_externalVelocity.sqrMagnitude < 0.01f) return;

        _characterController.Move(_externalVelocity * Time.fixedDeltaTime);
    }

    private void ApplyRotate()
    {
        if (!_playerCamera) return;

        var currentState = _playerHandlerContext.playerStatsHandler.CurrentPlayerState;

        if (currentState == null) return;

        if (!currentState.CanRotate) return;

        Quaternion targetRotation = GetRotateDir(_playerHandlerContext.playerInputHandler.InputDir);

        if (_playerPhysicsConfig.doLerpRotate)
        {
            var rotateSpeed = _playerPhysicsConfig.lerpRotateSpeed;
            if (currentState is AttackState)
            {
                rotateSpeed *= _playerPhysicsConfig.attackRotateSpeedDeclinedRate;
            }

            float t = Mathf.Clamp01(rotateSpeed * Time.fixedDeltaTime);

            _characterController.transform.rotation = Quaternion.Slerp(_characterController.transform.rotation,
                targetRotation, t);
        }
        else
        {
            _characterController.transform.rotation = targetRotation;
        }
    }

    private void ApplyRollMovement()
    {
        if (_rollVelocity.sqrMagnitude < 0.01f ||
            _playerHandlerContext.playerStatsHandler.CurrentPlayerState is not RollState) return;
        
        _characterController.transform.rotation = _rollDirection;
        
        _characterController.Move(_rollVelocity * Time.fixedDeltaTime);

        _rollVelocity = Vector3.Lerp(_rollVelocity, Vector3.zero,
            _playerPhysicsConfig.rollDecelerationSpeed * Time.fixedDeltaTime);
    }

    public Quaternion GetRotateDir(Vector2 inputDir)
    {
        if (inputDir.sqrMagnitude < 0.01f) return _characterController.transform.rotation;

        Vector3 cameraForward = _playerCamera.transform.forward;
        Vector3 cameraRight = _playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 rotateDir = (cameraForward * inputDir.y + cameraRight * inputDir.x)
            .normalized;

        Quaternion targetRotation = Quaternion.LookRotation(rotateDir);

        return targetRotation;
    }
}