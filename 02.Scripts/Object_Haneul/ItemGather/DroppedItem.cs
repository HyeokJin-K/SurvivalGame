using UnityEngine;

/// <summary>
/// 땅에 떨어진 아이템 (습득 가능한 오브젝트)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DroppedItem : MonoBehaviour
{
    [Header("아이템 정보")]
    public ItemData itemData;  // 아이템 데이터
    public int quantity = 1;   // 수량

    [Header("물리 설정")]
    public float upwardForce = 5f;      // 위로 튀는 힘
    public float randomForce = 3f;      // 랜덤 방향 힘
    public float rotationSpeed = 100f;  // 회전 속도

    [Header("습득 설정")]
    public float pickupDelay = 0.5f;    // 드랍 후 습득 가능 대기 시간
    public float magnetRange = 3f;      // 자석 효과 범위
    public float magnetSpeed = 8f;      // 플레이어로 끌려가는 속도
    public LayerMask playerLayer;       // 플레이어 레이어

    [Header("시각 효과")]
    public float bobHeight = 0.3f;      // 떠있는 높이
    public float bobSpeed = 2f;         // 속도
    public bool glowEffect = true;      // 발광 효과

    [Header("디버그")]
    public bool showDebugLog = true;

    private Rigidbody rb;
    private Collider col;
    private Transform targetPlayer;
    private Vector3 initialPosition;
    private float spawnTime;
    private bool canPickup = false;
    private bool isMagnetized = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // 물리 설정
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        col.isTrigger = false;  // 일반 Collider

        spawnTime = Time.time;
    }

    void Start()
    {
        initialPosition = transform.position;

        // 드랍 효과
        ApplyDropEffect();

        // 일정 시간 후 습득 가능
        Invoke(nameof(EnablePickup), pickupDelay);

        if (showDebugLog) // null이 아니면 itemName | null이면 Unknown
            Debug.Log($"[아이템 드랍] {itemData?.itemName ?? "Unknown"} x{quantity}");
    }

    void Update()
    {
        if (!canPickup) return;

        // 자석 효과 체크
        if (!isMagnetized)
        {
            CheckForPlayer();
        }

        // 플레이어에게 끌려감
        if (isMagnetized && targetPlayer != null)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // 둥실거림 효과
            BobEffect();
        }

        // 회전 효과
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 드랍 시 물리 효과
    /// </summary>
    void ApplyDropEffect()
    {
        // 위로 튀어오름
        Vector3 force = Vector3.up * upwardForce;

        // 랜덤 방향 추가
        force += new Vector3(
            Random.Range(-randomForce, randomForce),
            0,
            Random.Range(-randomForce, randomForce)
        );

        rb.AddForce(force, ForceMode.Impulse);

        // 랜덤 회전
        rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
    }

    /// <summary>
    /// 습득 가능 활성화
    /// </summary>
    void EnablePickup()
    {
        canPickup = true;

        // Trigger로 변경 (플레이어 습득용)
        col.isTrigger = true;

        // 물리 정지
        rb.isKinematic = true;
        rb.useGravity = false;

        if (showDebugLog)
            Debug.Log($"[아이템] {itemData?.itemName} 습득 가능");
    }

    /// <summary>
    /// 둥실 효과
    /// </summary>
    void BobEffect()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    /// <summary>
    /// 플레이어 감지 (자석 효과)
    /// </summary>
    void CheckForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRange, playerLayer);

        if (colliders.Length > 0)
        {
            targetPlayer = colliders[0].transform;
            isMagnetized = true;

            if (showDebugLog)
                Debug.Log($"[아이템] 자석 효과 활성화");
        }
    }

    /// <summary>
    /// 플레이어에게 이동
    /// </summary>
    void MoveTowardsPlayer()
    {
        if (targetPlayer == null) return;

        // 플레이어 중심으로 이동
        Vector3 targetPosition = targetPlayer.position + Vector3.up * 1f;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            magnetSpeed * Time.deltaTime
        );

        // 거리 체크
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < 0.5f)
        {
            PickupItem();
        }
    }

    /// <summary>
    /// 획득 ( 플레이어 충돌 )
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (!canPickup) return;

        // 플레이어 레이어 체크
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            targetPlayer = other.transform;
            PickupItem();
        }
    }

    /// <summary>
    /// 아이템 획득 ( 인벤토리 연동 )
    /// </summary>
    void PickupItem()
    {
        if (itemData == null)
        {
            Debug.LogWarning("ItemData가 없습니다!");
            Destroy(gameObject);
            return;
        }

        if (showDebugLog)
            Debug.Log($"[습득] {itemData.itemName} x{quantity}");

        // 인벤토리 연동 
        if (InventoryManager.Instance != null)
        {
            bool success = InventoryManager.Instance.AddItemToFirstEmptySlot(itemData, quantity);

            if (success)
            {
                if (showDebugLog)
                    Debug.Log($"[습득 성공] {itemData.itemName} x{quantity}");

                // 오브젝트 제거
                Destroy(gameObject);
            }
            else
            {
                if (showDebugLog)
                    Debug.LogWarning($"[습득 실패] 인벤토리가 가득 참!");
            }
        }

        // 오브젝트 제거
        Destroy(gameObject);
    }

    /// <summary>
    /// 아이템 설정 ( 외부 호출 )
    /// </summary>
    public void SetItemData(ItemData data, int amount)
    {
        itemData = data;
        quantity = amount;
    }

    // Gizmo로 자석 범위 표시
    void OnDrawGizmosSelected()
    {
        if (!canPickup) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}