using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(menuName = "Combat/Basic Attack")]
public class BasicAttackData : AttackDataBase
{
    public bool isLastCombo;
    
    public SerializableDictionaryBase<AttackInputType, BasicAttackData> nextComboAttacks;
    
    public BasicAttackData GetNextAttack(AttackInputType inputType)
    {
        if (nextComboAttacks.TryGetValue(inputType, out var nextAttack))
        {
            return nextAttack;
        }
        
        return null;
    }

    public bool HasComboLink(AttackInputType inputType)
    {
        return GetNextAttack(inputType);
    }
}