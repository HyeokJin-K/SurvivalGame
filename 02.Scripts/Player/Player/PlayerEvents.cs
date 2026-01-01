
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerEvents
{
    public UnityEvent<State> OnStateChanged = new UnityEvent<State>();
    
    public UnityEvent OnMoveStart = new UnityEvent();
    public UnityEvent OnMoveEnd = new UnityEvent();
    public UnityEvent OnMove = new UnityEvent();
    
    public UnityEvent OnAttackStart = new UnityEvent();
    public UnityEvent OnAttackEnd = new UnityEvent();
    public UnityEvent<HitContext> OnHitEnemy = new UnityEvent<HitContext>();
    
    public UnityEvent<float> OnChangedHealth = new UnityEvent<float>();
    public UnityEvent<float> OnChangedMaxHealth = new UnityEvent<float>();
    public UnityEvent<float> OnDamaged =  new UnityEvent<float>();
    public UnityEvent OnDeath = new UnityEvent();
    
    public UnityEvent<ItemData> OnUseItem = new UnityEvent<ItemData>();
}

