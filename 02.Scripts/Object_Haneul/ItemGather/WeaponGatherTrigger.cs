using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 무기에 부착 + 채집 판정
/// </summary>
[RequireComponent(typeof(Collider))]
public class WeaponGatherTrigger : MonoBehaviour
{
    [Header("채집 설정")]
    [Tooltip("채집 가능한 레이어")]
    public LayerMask gatherableLayer;

    [Tooltip("한 번의 공격에 하나의 자원만 채집")]
    public bool oneResourcePerAttack = true;

    [Tooltip("채집 범위 (Trigger Collider 사용)")]
    public float gatherRange = 1.5f;

    [Header("디버그")] // 테스트용
    public bool showDebugLog = false;
    public bool showGizmos = true;

    private Collider weaponCollider;
    private bool isAttacking = false;
    private HashSet<GameObject> hitResources = new HashSet<GameObject>();  // 이번 공격에서 이미 타격한 자원들
    private ResourceObject currentTargetResource = null;  // 현재 타겟 자원

    void Awake()
    {
        // Collider 설정
        weaponCollider = GetComponent<Collider>();
        // weaponCollider.isTrigger = true;

        // 초기 비활성화 설정
        // weaponCollider.enabled = false;

        if (showDebugLog)
            Debug.Log($"{gameObject.name}: WeaponGatherTrigger 초기화");
    }

    /// <summary>
    /// 공격 시작 (애니메이션 이벤트에서 호출)
    /// </summary>
    public void OnAttackStart()
    {
        if (showDebugLog)
            Debug.Log($"{gameObject.name}: 공격 시작!");

        isAttacking = true;
        hitResources.Clear();
        currentTargetResource = null;
    }

    /// <summary>
    /// 공격 종료 (애니메이션 이벤트에서 호출)
    /// </summary>
    public void OnAttackEnd()
    {
        if (showDebugLog)
            Debug.Log($"{gameObject.name}: 공격 종료!");

        isAttacking = false;
        hitResources.Clear();
        currentTargetResource = null;
    }

    /// <summary>
    /// 채집 가능 여부
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;

        // 레이어 확인
        if (((1 << other.gameObject.layer) & gatherableLayer) == 0)
            return;

        // 자원 컴포넌트
        ResourceObject resource = other.GetComponent<ResourceObject>();

        // 한 번에 하나만 채집
        if (oneResourcePerAttack)
        {
            if (currentTargetResource != null)
            {
                if (showDebugLog)
                    Debug.Log($"이미 {currentTargetResource.name} 채집 중");
                return;
            }
            // 채집 중인 자원
            currentTargetResource = resource;
        }

        // 자원 채집
        GatherResource(resource, other.gameObject);
    }

    /// <summary>
    /// 자원 채집 처리
    /// </summary>
    void GatherResource(ResourceObject resource, GameObject resourceObj)
    {
        // 자원 채집
        resource.TryGather();

        // 타격 목록에 추가
        hitResources.Add(resourceObj);

        if (showDebugLog)
            Debug.Log($"[채집] {resourceObj.name}");
    }

    /// <summary>
    /// 채집 범위 체크
    /// </summary>
    public void CheckGatherRaycast()
    {
        if (!isAttacking) return;

        // 무기 앞쪽 레이
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, gatherRange, gatherableLayer))
        {
            ResourceObject resource = hit.collider.GetComponent<ResourceObject>();
            if (resource != null && !hitResources.Contains(hit.collider.gameObject))
            {
                GatherResource(resource, hit.collider.gameObject);
            }
        }
    }

    /// <summary>
    /// 채집 범위 체크 (구체)
    /// </summary>
    public void CheckGatherSphere()
    {
        if (!isAttacking) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, gatherRange, gatherableLayer);

        foreach (Collider col in colliders)
        {
            // 한 번에 하나만 채집
            if (oneResourcePerAttack && currentTargetResource != null)
                break;

            // 이미 타격한 자원 제외
            if (hitResources.Contains(col.gameObject))
                continue;

            ResourceObject resource = col.GetComponent<ResourceObject>();
            if (resource != null)
            {
                if (oneResourcePerAttack)
                    currentTargetResource = resource;

                GatherResource(resource, col.gameObject);
            }
        }
    }

    // Gizmo로 범위 표시
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = isAttacking ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gatherRange);

        if (isAttacking && currentTargetResource != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentTargetResource.transform.position);
        }
    }
}