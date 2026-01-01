
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private Player player;
    
    public void PlayerHitEnemyEventVFX(HitContext context)
    {
        if(!context.hitParticle) return;
        
        PlayerVFXManager.Instance?.PlayParticle(context.hitParticle, context.hitPoint,
            Quaternion.identity);
    }
}
