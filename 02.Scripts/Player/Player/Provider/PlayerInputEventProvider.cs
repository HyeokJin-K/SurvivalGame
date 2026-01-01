
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEventProvider : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private PlayerInputHandler _playerInputHandler;

    #region Input Callback

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.MoveUpDownBuffer.Add(1);
        }

        if (context.canceled)
        {
            _playerInputHandler.MoveUpDownBuffer.Remove(1);
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.MoveUpDownBuffer.Add(-1);
        }

        if (context.canceled)
        {
            _playerInputHandler.MoveUpDownBuffer.Remove(-1);
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.MoveLeftRightBuffer.Add(-1);
        }

        if (context.canceled)
        {
            _playerInputHandler.MoveLeftRightBuffer.Remove(-1);
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.MoveLeftRightBuffer.Add(1);
        }

        if (context.canceled)
        {
            _playerInputHandler.MoveLeftRightBuffer.Remove(1);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.IsSprintBuffer = true;
        }

        if (context.canceled)
        {
            _playerInputHandler.IsSprintBuffer = false;
        }
    }

    public void OnLeftClickAttack(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.AttackCommandBuffer.AddCommand(new LeftClickAttackCommand());
        }
    }

    public void OnRightClickAttack(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        
        if (context.started)
        {
            _playerInputHandler.AttackCommandBuffer.AddCommand(new RightClickAttackCommand());
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;
        if (_playerInputHandler.InputDir == Vector2.zero) return; 
        
        if (context.started)
        {
            _playerInputHandler.RollCommandBuffer.AddCommand(new SpaceBarRollCommand());
        }
    }

    private int _index=0;
    
    public void OnWeaponChange(InputAction.CallbackContext context)
    {
        if(_playerInputHandler == null) return;

        if (context.started)
        {
            _index++;
            if (_index >= 3) _index = 1;
            player.ChangeWeapon(_index);
        }
    }

    #endregion
    
    public void SetPlayerInputHandler(PlayerInputHandler playerInputHandler)
    {
        _playerInputHandler = playerInputHandler;
    }
}

