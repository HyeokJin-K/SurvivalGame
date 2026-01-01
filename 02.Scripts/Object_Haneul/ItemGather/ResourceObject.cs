using UnityEngine;

/// <summary>
/// 채집 가능한 자원 오브젝트 (나무, 돌, 버섯 등)
/// </summary>
public class ResourceObject : MonoBehaviour, IInteractable
{
    [Header("자원 정보")]
    public ItemData resourceData;  // 드랍될 아이템
    public int quantityPerHit = 1;  // 한 번에 얻는 개수
    public int maxHits = 3;  // 총 타격 가능 횟수

    [Header("드랍 설정")]
    public GameObject droppedItemPrefab; // 프리팹
    public float dropRadius = 1.5f; // 반경
    public float dropHeight = 1f; // 높이

    [Header("디버그")] // 테스트용
    public bool showDebugLog = false;

    private int currentHits = 0;  // 현재 타격 횟수
    private bool isBeingGathered = false;  // 채집 중인지 체크
    private float lastHitTime = 0f;  // 마지막 타격 시간
    private const float hitCooldown = 0.3f;  // 타격 쿨다운 (중첩 방지)

    void Start()
    {
        currentHits = 0;
    }

    /// <summary>
    /// IInteractable 구현: 상호작용 (채집)
    /// </summary>
    public void OnInteract()
    {
        // 플레이어 공격
        TryGather();
    }

    /// <summary>
    /// 자원 채집 시도
    /// </summary>
    public void TryGather()
    {
        // 쿨다운 체크 (빠른 연속 타격 방지)
        if (Time.time - lastHitTime < hitCooldown)
        {
            if (showDebugLog)
                Debug.Log($"{gameObject.name}: 쿨다운 중");
            return;
        }

        lastHitTime = Time.time;
        isBeingGathered = true;

        // 타격 횟수 증가
        currentHits++;

        // 남은 타격 횟수
        if (showDebugLog)
            Debug.Log($"{gameObject.name} 타격: {currentHits}/{maxHits}");

        // 아이템 드랍
        GiveResource();

        // 파괴 체크
        if (currentHits >= maxHits)
        {
            Destroy(gameObject);
        }
        else
        {
            isBeingGathered = false;
        }
    }

    /// <summary>
    /// 자원 아이템 드랍
    /// </summary>
    void GiveResource()
    {
        if (resourceData == null)
        {
            Debug.LogWarning($"{gameObject.name}: resourceData가 설정되지 않음");
            return;
        }

        if(droppedItemPrefab == null)
        {
            Debug.LogWarning($"{gameObject.name}: droppedItemPrefab이 설정되지 않음");
            Debug.Log($"[채집] {resourceData.itemName} x{quantityPerHit} (Prefab 없음)");
            return;
        }

        // 드랍될 위치
        Vector3 dropPosition = transform.position + Vector3.up * dropHeight;
        dropPosition += new Vector3(Random.Range(-dropRadius, dropRadius), 0, Random.Range(-dropRadius, dropRadius));


        // 프리팹 생성
        GameObject droppedObj = Instantiate(droppedItemPrefab, dropPosition, Quaternion.identity);
        DroppedItem droppedItem = droppedObj.GetComponent<DroppedItem>();

        if (droppedItem !=null)
        {
            droppedItem.SetItemData(resourceData, quantityPerHit); // DroppedItem.cs

            if (showDebugLog)
            {
                Debug.Log($"[드랍] {resourceData.itemName} x {quantityPerHit}");
            }
        }
        else
        {
            Debug.LogError($"{droppedItemPrefab.name}에 DroppedItem 없음");
            Destroy(droppedObj);
        }

    }

    /// <summary>
    /// 현재 채집 중인지 확인
    /// </summary>
    public bool IsBeingGathered()
    {
        return isBeingGathered;
    }

    // Gizmo로 범위 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}