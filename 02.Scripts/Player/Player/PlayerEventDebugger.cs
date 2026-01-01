
using UnityEngine;

public class PlayerEventDebugger : MonoBehaviour
{
    public GameObject TestPrefab;
    
#if UNITY_EDITOR
    public void StateEventTest(State state)
    {
        Debug.Log($"{state} 상태");
    }

    public void HitPosDebug(HitContext hitContext)
    {
        Debug.Log($"충돌 지점 {hitContext.hitPoint}");
        GameObject go = Instantiate(TestPrefab, hitContext.collider.transform);
        go.transform.position = hitContext.hitPoint;
    }
    
    public void HitMonsterNameDebug(HitContext hitContext)
    {
        Debug.Log($"몬스터 네임 {hitContext.collider.name}");
    }
    public void DamageDebug(HitContext hitContext)
    {
        Debug.Log($"데미지 {hitContext.damage}");
    }

    public void AttackStartTest()
    {
        Debug.Log("공격 시작");
    }

    public void AttackEndTest()
    {
        Debug.Log("공격 종료");
    }
#endif
}
