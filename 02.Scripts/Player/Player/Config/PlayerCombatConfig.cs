
using System;
using UnityEngine;

[Serializable]
public class PlayerCombatConfig
{
    [Tooltip("1:한손검, 2:두손검")]public int initWeaponId;
    public Transform playerLeftHandTransform;
    public Transform playerRightHandTransform;
    public GameObject[] weaponPrefabs;
}

