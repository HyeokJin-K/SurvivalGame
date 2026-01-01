using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerSOData", menuName = "Scriptable Objects/PlayerSOData")]
// 플레이어 스탯 테이블
public class PlayerData : ScriptableObject
{
    
    [SerializeField] private string playerName;

    [SerializeField] private float maxHealth;
    
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private float sprintMultiplier;
    
    [SerializeField] private float attackPower;
    
    [SerializeField] private float attackSpeed;
    
    [SerializeField] private float defensePower;
    
    private PlayerEvents _playerEvents;
    
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            _playerEvents.OnChangedMaxHealth?.Invoke(maxHealth);
        }
    }
    
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    
    public float SprintMultiplier
    {
        get => sprintMultiplier;
        set => sprintMultiplier = value;
    }
    
    public float AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }
    
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }
    
    public float DefensePower
    {
        get => defensePower;
        set => defensePower = value;
    }

    private float _health;
    private float _previousHealth;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            _health = Mathf.Clamp(Health, 0, MaxHealth);

            if (Mathf.Approximately(_health, _previousHealth)) return;
            HUDManager.Instance?.HealthBarUpdate((int)_health, (int)MaxHealth);
            _playerEvents.OnChangedHealth?.Invoke(_health);
            _previousHealth = _health;

            if (_health <= 0f)
            {
                _playerEvents.OnDeath?.Invoke();
            }
        }
    }

    public PlayerData Clone()
    {
        return Instantiate(this);
    }

    public void InitPlayerData(PlayerEvents playerEvents)
    {
        _playerEvents = playerEvents;
        Health = MaxHealth;
        HUDManager.Instance?.HealthBarUpdate((int)_health, (int)MaxHealth);
    }
}