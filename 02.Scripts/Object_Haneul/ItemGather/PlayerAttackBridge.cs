using UnityEngine;

/// <summary>
/// 애니메이션 이벤트를 받아서 Weapon의 WeaponGatherTrigger로 전달
/// </summary>
public class PlayerAttackBridge : MonoBehaviour
{
    [Header("무기 참조")]
    [Tooltip("WeaponGatherTrigger가 부착된 무기 오브젝트")]
    public WeaponGatherTrigger weaponGatherTrigger;

    [Header("디버그")]
    public bool showDebugLog = true;
    

    /// <summary>
    /// 애니메이션 이벤트: 공격 시작
    /// </summary>
    public void OnAttackStart()
    {
        if (showDebugLog)
            Debug.Log("[Player Bridge] 공격 시작!");

        if (weaponGatherTrigger != null)
        {
            weaponGatherTrigger.OnAttackStart();
        }
        else
        {
            Debug.LogWarning("weaponGatherTrigger가 연결되지 않았습니다!");
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 공격 종료
    /// </summary>
    public void OnAttackEnd()
    {
        if (showDebugLog)
            Debug.Log("[Player Bridge] 공격 종료!");

        if (weaponGatherTrigger != null)
        {
            weaponGatherTrigger.OnAttackEnd();
        }
    }

    /// <summary>
    /// 애니메이션 이벤트: 타격 판정 시점 (선택사항)
    /// 특정 프레임에서 즉시 채집 판정을 원할 경우
    /// </summary>
    //public void OnAttackHit()
    //{
    //    if (showDebugLog)
    //        Debug.Log("[Player Bridge] 타격 판정!");

    //    if (weaponGatherTrigger != null)
    //    {
    //        // SphereCast로 즉시 채집 판정
    //        weaponGatherTrigger.CheckGatherSphere();
    //    }
    //}
}