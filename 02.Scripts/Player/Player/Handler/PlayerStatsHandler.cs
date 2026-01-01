using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerStatsHandler : PlayerHandler
{
    private readonly PlayerData _playerOriginData;
    private PlayerHandlerContext _playerHandlerContext;
    public PlayerData CurrentPlayerData { get; private set; }
    public PlayerStateContainer PlayerStateContainer { get; private set; }
    private State _currentPlayerState;

    public State CurrentPlayerState
    {
        get => _currentPlayerState;
        set
        {
            if (_currentPlayerState is DeadState) return;

            CurrentPlayerState?.Exit();
            _currentPlayerState = value;
            CurrentPlayerState?.Enter();
            PlayerEvents.OnStateChanged?.Invoke(CurrentPlayerState);
        }
    }


    public PlayerStatsHandler(PlayerEvents playerEvents, PlayerData playerData) : base(playerEvents)
    {
        PlayerEvents = playerEvents;
        _playerOriginData = playerData;
    }

    #region Handler LifeCycle

    public override void HandlerEnable()
    {
        base.HandlerEnable();
        CurrentPlayerData = _playerOriginData.Clone();
        PlayerStateContainer = new PlayerStateContainer(_playerHandlerContext);
        CurrentPlayerData.InitPlayerData(PlayerEvents);
        ChangePlayerState(typeof(SpawnState));

        PlayerEvents.OnDeath.AddListener(ChangeDeadState);
    }

    public override void HandlerUpdate()
    {
        CurrentPlayerState?.Update();
    }

    public override void HandlerDisable()
    {
        base.HandlerDisable();
        PlayerEvents.OnDeath.RemoveListener(ChangeDeadState);
    }

    #endregion

    public void SetPlayerHandlerContext(PlayerHandlerContext playerContext)
    {
        _playerHandlerContext = playerContext;
    }

    public void ChangePlayerState(Type stateType, AttackInputType attackInputType = AttackInputType.None)
    {
        var newState = PlayerStateContainer.GetState(stateType, attackInputType);
        CurrentPlayerState = newState;
    }

    public bool ConsumeItem(ItemData item)
    {
        if (!CanUseItem()) return false;
        
        //아이템 적용
        PlayerEvents.OnUseItem?.Invoke(item);
        return true;
    }

    private bool CanUseItem()
    {
        return CurrentPlayerState is not (DeadState or SpawnState
            or RollState or AttackState);
    }

    private void ChangeDeadState()
    {
        CurrentPlayerState = PlayerStateContainer.GetState(typeof(DeadState));
    }
}