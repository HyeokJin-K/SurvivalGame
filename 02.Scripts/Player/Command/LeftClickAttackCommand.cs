
using UnityEngine;
using static AttackIdTable;

public class LeftClickAttackCommand : IPlayerActionCommand
{
    public float TimeStamp { get; private set; } = Time.time;

    public void Execute(PlayerHandlerContext context)
    {
        var statsHandler = context.playerStatsHandler;

        if (statsHandler.CurrentPlayerState is AttackState attackState)
        {
            attackState.SetAttackInput(AttackInputType.LeftClick);
        }
        else
        {
            statsHandler.ChangePlayerState(typeof(AttackState), AttackInputType.LeftClick);
        }
    }

    public bool CanExecute(PlayerHandlerContext context)
    {
        var currentState = context.playerStatsHandler.CurrentPlayerState;

        if (currentState == null) return false;
        
        var hasAttack = context.playerCombatHandler.HasAttack(AttackInputType.LeftClick);
        return currentState.CanAttack && hasAttack;
    }
}

