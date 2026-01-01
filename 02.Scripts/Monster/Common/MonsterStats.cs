using UnityEngine;

[CreateAssetMenu(menuName = "Monster/MonsterStats")]
public class MonsterStats : ScriptableObject
{
    [Header("능력치")]
    [Min(1)] public float maxHP = 100f;
    [Min(0)] public float damage = 10f;

    [Header("이동")]
    [Range(0f, 50f)] public float turnSpeed = 15f;

    [Header("AI")]
    [Min(0)] public float patrolWaitTime = 1f;  // 순찰 포인트 도달 후 대기 시간
    [Range(0f, 100f)] public float patrolChance = 30f;  // Idle-Patrol 전환 확률
    [Min(0)] public float patrolDetectionDistance = 10f; // 탐지 반경
    [Range(0f, 180f)] public float detectionSightAngle = 60f; // 시야각
    [Min(0)] public float chaseWaitTime = 0.5f; // 추격 상태에서 딜레이
    [Min(0)] public float minimumRunDistance = 1f; // 목표와의 최소 거리(돌진/후퇴 등 판단)

    [Header("공격")]
    [Min(0)] public float attackCooldown = 1.0f; // 공격 쿨타임
    [Min(0)] public float attackRange = 2.0f; // 공격 범위
    [Range(0f, 180f)] public float attackAngle = 60f;

    [Header("레이어")]
    public LayerMask detectionTargetLayerMask;
}
