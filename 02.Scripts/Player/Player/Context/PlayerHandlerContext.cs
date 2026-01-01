
public class PlayerHandlerContext
{
    public PlayerAnimatorHandler playerAnimatorHandler;
    public PlayerCombatHandler playerCombatHandler;
    public PlayerStatsHandler playerStatsHandler;
    public PlayerMovementHandler playerMovementHandler;
    public PlayerInputHandler playerInputHandler;
    
    public PlayerHandlerContext(PlayerAnimatorHandler playerAnimatorHandler, PlayerCombatHandler playerCombatHandler,
        PlayerStatsHandler playerStatsHandler, PlayerMovementHandler playerMovementHandler,
        PlayerInputHandler playerInputHandler)
    {
        this.playerAnimatorHandler = playerAnimatorHandler;
        this.playerCombatHandler = playerCombatHandler;
        this.playerStatsHandler = playerStatsHandler;
        this.playerMovementHandler = playerMovementHandler;
        this.playerInputHandler = playerInputHandler;
    }
}

