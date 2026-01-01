using UnityEngine;
public class StraightProjectile : ProjectileBase
{
    private Vector3 dir;
    
    void Start()
    {
        dir = (target.position - transform.position) + Vector3.up * 0.75f;
    }
    
    protected override void OnFire()
    {
        rb.linearVelocity = dir * stats.speed;
    }
}
