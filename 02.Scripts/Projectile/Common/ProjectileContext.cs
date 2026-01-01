using UnityEngine;

public struct ProjectileContext
{
    public Transform shooter;   // 사수(아군/자기충돌 무시용)
    public Vector3 position;    // 시작 위치
    public Vector3 direction;   // 정규화 방향 (호밍이면 초기 방향)
    public Transform target;    // 호밍 타깃
}