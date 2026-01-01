using UnityEngine;
/// <summary>
/// 직선 탄 클래스
/// </summary>
public class GuidedProjectile : ProjectileBase
{
    protected override void OnFire()
    {
        var dir = (target.position - transform.position) + Vector3.up * 0.75f;
        rb.linearVelocity = dir * stats.speed;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}