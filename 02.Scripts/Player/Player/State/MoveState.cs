using UnityEngine;

public class MoveState : State
{
    public override bool CanRotate { get; protected set; } = true;
    public override bool CanMove { get; protected set; } = true;
    public override bool CanAttack { get; protected set; } = true;
    public override bool CanRoll { get; protected set; } = true;

    public MoveState(PlayerHandlerContext handlerContext) : base(handlerContext)
    {
        CurrentPlayerHandlerContext = handlerContext;
    }
    
    public override void Enter()
    {
        Init();
        
        CurrentPlayerHandlerContext.playerStatsHandler.PlayerEvents.OnMoveStart?.Invoke();
    }

    public override void Exit()
    {
        CurrentPlayerHandlerContext.playerStatsHandler.PlayerEvents.OnMoveEnd?.Invoke();
    }

    public override void Update()
    {
        if (CurrentPlayerHandlerContext.playerInputHandler.InputDir.sqrMagnitude < 0.01f)
        {
            CurrentPlayerHandlerContext.playerStatsHandler.ChangePlayerState(typeof(IdleState));
        }

        CurrentPlayerHandlerContext.playerAnimatorHandler?.SetMove(
            CurrentPlayerHandlerContext.playerInputHandler.InputDir);
    }

    protected override void Init()
    {
        CanRotate = true;
        CanMove = true;
        CanAttack = true;
        CanRoll = true;
    }
}