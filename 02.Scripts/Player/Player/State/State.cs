
public abstract class State
{
    public abstract bool CanRotate { get; protected set; }
    public abstract bool CanMove { get; protected set; }
    public abstract bool CanAttack { get; protected set; }
    public abstract bool CanRoll { get; protected set; }
    protected PlayerHandlerContext CurrentPlayerHandlerContext { get; set; }

    protected State(PlayerHandlerContext playerHandlerContext)
    {
        CurrentPlayerHandlerContext = playerHandlerContext;
    }
    

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    
    protected abstract void Init();
}

