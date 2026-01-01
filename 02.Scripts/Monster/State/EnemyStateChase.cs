using UnityEngine;
using UnityEngine.AI;
using static Constants;

public class EnemyStateChase : EnemyState, ICharacterState
{
    public EnemyStateChase(EnemyController enemyController, Animator animator, NavMeshAgent navMeshAgent)
        : base(enemyController, animator, navMeshAgent) { }

    public void Enter()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool(EnemyAniParamChase, true);
    }

    public void Update()
    {
        var target = _enemyController.DetectionTargetInCircle();
        if (!target)
        {
            _enemyController.SetState(EEnemyState.Patrol);
            return;
        }

        _navMeshAgent.SetDestination(target.position);
        _enemyController.FaceTarget();

        if (_enemyController.IsTargetInAttackRange())
        {
            _enemyController.SetState(EEnemyState.Attack);
            return;
        }
    }

    public void Exit()
    {
        _animator.SetBool(EnemyAniParamChase, false);
        _navMeshAgent.isStopped = true;
    }
}