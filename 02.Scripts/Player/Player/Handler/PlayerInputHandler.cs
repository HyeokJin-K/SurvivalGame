using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : PlayerHandler
{
    public readonly List<int> MoveUpDownBuffer = new List<int>(2);
    public readonly List<int> MoveLeftRightBuffer = new List<int>(2);
    public readonly CommandBuffer AttackCommandBuffer = new CommandBuffer(0.4f);
    public readonly CommandBuffer RollCommandBuffer = new CommandBuffer(0.4f);
    public bool IsSprintBuffer { get; set; }

    private PlayerHandlerContext _playerHandlerContext;
    public Vector2 InputDir
    {
        get
        {
            Vector2 inputDir;
            inputDir.y = MoveUpDownBuffer.Count > 0 ? MoveUpDownBuffer[MoveUpDownBuffer.Count - 1] : 0f;
            inputDir.x = MoveLeftRightBuffer.Count > 0 ? MoveLeftRightBuffer[MoveLeftRightBuffer.Count - 1] : 0f;

            if (inputDir.x == 0 && inputDir.y == 0) return Vector2.zero;

            inputDir.Normalize();
            return inputDir;
        }
    }

    public PlayerInputHandler(PlayerEvents playerEvents) : base(playerEvents)
    {
        PlayerEvents = playerEvents;
    }

    #region Handler LifeCycle

    public override void HandlerEnable()
    {
        base.HandlerEnable();
        ClearAllBuffers();
    }

    public override void HandlerUpdate()
    {
        base.HandlerUpdate();

        if (RollCommandBuffer.HasCommands())
        {
            RollCommandBuffer.ProcessCommands(_playerHandlerContext);
        }
        
        if (AttackCommandBuffer.HasCommands())
        {
            AttackCommandBuffer.ProcessCommands(_playerHandlerContext);
        }
    }
    #endregion


    public void SetPlayerHandlerContext(PlayerHandlerContext context)
    {
        _playerHandlerContext = context;
    }

    public void ClearAllBuffers()
    {
        MoveUpDownBuffer.Clear();
        MoveLeftRightBuffer.Clear();
        AttackCommandBuffer.ClearBuffer();
        IsSprintBuffer = false;
    }
}