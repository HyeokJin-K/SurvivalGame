
public class PlayerHandler : IHandlerLifeCycleAwake, IHandlerLifeCycleStart, IHandlerLifeCycleUpdate,
    IHandlerLifeCycleFixedUpdate, IHandlerLifeCycleOnEnable, IHandlerLifeCycleOnDisable
{
    public PlayerEvents PlayerEvents { get; protected set; }

    protected PlayerHandler(PlayerEvents playerEvents)
    {
        PlayerEvents = playerEvents;
    }
    
    public virtual void HandlerAwake()
    {
    }

    public virtual void HandlerStart()
    {
    }

    public virtual void HandlerUpdate()
    {
        
    }

    public virtual void HandlerFixedUpdate()
    {
    }

    public virtual void HandlerEnable()
    {
    }

    public virtual void HandlerDisable()
    {
    }
}

