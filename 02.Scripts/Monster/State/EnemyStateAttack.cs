using UnityEngine;
using UnityEngine.AI;
using static Constants;

public class EnemyStateAttack : EnemyState, ICharacterState
{
    private bool _played;

    public EnemyStateAttack(EnemyController enemyController, Animator animator, NavMeshAgent navMeshAgent)
        : base(enemyController, animator, navMeshAgent) { }

    public void Enter()
    {
        _played = false;
        _navMeshAgent.isStopped = true;
        _enemyController.FaceTarget();
        
        _animator.ResetTrigger(EnemyAniParamAttack);
        _animator.SetTrigger(EnemyAniParamAttack);

        _enemyController.MarkAttackUsed();
    }

    public void Update()
    {
        var target = _enemyController.Target;
        if (!target)
        {
            _enemyController.SetState(EEnemyState.Idle);
            return;
        }

        _enemyController.FaceTarget();

        var st = _animator.GetCurrentAnimatorStateInfo(0);
        if (st.IsTag("Attack"))
            _played = true;

        if (_played && !st.IsTag("Attack"))
        {
            if (_enemyController.IsTargetInAttackRange())
                _enemyController.SetState(EEnemyState.Attack);
            else
                _enemyController.SetState(EEnemyState.Patrol);
        }
    }

    public void Exit()
    {
        _navMeshAgent.isStopped = false;
    }
}