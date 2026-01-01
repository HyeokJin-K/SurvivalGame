
public interface IHandlerLifeCycleStart
{
    public void HandlerStart();
}

public interface IHandlerLifeCycleAwake
{
    public void HandlerAwake();
}

public interface IHandlerLifeCycleUpdate
{
    public void HandlerUpdate();
}

public interface IHandlerLifeCycleFixedUpdate
{
    public void HandlerFixedUpdate();
}

public interface IHandlerLifeCycleLateUpdate
{
    public void HandlerLateUpdate();
}

public interface IHandlerLifeCycleOnEnable
{
    public void HandlerEnable();
}

public interface IHandlerLifeCycleOnDisable
{
    public void HandlerDisable();
}

public interface IHandlerLifeCycleOnDestroy
{
    public void HandlerDestroy();
}
    