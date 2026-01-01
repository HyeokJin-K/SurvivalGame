using System;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponHitBox weaponHitbox;
    [SerializeField] private WeaponGatherTrigger weaponGatherTrigger;
    
    [SerializeField]
    private SerializableDictionaryBase<AttackInputType, BasicAttackData> weaponAttacks =
        new SerializableDictionaryBase<AttackInputType, BasicAttackData>();
    public SerializableDictionaryBase<AttackInputType, BasicAttackData> WeaponAttacks => weaponAttacks;
        
    public WeaponData WeaponData => weaponData;
    public string WeaponName => weaponData.weaponName;
    public WeaponType WeaponType => weaponData.weaponType;
    public int WeaponID => weaponData.weaponId;
    public float WeaponPower => weaponData.weaponPower;
    public Vector3 WeaponInitPos => weaponData.initialPosition;
    public Quaternion WeaponInitRot => weaponData.initialRotation;

    public void SetCombatHandler(PlayerCombatHandler combatHandler)
    {
        weaponHitbox.SetCombatHandler(combatHandler);
    }

    public WeaponGatherTrigger GetWeaponGatherTrigger()
    {
        return weaponGatherTrigger;
    }
    
    public void EnableHitbox()
    {
        weaponHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        weaponHitbox.DisableHitbox();
    }

}
