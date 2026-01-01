using UnityEngine;

public class MeleeController : EnemyController
{
    [Header(" * 근거리 공격 *")]
    [SerializeField] private Transform hitOrigin;
    [SerializeField] private float hitRadius = 1.0f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float knockbackForce = 7f;
    
    private bool _didHitThisSwing;
    
    public void AttackBegin()
    {
        Debug.Log("AttackBegin");
        _didHitThisSwing = false;

        if (attackVfx != null)
        {
            attackVfx.PlaySwing();
            sfxController.PlayAttack();
        }

        TryDoHit();
    }


    public void AttackEnd()
    {
        attackVfx.PlayHit(hitOrigin.position);
        _didHitThisSwing = true;
    }

    private void TryDoHit()
    {
        if (_didHitThisSwing) return;

        Vector3 center = hitOrigin ? hitOrigin.position : transform.position;
        var results = Physics.OverlapSphere(center, hitRadius, hitMask, QueryTriggerInteraction.Collide);
        if (results.Length == 0) return;
        
        for (int i = 0; i < results.Length; i++)
        {
            var col = results[i];
            if (col == null) continue;
            
            if (col.GetComponentInParent<Player>() is Player player)
            {
                player.TakeDamage(Damage);
                
                var rb = col.attachedRigidbody;
                if (rb)
                {
                    Vector3 dir = (player.transform.position - transform.position).normalized;
                    dir.y = 0f;
                    rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
                }

                _didHitThisSwing = true;
                break;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = hitOrigin ? hitOrigin.position : transform.position;
        Gizmos.DrawWireSphere(center, hitRadius);
    }
}