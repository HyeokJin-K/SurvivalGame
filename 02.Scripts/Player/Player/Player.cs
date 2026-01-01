using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static WeaponIdTable;

[RequireComponent(typeof(PlayerInputEventProvider))]
public class Player : MonoBehaviour, IDamagable, IPlayerEventProvider, IPlayerStatsProvider
    , ICanUseItem
{
    [Header("플레이어")] [SerializeField] private PlayerData playerData;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInputEventProvider playerInputEventProvider;
    [SerializeField] private PlayerAttackBridge playerAttackBridge;

    [Header("플레이어 물리 설정")] [SerializeField]
    private PlayerPhysicsConfig playerPhysicsConfig;

    [Header("플레이어 전투 설정")] [SerializeField]
    private PlayerCombatConfig playerCombatConfig;

    [Header("플레이어 이벤트")] [SerializeField] private PlayerEvents playerEvents;

    private PlayerStatsHandler _playerStatsHandler;
    private PlayerMovementHandler _playerMovementHandler;
    private PlayerAnimatorHandler _playerAnimatorHandler;
    private PlayerCombatHandler _playerCombatHandler;
    private PlayerInputHandler _playerInputHandler;
    private readonly List<PlayerHandler> _playerHandlers = new List<PlayerHandler>();

    private PlayerHandlerContext _playerHandlerContext;
    private MoveContext _moveContext;
    private CombatContext _combatContext;
    

    #region Unity LifeCycle

    public void Awake()
    {
        if (!playerCamera) Debug.LogError("플레이어 컴포넌트 카메라 오브젝트 캐싱 필요");


        _playerStatsHandler = new PlayerStatsHandler(playerEvents, playerData);
        _playerMovementHandler =
            new PlayerMovementHandler(playerEvents, playerCamera, characterController, playerPhysicsConfig);
        _playerCombatHandler = new PlayerCombatHandler(playerEvents, playerCombatConfig);
        _playerAnimatorHandler = new PlayerAnimatorHandler(playerEvents, animator);
        _playerInputHandler = new PlayerInputHandler(playerEvents);

        _moveContext = new MoveContext();
        _combatContext = new CombatContext(playerCombatConfig.weaponPrefabs);
        _playerHandlerContext = new PlayerHandlerContext(_playerAnimatorHandler, _playerCombatHandler,
            _playerStatsHandler, _playerMovementHandler, _playerInputHandler);

        _playerStatsHandler.SetPlayerHandlerContext(_playerHandlerContext);
        _playerAnimatorHandler.SetPlayerHandlerContext(_playerHandlerContext);
        _playerCombatHandler.SetPlayerHandlerContext(_playerHandlerContext);
        _playerMovementHandler.SetPlayerHandlerContext(_playerHandlerContext);
        _playerInputHandler.SetPlayerHandlerContext(_playerHandlerContext);

        _playerMovementHandler.SetMoveContext(_moveContext);
        _playerCombatHandler.SetCombatContext(_combatContext);

        _playerHandlers.Add(_playerMovementHandler);
        _playerHandlers.Add(_playerStatsHandler);
        _playerHandlers.Add(_playerCombatHandler);
        _playerHandlers.Add(_playerInputHandler);
        _playerHandlers.Add(_playerAnimatorHandler);

        playerInputEventProvider?.SetPlayerInputHandler(_playerInputHandler);

        foreach (var t in _playerHandlers)
        {
            t.HandlerAwake();
        }
    }

    public void Start()
    {
        foreach (var t in _playerHandlers)
        {
            t.HandlerStart();
        }
    }

    public void Update()
    {
        foreach (var t in _playerHandlers)
        {
            t.HandlerUpdate();
        }
    }

    public void FixedUpdate()
    {
        foreach (var t in _playerHandlers)
        {
            t.HandlerFixedUpdate();
        }
    }

    private void OnEnable()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        foreach (var t in _playerHandlers)
        {
            t.HandlerEnable();
        }

        UpdateMoveContext();
    }

    private void OnDisable()
    {
        foreach (var t in _playerHandlers)
        {
            t.HandlerDisable();
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        
        ChangeWeapon(playerCombatConfig.initWeaponId);
    }

    #endregion

    private void UpdateMoveContext()
    {
        _moveContext.SetMoveStatProperty(GetCurrentPlayerData().MoveSpeed, GetCurrentPlayerData().SprintMultiplier);
        _playerMovementHandler.SetMoveContext(_moveContext);
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - GetCurrentPlayerData().DefensePower;
        playerEvents.OnDamaged?.Invoke(totalDamage);
        GetCurrentPlayerData().Health -= totalDamage;
    }

    public PlayerEvents GetPlayerEvents()
    {
        return playerEvents;
    }

    public PlayerData GetCurrentPlayerData()
    {
        return _playerStatsHandler.CurrentPlayerData;
    }

    public List<PlayerHandler> GetPlayerHandlers()
    {
        return _playerHandlers;
    }

    public bool UseItem(ItemData item)
    {
        return _playerStatsHandler.ConsumeItem(item);
    }

    public void ChangeWeapon(int weaponId)
    {
        if(_playerCombatHandler==null) return;
        if (_playerCombatHandler.ChangeWeapon(weaponId))
        {
            playerAttackBridge.weaponGatherTrigger = _playerCombatHandler.GetCurrentWeapon().GetWeaponGatherTrigger();
        }
    }

    #region Animation Event

    public void EnableHitbox() // 애니메이션 이벤트
    {
        _playerCombatHandler.SetHitBoxEnabled(true);
    }

    public void DisableHitbox() // 애니메이션 이벤트
    {
        _playerCombatHandler.SetHitBoxEnabled(false);
    }

    public void ComboWindowOpen() // 애니메이션 이벤트
    {
        var currentState = _playerStatsHandler.CurrentPlayerState;

        if (currentState is AttackState attackState)
        {
            attackState.ComboWindowOpen();
        }
    }

    public void ComboWindowClose() // 애니메이션 이벤트
    {
        var currentState = _playerStatsHandler.CurrentPlayerState;

        if (currentState is AttackState attackState)
        {
            attackState.ComboWindowClose();
        }
    }

    public void AttackAnimationComplete() // 애니메이션 이벤트
    {
        var currentState = _playerStatsHandler.CurrentPlayerState;

        if (currentState is AttackState attackState)
        {
            attackState.AttackAnimationComplete();
        }
    }

    public void RollAnimationComplete() // 애니메이션 이벤트
    {
        var currentState = _playerStatsHandler.CurrentPlayerState;

        if (currentState is RollState rollState)
        {
            rollState.RollAnimationComplete();
        }
    }

    #endregion
}