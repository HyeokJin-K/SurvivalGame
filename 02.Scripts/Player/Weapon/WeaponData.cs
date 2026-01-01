using System.Collections.Generic;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("무기 정보")]
    public WeaponType weaponType;
    public int weaponId;
    public string weaponName;
    public float weaponPower;
    public float attackSpeed;
    
    [Header("해당 무기의 초기 위치")]
    public Vector3 initialPosition;
    public Quaternion initialRotation;
}

public enum WeaponType
{
    OneHanded,
    TwoHanded,
}