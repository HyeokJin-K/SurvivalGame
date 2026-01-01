using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable] 
    public class ResourceObject
    {
        public GameObject prefab; // 생성할 오브젝트 ( 프리팹 )
        public int spawnCount = 10; // 개수
        [Range(0f, 1f)]
        public float spawnProbability = 1f; // 생성 확률 1 ~ 100%
    }

    [System.Serializable]
    public class TerrainType
    {
        public string terrainName; // 지형 이름
        public LayerMask terrainLayer; // 지형 레이어 ( Raycast 필터 )
        public List<ResourceObject> resources = new List<ResourceObject>(); // 생성할 자원
    }

    /* ---------- 지형 설정 ---------- */
    #region  [지형]인스펙터 설정
    [Header("스폰 영역 설정")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(100f, 0f, 100f);

    [Header("지형 타입별 자원")]
    public List<TerrainType> terrainTypes = new List<TerrainType>();

    [Header("스폰 설정")]
    [Tooltip("Raycast 시작 높이 (지형 위)")]
    public float raycastHeight = 150f;
    
    [Tooltip("생성 가능 최대 허용 경사각")]
    [Range(0f, 90f)]
    public float maxSlope = 45f;
    
    [Tooltip("지면으로부터 오프셋")]
    public float groundOffset = 0.1f;
    
    [Tooltip("최소 오브젝트 간 거리")]
    public float minDistanceBetweenObjects = 2f;

    [Header("디버그")] // 테스트
    public bool showDebugGizmos = true;
    public bool autoSpawnOnStart = false;
    #endregion

    // 생성된 오브젝트 위치 리스트 ( 중복 배치 방지 )
    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        if (autoSpawnOnStart) // 자동 생성 옵션
        {
            SpawnAllResources();
        }
    }

    /// <summary>
    /// 모든 자원 오브젝트 생성
    /// </summary>
    public void SpawnAllResources()
    {
        // 기존 오브젝트 삭제 & 초기화
        ClearSpawnedObjects();
        spawnedPositions.Clear();

        foreach (TerrainType terrain in terrainTypes)
        {
            SpawnResourcesForTerrain(terrain);
        }

        Debug.Log($"총 {spawnedPositions.Count} 오브젝트 생성");
    }

    /// <summary>
    /// 특정 지형 타입의 자원 생성
    /// </summary>
    void SpawnResourcesForTerrain(TerrainType terrain)
    {
        foreach (ResourceObject resource in terrain.resources)
        {
            if (resource.prefab == null) continue;

            int successCount = 0;
            int attemptCount = 0;
            int maxAttempts = resource.spawnCount * 10; // 실패 방지용 --

            while (successCount < resource.spawnCount && attemptCount < maxAttempts)
            {
                attemptCount++;

                // 확률 체크
                if (Random.value > resource.spawnProbability)
                    continue;

                // 랜덤 위치 생성
                Vector3 randomPos = GetRandomPositionInArea();
                
                // 지형 검사 
                if (TryGetValidSpawnPoint(randomPos, terrain.terrainLayer, out Vector3 spawnPoint, out Vector3 normal))
                {
                    // 최소 거리 체크
                    if (IsPositionValid(spawnPoint))
                    {
                        // 오브젝트 생성 ( 원본, 위치, 회전, 부모 )
                        GameObject obj = Instantiate(resource.prefab, spawnPoint, Quaternion.identity, transform);
                        
                        // 지형 법선에 맞춰 회전
                        AlignToNormal(obj.transform, normal);
                        
                        spawnedPositions.Add(spawnPoint);
                        successCount++;
                    }
                }
            }

            Debug.Log($"{terrain.terrainName} - {resource.prefab.name}: {successCount}/{resource.spawnCount} 생성");
        }
    }

    /// <summary>
    /// 영역 내 랜덤 위치 반환
    /// </summary>
    Vector3 GetRandomPositionInArea()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);
        
        return spawnAreaCenter + new Vector3(randomX, raycastHeight, randomZ);
    }

    /// <summary>
    /// 유효한 스폰 지점 찾기 (Raycast)
    /// </summary>
    bool TryGetValidSpawnPoint(Vector3 position, LayerMask layer, out Vector3 spawnPoint, out Vector3 normal)
    {
        spawnPoint = Vector3.zero;
        normal = Vector3.up;

        // 위->아래 ( 시작점, 방향, hit정보, 거리, 레이어 )
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, raycastHeight * 2f, layer))
        {
            // 경사도 체크
            float slope = Vector3.Angle(hit.normal, Vector3.up);
            
            if (slope <= maxSlope)
            {
                // 충돌 지점 월드 좌표 + 충돌 표면 법선 벡터(수직)
                spawnPoint = hit.point + hit.normal * groundOffset;
                normal = hit.normal;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 다른 오브젝트와의 최소 거리 체크
    /// </summary>
    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPos in spawnedPositions)
        {
            if (Vector3.Distance(position, existingPos) < minDistanceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 오브젝트를 지형 법선에 맞춰 회전
    /// </summary>
    void AlignToNormal(Transform obj, Vector3 normal)
    {
        // Y축 랜덤 회전 + 지형 법선 적용
        float randomYRotation = Random.Range(0f, 360f);
        // 시작 -> 끝 회전 (시작, 끝)
        Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
        // 오일러 -> 쿼터니언 변환
        Quaternion yRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        
        // 회전 합성 ( 지형 경사 * y 랜덤 회전 )
        obj.rotation = normalRotation * yRotation;
    }

    /// <summary>
    /// 생성된 모든 오브젝트 제거
    /// </summary>
    public void ClearSpawnedObjects()
    {
        // 자식 오브젝트 모두 제거
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
        spawnedPositions.Clear();
    }

    // 기즈모로 스폰 영역 시각화
    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        // 스폰 영역 박스
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);

        // 생성된 오브젝트 위치
        Gizmos.color = Color.green;
        foreach (Vector3 pos in spawnedPositions)
        {
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}