
using UnityEngine;

public class SpaceBarRollCommand : IPlayerActionCommand
{
    public float TimeStamp { get; } = Time.time;
    
    public void Execute(PlayerHandlerContext handlerContext)
    {
        var statsHandler = handlerContext.playerStatsHandler;
        statsHandler.ChangePlayerState(typeof(RollState));
    }

    public bool CanExecute(PlayerHandlerContext handlerContext)
    {
        var currentState = handlerContext.playerStatsHandler.CurrentPlayerState;
        return currentState.CanRoll;
    }
}

