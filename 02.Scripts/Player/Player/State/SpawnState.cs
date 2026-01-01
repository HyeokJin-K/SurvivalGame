
using UnityEngine;

public class SpawnState : State
{
    public override bool CanRotate { get; protected set; } = false;
    public override bool CanMove { get; protected set; } = false;
    public override bool CanAttack { get; protected set; } = false;
    public override bool CanRoll { get; protected set; } = false;

    private float _spawnTime = 1f;
    
    public SpawnState(PlayerHandlerContext playerHandlerContext) : base(playerHandlerContext)
    {
        this.CurrentPlayerHandlerContext = playerHandlerContext;
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
        _spawnTime -= Time.deltaTime;

        if (_spawnTime <= 0f)
        {
            CurrentPlayerHandlerContext.playerStatsHandler.ChangePlayerState(typeof(IdleState));
        }
    }

    protected override void Init()
    {
        CanRotate = false;
        CanMove = false;
        CanAttack = false;
        CanRoll = false;

        _spawnTime = 1f;
    }
}

