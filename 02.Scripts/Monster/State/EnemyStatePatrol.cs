using UnityEngine;
using UnityEngine.AI;
using static Constants;

public class EnemyStatePatrol : EnemyState, ICharacterState
{
    private float _waitTime;

    public EnemyStatePatrol(EnemyController enemyController, Animator animator, NavMeshAgent navMeshAgent)
        : base(enemyController, animator, navMeshAgent) { }

    public void Enter()
    {
        _waitTime = 0f;
        _navMeshAgent.isStopped = false;
        _animator.SetBool(EnemyAniParamPatrol, true);
    }

    public void Update()
    {
        var detectionTargetTransform = _enemyController.DetectionTargetInCircle();
        if (detectionTargetTransform && _waitTime > _enemyController.ChaseWaitTime)
        {
            _navMeshAgent.SetDestination(detectionTargetTransform.position);
            _enemyController.SetState(EEnemyState.Chase);
        }
        else if (!_navMeshAgent.pathPending &&
                 _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _enemyController.SetState(EEnemyState.Idle);
        }

        float speed01 = 0f;
        if (_navMeshAgent.speed > 0.01f)
            speed01 = _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
        _animator.SetFloat(EnemyAniParamMoveSpeed, speed01);

        _waitTime += Time.deltaTime;
    }

    public void Exit()
    {
        _animator.SetBool(EnemyAniParamPatrol, false);
    }
}