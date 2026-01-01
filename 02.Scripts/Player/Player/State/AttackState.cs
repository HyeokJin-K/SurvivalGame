
using UnityEngine;

public class AttackState : State
{
    public override bool CanRotate { get; protected set; } = false;
    public override bool CanMove { get; protected set; } = false;
    public override bool CanAttack { get; protected set; } = true;
    public override bool CanRoll { get; protected set; } = false;
    
    private AttackInputType _attackInput;
    private float _rotateTime = 0f;

    public AttackState(PlayerHandlerContext playerHandlerContext) : base(playerHandlerContext)
    {
        CurrentPlayerHandlerContext = playerHandlerContext;
    }
    
    public override void Enter()
    {
        Init();
        
        if (CurrentPlayerHandlerContext.playerCombatHandler.RequestAttack(_attackInput, out var attackId))
        {
            CurrentPlayerHandlerContext.playerAnimatorHandler.SetAttack(attackId);
            CurrentPlayerHandlerContext.playerAnimatorHandler.SetAttackSpeed(CurrentPlayerHandlerContext.playerStatsHandler.
                CurrentPlayerData.AttackSpeed);
            CanAttack = false;
            InitAttackInput();
        }
        else
        {
            var idleState = CurrentPlayerHandlerContext.playerStatsHandler.PlayerStateContainer.IdleState;
            CurrentPlayerHandlerContext.playerStatsHandler.CurrentPlayerState = idleState;
        }
    }

    public override void Exit()
    {
        CurrentPlayerHandlerContext.playerInputHandler.AttackCommandBuffer.ClearBuffer();
        CurrentPlayerHandlerContext.playerCombatHandler.ResetAttack();
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetAttackEnd();
    }

    public override void Update()
    {
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetMove(Vector3.zero);

        if (_rotateTime > 0f)
        {
            _rotateTime -= Time.deltaTime;
        }
        else
        {
            CanRotate = false;
        }
        
        if(!CanAttack) return;
        
        if(_attackInput == AttackInputType.None) return;
        
        if (CurrentPlayerHandlerContext.playerCombatHandler.RequestAttack(_attackInput, out var attackId))
        {
            CurrentPlayerHandlerContext.playerAnimatorHandler.SetAttack(attackId);
            CanAttack = false;
            CanRoll = false;
            DoRotateForSeconds(0.15f);
            InitAttackInput();
        }
        else
        {
            _rotateTime = 0f;
            InitAttackInput();
        }
    }

    public void DoRotateForSeconds(float seconds)
    {
        CanRotate = true;
        _rotateTime = seconds;
    }
    
    public void SetAttackInput(AttackInputType inputType)
    {
        _attackInput = inputType;
    }

    public void ComboWindowOpen()
    {
        CanAttack = true;
        CanRoll = true;
    }

    public void ComboWindowClose()
    {
        CanAttack = false;
        CanRoll = false;
    }

    public void AttackAnimationComplete()
    {
        CurrentPlayerHandlerContext.playerAnimatorHandler.SetAttackEnd();
        CurrentPlayerHandlerContext.playerStatsHandler.ChangePlayerState(typeof(IdleState));
        CurrentPlayerHandlerContext.playerCombatHandler.ResetAttack();
    }
    
    protected override void Init()
    {
        CanRotate = false;
        CanMove = false;
        CanAttack = true;
        CanRoll = false;
        
        _rotateTime = 0f;
    }
    
    private void InitAttackInput()
    {
        _attackInput = AttackInputType.None;
    }

}

