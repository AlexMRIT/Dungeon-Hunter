using System;
using UnityEngine;
using System.Collections;

#pragma warning disable

public delegate void CallEvent();
public delegate void CallEvent<in T>(T obj);

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public abstract class BaseEnemy : MonoBehaviour, IDisposable, IEnemyEvent, ISpecification
{
    [SerializeField] private Transform _swordCollision;

    [HideInInspector] public Specification EnemySpecification;
    [HideInInspector] public AggressSettings EnemyAggressiveSettings;

    public abstract IPlayer Player { get; }

    public abstract void Initialize(Specification specification, AggressSettings aggressSettings);

    public event CallEvent OnEnemyDeathEvent;
    public event CallEvent<float> OnEnemyTakeDamage;
    public event CallEvent<bool> OnEnemyAggressive;

    public virtual void TakeDamage(float damage)
    {
        EnemySpecification.Health -= Calculate.CalculateDamage(damage, EnemySpecification.Defence);
        OnEnemyTakeDamage?.Invoke(EnemySpecification.Health);

        if (EnemySpecification.Health <= 0)
        {
            InternalPlayAnimation(AnimationType.PlayAnimationDeath);
            OnEnemyDeathEvent?.Invoke();
        }
    }

    public abstract void SetTarget(IPlayer player);

    private AnimationType _lastAnimationIvoke;
    private bool _isAttacking;
    private bool _isFirstAggressive;
    private Coroutine _animationTime;
    private Animator _animator;
    private CharacterController _characterController;

    public virtual void Setup()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        OnEnemyAggressive?.Invoke(_isFirstAggressive);

        _animationTime = StartCoroutine(AnimationRepeatWithTime());
    }

    public virtual void Update()
    {
        float distance = Calculate.CalculateDistance(transform, Player.GetPlayerTransform());
        Vector3 direction = Calculate.GetDirection(transform, Player.GetPlayerTransform());
        bool checkAttacking = false;
        bool checkAggression = false;

        if (checkAttacking = Calculate.CheckForAttacking(distance, EnemyAggressiveSettings) && !_isAttacking)
        {
            InternalPlayAnimation(AnimationType.PlayAnimationAttack);
            _isAttacking = true;
        }
        else if (checkAggression = Calculate.CheckForAggression(distance, EnemyAggressiveSettings) && !_isAttacking)
        {
            _characterController.Move(direction * EnemySpecification.MoveSpeed * Time.deltaTime);

            InternalPlayAnimation(AnimationType.PlayAnimationRun);

            if (!_isFirstAggressive)
            {
                _isFirstAggressive = true;
                OnEnemyAggressive?.Invoke(_isFirstAggressive);
            }
        }
        else if (!_isAttacking)
        {
            InternalPlayAnimation(AnimationType.PlayAnimationIdle);

            if (_isFirstAggressive)
            {
                _isFirstAggressive = false;
                OnEnemyAggressive?.Invoke(_isFirstAggressive);
            }
        }

        if (checkAttacking || checkAggression)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards
                (transform.forward, direction, EnemySpecification.MoveSpeed, 0f));
        }
    }

    public void AttackEvent(int status)
    {
        _swordCollision.gameObject.SetActive(Convert.ToBoolean(status));
    }

    protected void InternalPlayAnimation(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.PlayAnimationIdle:
                _animator.SetFloat("idleSpeed", 1.0f);
                _animator.Play("idle");
                break;

            case AnimationType.PlayAnimationRun:
                _animator.SetFloat("runSpeed", 1.0f);
                _animator.Play("run");
                break;

            case AnimationType.PlayAnimationAttack:
                _animator.SetFloat("attackSpeed", EnemySpecification.AttackSpeed);
                _animator.Play("attack");
                break;
            case AnimationType.PlayAnimationDeath:
                _animator.Play("death");
                break;
        }

        _lastAnimationIvoke = animationType;
    }

    private IEnumerator AnimationRepeatWithTime()
    {
        while (true)
        {
            if (_isAttacking)
            {
                yield return new WaitForSeconds(EnemySpecification.AttackSpeed / 2.0f);
                _isAttacking = false;
                InternalPlayAnimation(AnimationType.PlayAnimationIdle);
            }

            yield return null;
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, EnemyAggressiveSettings.DistanceAggressive);
    }

    public void Dispose()
    {
        if (_animationTime != null)
            StopCoroutine(_animationTime);
        Destroy(this);
    }

    public Specification GetSpecification()
    {
        return EnemySpecification;
    }
}