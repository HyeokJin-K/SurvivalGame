
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContainer
{
    public SpawnState SpawnState { get; private set; }
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public AttackState AttackState { get; private set; }
    public RollState RollState { get; private set; }
    public DeadState DeadState { get; private set; }
    
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();

    public PlayerStateContainer(PlayerHandlerContext context)
    {
        var spawnState = new SpawnState(context);
        var idleState = new IdleState(context);
        var moveState = new MoveState(context);
        var attackState = new AttackState(context);
        var rollState = new RollState(context);
        var deadState = new DeadState(context);
        
        SpawnState = spawnState;
        IdleState = idleState;
        MoveState = moveState;
        AttackState = attackState;
        RollState = rollState;
        DeadState = deadState;
        
        _states.Add(typeof(SpawnState),SpawnState);
        _states.Add(typeof(IdleState),IdleState);
        _states.Add(typeof(MoveState),MoveState);
        _states.Add(typeof(AttackState),AttackState);
        _states.Add(typeof(RollState),RollState);
        _states.Add(typeof(DeadState),DeadState);
    }

    public State GetState(Type stateType, AttackInputType attackInputType = AttackInputType.None)
    {
        if (_states.TryGetValue(stateType, out var state))
        {
            if (state is AttackState attackState)
            {
                attackState.SetAttackInput(attackInputType);
                return attackState;
            }
            else
            {   
                return state;
            }
        }
        else
        {
            return null;
        }
    }
}

