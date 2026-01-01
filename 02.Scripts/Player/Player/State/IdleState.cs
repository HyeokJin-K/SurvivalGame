
using UnityEngine;

public class IdleState : State
{
    public override bool CanRotate { get; protected set; } = true;
    public override bool CanMove { get; protected set; } = true;
    public override bool CanAttack { get; protected set; } = true;
    public override bool CanRoll { get; protected set; } = true;

    public IdleState(PlayerHandlerContext handlerContext) : base(handlerContext)
    {
        CurrentPlayerHandlerContext = handlerContext;
    }
    
    public override void Enter()
    {
        Init();
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        var inputDir = CurrentPlayerHandlerContext.playerInputHandler.InputDir;

        if (inputDir.sqrMagnitude >= 0.01f)
        {
            CurrentPlayerHandlerContext.playerStatsHandler.ChangePlayerState(typeof(MoveState));
        }
        
        CurrentPlayerHandlerContext.playerAnimatorHandler?.SetMove(Vector3.zero);
    }

    protected override void Init()
    {
        CanRotate = true;
        CanMove = true;
        CanAttack = true;
        CanRoll = true;
    }
}

