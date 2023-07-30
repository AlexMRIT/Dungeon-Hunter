using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct EnemyImplement
{
    public BaseEnemy BaseEnemyImpl;
    public Specification SpecificationEnemy;
    public AggressSettings AggressSettingsEnemy;
}

public sealed class GameHandler : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] private Transform _spawnPointThePlayer;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private float _smoothPositionCamera;
    [SerializeField] private Vector3 _distanceCamera;
    [SerializeField] private AttachTransformWithObject _attachTransformWithObject;

    [Space, Header("Movement settings")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _attakcButton;

    [Space, Header("Player settings")]
    [SerializeField] private Specification _playerSpecification;

    [Space, Header("Enemy settings")]
    [SerializeField] private EnemyImplement[] _enemyImplements;

    private PlayerAnimation _playerAnimation;
    private Animator _animator;
    private PlayerAttackCombat _playerAttackCombat;
    private IPlayer _basePlayerImplement;

    private void Awake()
    {
#if UNITY_ANDROID
        _attakcButton.gameObject.SetActive(true);
#else
        _attakcButton.gameObject.SetActive(false);
#endif

        GameObject playerObject = Instantiate(_player.gameObject, _spawnPointThePlayer);

        if (!playerObject.TryGetComponent(out PlayerMovement playerMovement))
            throw new MissingComponentException(nameof(PlayerMovement));

        if (!playerObject.TryGetComponent(out PlayerAnimation playerAnimation))
            throw new MissingComponentException(nameof(PlayerAnimation));

        if (!playerObject.TryGetComponent(out Animator animator))
            throw new MissingComponentException(nameof(Animator));

        if (!playerObject.TryGetComponent(out PlayerAttackCombat playerAttackCombat))
            throw new MissingComponentException(nameof(PlayerAttackCombat));

        if (!playerObject.TryGetComponent(out BasePlayer basePlayer))
            throw new MissingComponentException(nameof(BasePlayer));

        _player = playerMovement;
        _playerAnimation = playerAnimation;
        _animator = animator;
        _playerAttackCombat = playerAttackCombat;
        _basePlayerImplement = basePlayer;

        IParamArgs args = new DIContainer();

        args.AddNewContainer(InternalGetArgsAttachCamera());
        _attachTransformWithObject.OnInitialization(args);

        args.AddNewContainer(InternalGetArgsInstallAnimation());
        _playerAnimation.OnInitialization(args);

        args.AddNewContainer(InternalGetArgsInstallMovement());
        _player.OnInitialization(args);

        args.AddNewContainer(InternalGetArgsAttackCombat());
        _playerAttackCombat.OnInitialization(args);

        args.AddNewContainer(InternalGetArgsSpecification());
        ((IModuleHandler)_basePlayerImplement).OnInitialization(args);

        _basePlayerImplement.InstallPlayerComponent(playerMovement, playerAnimation, playerAttackCombat);

        for (int iterator = 0; iterator < _enemyImplements.Length; iterator++)
        {
            Specification specification = _enemyImplements[iterator].SpecificationEnemy;
            AggressSettings aggressSettings = _enemyImplements[iterator].AggressSettingsEnemy;

            _enemyImplements[iterator].BaseEnemyImpl.Initialize(specification, aggressSettings);
            _enemyImplements[iterator].BaseEnemyImpl.SetTarget(_basePlayerImplement);
            _enemyImplements[iterator].BaseEnemyImpl.Setup();
        }
    }

    private object[] InternalGetArgsAttachCamera()
    {
        object[] args = new object[3];

        args[0] = _player;
        args[1] = _smoothPositionCamera;
        args[2] = _distanceCamera;

        return args;
    }

    private object[] InternalGetArgsInstallMovement()
    {
        object[] args = new object[5];

        args[0] = _player.gameObject;
        args[1] = _joystick;
        args[2] = _playerSpecification.MoveSpeed;
        args[3] = _playerAnimation;
        args[4] = (IEntityEvent)_basePlayerImplement;

        return args;
    }

    private object[] InternalGetArgsInstallAnimation()
    {
        object[] args = new object[2];

        args[0] = _animator;
        args[1] = _playerSpecification.AttackSpeed;

        return args;
    }

    private object[] InternalGetArgsAttackCombat()
    {
        object[] args = new object[4];

        args[0] = _playerAnimation;
        args[1] = _playerSpecification.AttackSpeed;
        args[2] = _attakcButton;
        args[3] = (IEntityEvent)_basePlayerImplement;

        return args;
    }

    private object[] InternalGetArgsSpecification()
    {
        object[] args = new object[1];

        args[0] = _playerSpecification;

        return args;
    }
}