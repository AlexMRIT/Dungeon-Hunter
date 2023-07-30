using System;
using UnityEngine;

#pragma warning disable

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerMovement : MonoBehaviour, IModuleHandler
{
    private GameObject _playerObject;
    private Joystick _joystick;
    private float _movementSpeed;
    private Vector3 _direction;
    private CharacterController _characterController;
    private PlayerAnimation _playerAnimation;
    private Camera _mainCamera;
    private IEntityEvent _entityEvent;
    private bool _isDeath;

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="MissingComponentException"></exception>
    public void OnInitialization(IParamArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        _playerObject = (GameObject)args.GetContainer()?[0] ?? null;

        if (_playerObject == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        if (!_playerObject.TryGetComponent(out PlayerMovement playerMovement))
            throw new MissingComponentException(nameof(PlayerMovement));

        _joystick = (Joystick)args.GetContainer()?[1] ?? null;

        if (_joystick == null)
            throw new ArgumentNullException(nameof(Joystick));

        _movementSpeed = (float)args.GetContainer()?[2];

        if (_movementSpeed <= 0)
            Debug.LogWarning("Movement speed is less than or equal to zero!");

        _characterController = GetComponent<CharacterController>();

        _playerAnimation = (PlayerAnimation)args.GetContainer()?[3] ?? null;

        if (_playerAnimation == null)
            throw new ArgumentNullException(nameof(PlayerAnimation));

        _entityEvent = (IEntityEvent)args.GetContainer()?[4] ?? null;

        if (_entityEvent == null)
            throw new NullReferenceException(nameof(IEntityEvent));

        _mainCamera = Camera.main;
        _isDeath = false;
        _entityEvent.OnEnemyDeathEvent += PlayerDeathEvent;

        Debug.Log($"Module: {nameof(PlayerMovement)} has been initialise.");
    }

    public void OnDisable()
    {
        _entityEvent.OnEnemyDeathEvent -= PlayerDeathEvent;
    }

    private void PlayerDeathEvent()
    {
        _isDeath = true;
    }

    private void Update()
    {
        if (_isDeath)
            return;

        if (_playerAnimation.GetRunningAnimation() == AnimationType.PlayAnimationAttack)
            return;

#if !UNITY_ANDROID
        _direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
#else
        _direction = _joystick.Direction;
#endif
        if (_direction.magnitude >= 0.1f)
        {
            float angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));
            _playerAnimation.PlayAnimation(AnimationType.PlayAnimationRun);

            _characterController.Move(_direction * _movementSpeed * Time.deltaTime);
        }
        else
            _playerAnimation.PlayAnimation(AnimationType.PlayAnimationIdle);
    }
}