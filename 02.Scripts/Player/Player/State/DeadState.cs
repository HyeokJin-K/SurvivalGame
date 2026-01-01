
public class DeadState : State
{
    public override bool CanRotate { get; protected set; } = false;
    public override bool CanMove { get; protected set; } = false;
    public override bool CanAttack { get; protected set; } = false;
    public override bool CanRoll { get; protected set; } = false;

    public DeadState(PlayerHandlerContext playerHandlerContext) : base(playerHandlerContext)
    {
        CurrentPlayerHandlerContext = playerHandlerContext;       
    }
    public override void Enter()
    {
        Init();
        
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetDead();
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        
    }

    protected override void Init()
    {
        CanRotate = false;
        CanMove = false;
        CanAttack = false;
        CanRoll = false;
    }
}

