
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    [SerializeField] private Collider hitbox;
    
    private PlayerCombatHandler _playerCombatHandler;
    private readonly HitContext _hitContext = new HitContext();
    private HashSet<IDamagable> HitObjects { get; set; } = new HashSet<IDamagable>();

    private void OnEnable()
    {
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Monster")) return;
        
        if (other.TryGetComponent<IDamagable>(out var enemy))
        {
            if (HitObjects.Add(enemy))
            {
                var currentAttackData = _playerCombatHandler.GetCurrentAttackData();
                _hitContext.Init(other, other.ClosestPoint(transform.position), _playerCombatHandler.GetTotalDamage(),
                    currentAttackData.hitAudioClip,currentAttackData.hitParticle);
                enemy.TakeDamage(_playerCombatHandler.GetTotalDamage());
                _playerCombatHandler.PlayerEvents.OnHitEnemy?.Invoke(_hitContext);
                enemy.TakeDamage(_playerCombatHandler.GetTotalDamage());
            }
        }
    }

    public void SetCombatHandler(PlayerCombatHandler combatHandler)
    {
        _playerCombatHandler = combatHandler;
    }

    public void EnableHitbox()
    {
        HitObjects.Clear();
        _playerCombatHandler.PlayerEvents.OnAttackStart?.Invoke();
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        _playerCombatHandler.PlayerEvents.OnAttackEnd?.Invoke();
        hitbox.enabled = false;
    }
}
