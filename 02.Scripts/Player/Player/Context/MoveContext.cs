using UnityEngine;

public class MoveContext
{
    public float MoveSpeed { get; private set; }
    public float SprintMultiplier { get; private set; }
    
    public void SetMoveStatProperty(float moveSpeed, float sprintMultiplier)
    {
        MoveSpeed = moveSpeed;
        SprintMultiplier = sprintMultiplier;
    }
}