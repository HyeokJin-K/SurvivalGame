using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private string[] keys;         // 프리팹을 부를 이름
    [SerializeField] private int[] sizes;             // 초기 풀 크기
    [SerializeField] private int expandBatch = 8;   // 부족 시 한 번에 추가 생성 수
    [SerializeField] private Transform parent;

    private Queue<GameObject> _pool;
    
    private readonly Dictionary<string, Queue<GameObject>> _pools = new();
    private readonly Dictionary<string, GameObject> _prefabMap = new();
    
    private static Factory _instance;
    public static Factory Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("[Factory] Duplicate instance detected. Destroying this.");
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        if (parent == null) parent = transform;

        // 기본 검증
        if (keys == null || prefabs == null || sizes == null)
        {
            Debug.LogError("[Factory] Arrays are null. Check inspector.");
            return;
        }
        if (!(keys.Length == prefabs.Length && keys.Length == sizes.Length))
        {
            Debug.LogError("[Factory] keys/prefabs/sizes length mismatch.");
            return;
        }
        
        // 매핑 + 프리로드
        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            GameObject prefab = prefabs[i];
            int size = Mathf.Max(0, sizes[i]);

            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.LogError($"[Factory] Empty key at index {i}");
                continue;
            }
            if (prefab == null)
            {
                Debug.LogError($"[Factory] Prefab is null for key '{key}' (index {i})");
                continue;
            }

            _prefabMap[key] = prefab;
            _pools[key] = new Queue<GameObject>(size);

            if (size > 0)
                CreateBatch(key, size);
        }
    }
    
    private void CreateBatch(string key, int count)
    {
        if (!_prefabMap.TryGetValue(key, out var prefab))
        {
            Debug.LogError($"[Factory] Unknown key '{key}' - cannot create batch.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab, parent); // 단일 부모에만 붙임
            go.SetActive(false);
            _pools[key].Enqueue(go);
        }
    }
    
    public GameObject GetObject(string key)
    {
        if (!_pools.ContainsKey(key))
        {
            Debug.LogError($"[Factory] Pool for key '{key}' not found.");
            return null;
        }

        if (_pools[key].Count == 0)
        {
            int batch = Mathf.Max(1, expandBatch);
            CreateBatch(key, batch);
        }

        var go = _pools[key].Dequeue();
        go.SetActive(true);
        return go;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (obj == null) return;

        if (!_pools.ContainsKey(key))
        {
            // 등록되지 않은 키로 반환되면 파괴(프로젝트 규칙에 맞게 조정 가능)
            obj.SetActive(false);
            Debug.LogWarning($"[Factory] Returned object with unknown key '{key}'. Destroying.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(parent, false); // 혹시 외부에서 리-parent 했다면 복귀
        _pools[key].Enqueue(obj);
    }
}
