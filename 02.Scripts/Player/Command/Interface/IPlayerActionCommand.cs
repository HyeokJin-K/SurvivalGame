
public interface IPlayerActionCommand
{
    public float TimeStamp {get;}
    public void Execute(PlayerHandlerContext handlerContext);
    public bool CanExecute(PlayerHandlerContext handlerContext);
    
}
