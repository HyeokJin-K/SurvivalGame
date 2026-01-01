using System;
using System.Collections;
using UnityEngine;
using static AnimatorParameters;
using static AnimatorThresholds;

public class PlayerAnimatorHandler : PlayerHandler
{
    private readonly Animator _animator;
    private float _moveSqr;
    private float _currentThreshold;
    private bool _isAttacking;
    
    private PlayerHandlerContext _playerHandlerContext;

    public PlayerAnimatorHandler(PlayerEvents playerEvents, Animator animator) : base(playerEvents)
    {
        PlayerEvents = playerEvents;
        _animator = animator;
    }

    public void SetPlayerHandlerContext(PlayerHandlerContext playerHandlerContext)
    {
        _playerHandlerContext = playerHandlerContext;
    }

    public void SetMove(Vector3 moveInput)
    {
        _moveSqr = moveInput.sqrMagnitude;

        if (_moveSqr >= 0.01f)
        {
            _currentThreshold = _playerHandlerContext.playerInputHandler.IsSprintBuffer ? SprintThreshold : RunThreshold;
        }
        else
        {
            _currentThreshold = IdleThreshold;
        }

        if (_animator.GetFloat(MoveBlend) < 0.01f && _moveSqr < 0.01f)
        {
            _animator.SetFloat(MoveBlend, IdleThreshold);
            return;
        }
        
        _animator.SetFloat(MoveBlend, _currentThreshold, 0.1f, Time.deltaTime);
    }

    public void SetHit()
    {
        _animator.SetTrigger(IsHit);
    }

    public void SetAttack(int attackId)
    {
        _animator.SetInteger(AttackId, attackId);
        _animator.SetTrigger(IsAttack);
    }

    public void SetAttackEnd()
    {
        _animator.SetTrigger(IsAttackEnd);
    }

    public void SetDead()
    {
        _animator.SetTrigger(IsDead);
    }

    public void SetMoveBlend(float moveBlend)
    {
        _animator.SetFloat(MoveBlend, moveBlend);
    }

    public void SetWeaponType(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.OneHanded:
                _animator.SetBool(IsOneHanded, true);
                _animator.SetBool(IsTwoHanded, false);
                break;
            case WeaponType.TwoHanded:
                _animator.SetBool(IsTwoHanded, true);
                _animator.SetBool(IsOneHanded, false);
                break;
        }
    }

    public void SetRoll()
    {
        _animator.SetTrigger(IsRoll);
    }

    public void SetRollEnd()
    {
        _animator.SetTrigger(IsRollEnd);
    }

    public void SetAttackSpeed(float speed)
    {
        _animator.SetFloat(AttackSpeed, speed);
    }
}

public static class AnimatorParameters
{
    public static readonly int IsSprint = Animator.StringToHash("isSprint");
    public static readonly int IsMove = Animator.StringToHash("isMove");
    public static readonly int IsHit = Animator.StringToHash("isHit");
    public static readonly int IsAttack = Animator.StringToHash("isAttack");
    public static readonly int IsDead = Animator.StringToHash("isDead");
    public static readonly int IsRoll = Animator.StringToHash("isRoll");
    public static readonly int IsRollEnd = Animator.StringToHash("isRollEnd");
    public static readonly int MoveBlend = Animator.StringToHash("moveBlend");
    public static readonly int AttackId = Animator.StringToHash("attackId");
    public static readonly int IsAttackEnd = Animator.StringToHash("isAttackEnd");
    public static readonly int IsOneHanded = Animator.StringToHash("isOneHanded");
    public static readonly int IsTwoHanded = Animator.StringToHash("isTwoHanded");
    public static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");
}

public static class AnimatorThresholds
{
    public const float IdleThreshold = 0f;
    public const float WalkThreshold = 0.33f;
    public const float RunThreshold = 0.66f;
    public const float SprintThreshold = 1f;
}