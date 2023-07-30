using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#pragma warning disable

public sealed class PlayerAttackCombat : MonoBehaviour, IModuleHandler, IDisposable
{
    private PlayerAnimation _playerAnimation;
    private Coroutine _animationTime;
    private float _attackingSpeed;
    private bool _isAttacking;
    private IEntityEvent _entityEvent;
    private bool _isDeath;

#if UNITY_ANDROID
    private Button _attackButton;
#endif

    public void OnInitialization(IParamArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        _playerAnimation = (PlayerAnimation)args.GetContainer()?[0] ?? null;

        if (_playerAnimation == null)
            throw new NullReferenceException(nameof(PlayerAnimation));

        _attackingSpeed = (float)args.GetContainer()?[1];

        if (_attackingSpeed <= 0)
            Debug.LogWarning("Attack speed is less than or equal to zero!");

#if UNITY_ANDROID
        _attackButton = (Button)args.GetContainer()?[2] ?? null;

        if (_attackButton == null)
            throw new NullReferenceException(nameof(Button));

        _attackButton.onClick.AddListener(InternalButtonAttackClickHandler);
#endif
        _entityEvent = (IEntityEvent)args.GetContainer()?[3] ?? null;

        if (_entityEvent == null)
            throw new NullReferenceException(nameof(IEntityEvent));

        _animationTime = StartCoroutine(AnimationRepeatWithTime());
        _isAttacking = false;
        _isDeath = false;
        _entityEvent.OnEnemyDeathEvent += PlayerDeathEvent;

        Debug.Log($"Module: {nameof(PlayerAttackCombat)} has been initialise.");
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

#if !UNITY_ANDROID
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
        {
            _playerAnimation.PlayAnimation(AnimationType.PlayAnimationAttack);
            _isAttacking = true;
        }
#endif
    }

    private IEnumerator AnimationRepeatWithTime()
    {
        while (true)
        {
            if (_isAttacking)
            {
                yield return new WaitForSeconds(_attackingSpeed / 2.0f);
                _isAttacking = false;
                _playerAnimation.PlayAnimation(AnimationType.PlayAnimationIdle);
            }

            yield return null;
        }
    }

    public void Dispose()
    {
        if (_animationTime != null)
            StopCoroutine(_animationTime);
#if UNITY_ANDROID
        _attackButton.onClick.RemoveAllListeners();
#endif
    }

#if UNITY_ANDROID
    private void InternalButtonAttackClickHandler()
    {
        if (_isDeath)
            return;

        if (!_isAttacking)
        {
            _playerAnimation.PlayAnimation(AnimationType.PlayAnimationAttack);
            _isAttacking = true;
        }
    }
#endif
}