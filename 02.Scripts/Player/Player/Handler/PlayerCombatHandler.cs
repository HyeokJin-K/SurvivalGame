using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static WeaponIdTable;
using Object = UnityEngine.Object;

public class PlayerCombatHandler : PlayerHandler
{
    private PlayerCombatConfig _playerCombatConfig;
    
    private Weapon _currentWeapon;
    private BasicAttackData _currentComboAttack;
    
    private PlayerHandlerContext _playerHandlerContext;
    private CombatContext _combatContext;

    public PlayerCombatHandler(PlayerEvents playerEvents, PlayerCombatConfig playerCombatConfig) : base(playerEvents)
    {
        _playerCombatConfig = playerCombatConfig;
        PlayerEvents = playerEvents;
    }

    public bool HasAttack(AttackInputType inputType)
    {
        if(!_currentWeapon) return false;
        
        if (!_currentComboAttack)
        {
            var firstAttacks = _currentWeapon.WeaponAttacks;

            return firstAttacks.ContainsKey(inputType);
        }
        else
        {
            var nextComboAttacks = _currentComboAttack.nextComboAttacks;
            
            return nextComboAttacks.ContainsKey(inputType);
        }
    }
    
    public bool RequestAttack(AttackInputType inputType, out int attackId)
    {
        var attacks = _currentWeapon.WeaponAttacks;

        if (_currentComboAttack)
        {
            if (_currentComboAttack.HasComboLink(inputType))
            {
                var nextAttack = _currentComboAttack.GetNextAttack(inputType);
                attackId = nextAttack.attackId;
                _currentComboAttack = nextAttack;
                return true;
            }
        }
        
        if (attacks.TryGetValue(inputType, out var attack))
        {
            attackId = attack.attackId;
            _currentComboAttack = attack;
            return true;
        }
        else
        {
            attackId = 0;
            _currentComboAttack = null;
            return false;
        }
    }

    public void ResetAttack()
    {
        _currentComboAttack = null;
    }
    
    public void UpdateCombatConfig(PlayerCombatConfig playerCombatConfig)
    {
        _playerCombatConfig = playerCombatConfig;
    }

    public float GetTotalDamage()
    {
        float playerPower = _playerHandlerContext.playerStatsHandler.CurrentPlayerData.AttackPower;
        float weaponPower = _currentWeapon.WeaponPower;
        float motionPowerRate = _currentComboAttack.motionAttackPowerRate;

        return playerPower * weaponPower * motionPowerRate;
    }

    public BasicAttackData GetCurrentAttackData()
    {
        return _currentComboAttack;
    }

    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public void SetHitBoxEnabled(bool enabled)
    {
        if (enabled) _currentWeapon.EnableHitbox();
        else _currentWeapon.DisableHitbox();
    }
    
    public void SetCombatContext(CombatContext context)
    {
        _combatContext = context;
    }

    public void SetPlayerHandlerContext(PlayerHandlerContext context)
    {
        _playerHandlerContext = context;
    }

    public bool ChangeWeapon(int weaponId)
    {
        if(_combatContext == null) return false;

        var weaponPrefabs = _combatContext.weaponPrefabs;
        
        if (weaponPrefabs.Length == 0) return false;

        if (_playerHandlerContext.playerStatsHandler.CurrentPlayerState is not (IdleState or MoveState)) return false;

        if (weaponId == 0)
        {
            if (_currentWeapon)
            {
                Object.Destroy(_currentWeapon.gameObject);
                _playerHandlerContext.playerAnimatorHandler.SetWeaponType(WeaponType.OneHanded);
                _currentWeapon = null;
                return true;
            }
        }
        
        foreach (var t in weaponPrefabs)
        {
            var weaponPrefab = t.GetComponent<Weapon>();
            if (weaponPrefab.WeaponID == weaponId)
            {
                if (_currentWeapon)
                {
                    Object.Destroy(_currentWeapon.gameObject);
                    _currentWeapon = null;
                }
                
                GameObject weaponObj = Object.Instantiate(weaponPrefab.gameObject,
                    _playerCombatConfig.playerRightHandTransform);
                _currentWeapon = weaponObj.GetComponent<Weapon>();
                _currentWeapon.SetCombatHandler(this);
                weaponObj.transform.localPosition = _currentWeapon.WeaponInitPos;
                weaponObj.transform.localRotation = _currentWeapon.WeaponInitRot;
                weaponObj.transform.localScale *= 100f;
                
                _playerHandlerContext.playerAnimatorHandler.SetWeaponType(_currentWeapon.WeaponType);
                break;
            }
        }
        return true;
    }
}