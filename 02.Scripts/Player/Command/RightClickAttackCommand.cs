
using UnityEngine;
using static AttackIdTable;

public class RightClickAttackCommand : IPlayerActionCommand
{
    public float TimeStamp { get; private set; } = Time.time;

    public void Execute(PlayerHandlerContext context)
    {
        var statsHandler = context.playerStatsHandler;

        if (statsHandler.CurrentPlayerState is AttackState attackState)
        {
            attackState.SetAttackInput(AttackInputType.RightClick);
        }
        else
        {
            statsHandler.ChangePlayerState(typeof(AttackState), AttackInputType.RightClick);
        }
    }

    public bool CanExecute(PlayerHandlerContext context)
    {
        var currentState = context.playerStatsHandler.CurrentPlayerState;
        
        if(currentState == null) return false;
        
        var hasAttack = context.playerCombatHandler.HasAttack(AttackInputType.RightClick);
        return currentState.CanAttack && hasAttack;
    }
}

