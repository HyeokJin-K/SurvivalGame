using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Constants;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour, IDamagable
{
    public EEnemyState State;

    [SerializeField] protected MonsterStats _stats;
    [SerializeField] protected EnemyAttackVFX attackVfx;
    [SerializeField] protected EnemySfxController sfxController;
    [SerializeField] protected float currentHP;
    private Animator anim;
    private NavMeshAgent agent;
    [SerializeField] private Transform _targetTransform;
    public Transform Target => _targetTransform;

    [SerializeField] protected float maxHP;
    [SerializeField] protected float damage;

    [SerializeField] protected float patrolWaitTime = 1f;
    [SerializeField] protected float patrolChance = 30f;
    [SerializeField] protected float patrolDetectionDistance = 10f;
    [SerializeField] protected LayerMask detactionTargetLayerMask;
    [SerializeField] protected float chaseWaitTime = 1f;
    [SerializeField] protected float turnSpeed = 15f;
    [SerializeField] protected float detectionSightAngle = 30f;
    [SerializeField] protected float attackCooldown = 1.0f;
    [SerializeField] protected float attackRange = 2.0f;
    [SerializeField] protected float attackAngle = 60f;

    public float ChaseWaitTime => chaseWaitTime;
    public float PatrolWaitTime => patrolWaitTime;
    public float PatrolChance => patrolChance;
    public float PatrolDetectionDistance => patrolDetectionDistance;
    public float Damage => damage;

    private float _nextAttackTime;
    private Collider[] _detectionResults = new Collider[1];
    private Dictionary<EEnemyState, ICharacterState> _states;

    private void Init()
    {
        damage = _stats.damage;
        maxHP = _stats.maxHP;
        currentHP = maxHP;
        patrolWaitTime = _stats.patrolWaitTime;
        patrolChance = _stats.patrolChance;
        patrolDetectionDistance = _stats.patrolDetectionDistance;
        detectionSightAngle = _stats.detectionSightAngle;
        chaseWaitTime = _stats.chaseWaitTime;
        turnSpeed = _stats.turnSpeed;
        attackCooldown = _stats.attackCooldown;
        attackRange = _stats.attackRange;
        attackAngle = _stats.attackAngle;
        detactionTargetLayerMask = _stats.detectionTargetLayerMask;
        agent.stoppingDistance = attackRange;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
       
        _targetTransform = null;

        var enemyStateIdle = new EnemyStateIdle(this, anim, agent);
        var enemyStatePatrol = new EnemyStatePatrol(this, anim, agent);
        var enemyStateChase = new EnemyStateChase(this, anim, agent);
        var enemyStateAttack = new EnemyStateAttack(this, anim, agent);
        var enemyStateHit = new EnemyStateHit(this, anim, agent);
        var enemyStateDead = new EnemyStateDead(this, anim, agent);

        _states = new Dictionary<EEnemyState, ICharacterState>
        {
            { EEnemyState.Idle, enemyStateIdle },
            { EEnemyState.Patrol, enemyStatePatrol },
            { EEnemyState.Chase, enemyStateChase },
            { EEnemyState.Attack, enemyStateAttack },
            { EEnemyState.Hit, enemyStateHit },
            { EEnemyState.Dead, enemyStateDead }
        };

        SetState(EEnemyState.Idle);
    }

    private void Start()
    {
        if (_stats == null)
        {
            Debug.LogError($"{name}: MonsterStats is null");
            enabled = false;
            return;
        }

        Init();
        currentHP = maxHP;

        if (agent)
            agent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        if (State != EEnemyState.Dead && State != EEnemyState.None)
            _states[State].Update();
    }

    public void SetState(EEnemyState state)
    {
        if (State == state) return;
        if (State == EEnemyState.Dead) return;
        if (State != EEnemyState.None) _states[State].Exit();
        State = state;
        if (State != EEnemyState.None) _states[State].Enter();
    }

    public void TakeDamage(float damage)
    {
        if (State == EEnemyState.Dead) return;
        currentHP -= damage;
        attackVfx.PlayHit(_targetTransform.position);
        sfxController.PlayHit();
        
        SetState(EEnemyState.Hit);
        if (currentHP <= 0)
        {
            currentHP = 0f;
            SetState(EEnemyState.Dead);
            Destroy(gameObject, 6f);
        }
    }

    public Transform DetectionTargetInCircle()
    {
        if (!_targetTransform)
        {
            Physics.OverlapSphereNonAlloc(transform.position, patrolDetectionDistance, _detectionResults, detactionTargetLayerMask);
            _targetTransform = _detectionResults[0]?.transform;
        }
        else
        {
            float playerDistance = Vector3.Distance(transform.position, _targetTransform.position);
            if (playerDistance > patrolDetectionDistance)
            {
                _targetTransform = null;
                _detectionResults[0] = null;
            }
        }
        return _targetTransform;
    }

    public bool IsTargetInAttackRange()
    {
        if (!_targetTransform) return false;
        Vector3 to = _targetTransform.position - transform.position;
        float dist = to.magnitude;
        if (dist > attackRange) return false;
        float ang = Vector3.Angle(transform.forward, to);
        return ang <= (attackAngle * 0.5f);
    }

    public void MarkAttackUsed() => _nextAttackTime = Time.time + attackCooldown;

    public void FaceTarget()
    {
        if (!_targetTransform) return;
        Vector3 dir = _targetTransform.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        var rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolDetectionDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Vector3 rightDirection = Quaternion.Euler(0, detectionSightAngle, 0) * transform.forward;
        Vector3 leftDirection = Quaternion.Euler(0, -detectionSightAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rightDirection * patrolDetectionDistance);
        Gizmos.DrawRay(transform.position, leftDirection * patrolDetectionDistance);
        Gizmos.DrawRay(transform.position, transform.forward * patrolDetectionDistance);

        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(agent.destination, 0.5f);
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }
}
