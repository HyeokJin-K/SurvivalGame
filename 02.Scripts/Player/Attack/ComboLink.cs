using System;

[Serializable]
public class ComboLink
{
    public AttackInputType inputType;
    public BasicAttackData nextAttack;
}

public enum AttackInputType
{
    None,
    LeftClick,
    RightClick
}