
using UnityEngine;

public class RollState : State
{
    public override bool CanRotate { get; protected set; } = false;
    public override bool CanMove { get; protected set; } = false;
    public override bool CanAttack { get; protected set; } = false;
    public override bool CanRoll { get; protected set; } = false;
    
    public RollState(PlayerHandlerContext playerHandlerContext) : base(playerHandlerContext)
    {
        CurrentPlayerHandlerContext = playerHandlerContext;
    }
    
    public override void Enter()
    {
        Init();
        
        CurrentPlayerHandlerContext.playerMovementHandler.SetPlayerRotation(CurrentPlayerHandlerContext.playerInputHandler.InputDir);
        //CurrentPlayerHandlerContext.playerAnimatorHandler.SetMoveBlend(0f);
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetRoll();
    }

    public override void Exit()
    {
        CurrentPlayerHandlerContext.playerInputHandler.RollCommandBuffer.ClearBuffer();
        CurrentPlayerHandlerContext.playerInputHandler.AttackCommandBuffer.ClearBuffer();
    }

    public override void Update()
    {
    }
    
    public void RollAnimationComplete()
    {
        CurrentPlayerHandlerContext.playerStatsHandler.ChangePlayerState(typeof(IdleState));
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetRollEnd();
    }

    protected override void Init()
    {
        CanRotate = false;
        CanMove = false;
        CanAttack = false;
        CanRoll = false;
    }
}

