
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private Player player;
    private List<PlayerHandler> _playerHandlers;
    
    private void Start()
    {
        _playerHandlers = player.GetPlayerHandlers();
    }

    public void PlayerHitEnemyEventSFX(HitContext context)
    {
        if(!context.hitSFX) return;
        
        PlayerSFXManager.Instance?.PlaySFXOneShot(context.hitSFX);
    }

    public void PlayerSwingWeaponEventSFX()
    {
        PlayerCombatHandler combatHandler = null;
        
        foreach (var playerHandler in _playerHandlers)
        {
            if (playerHandler is PlayerCombatHandler handler)
            {
                combatHandler = handler;
            }
        }
        
        if(combatHandler == null) return;

        var attackData = combatHandler.GetCurrentAttackData();
        
        if(!attackData.attackAudioClip) return;
        
        PlayerSFXManager.Instance?.PlaySFXOneShot(attackData.attackAudioClip);
    }
}

