using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAttackCombat))]
public sealed class BasePlayer : MonoBehaviour, IModuleHandler, IPlayer, ISpecification, IEntityEvent
{
    [SerializeField] private Transform _swordCollision;

    private PlayerMovement _playerMovement;
    private PlayerAnimation _playerAnimation;
    private PlayerAttackCombat _playerAttackCombat;
    private Specification _specification;

    public event CallEvent OnEnemyDeathEvent;
    public event CallEvent<float> OnEnemyTakeDamage;

    public void OnInitialization(IParamArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(IParamArgs));

        _specification = (Specification)args.GetContainer()?[0];

        if (_specification.Health <= 0)
            Debug.LogWarning($"Player: {nameof(BasePlayer)} has 0 percent of life.");

        Debug.Log($"Module: {nameof(BasePlayer)} has been initialise.");
    }

    public void InstallPlayerComponent(PlayerMovement playerMovement,
        PlayerAnimation playerAnimation, PlayerAttackCombat playerAttackCombat)
    {
        _playerMovement = playerMovement;
        _playerAnimation = playerAnimation;
        _playerAttackCombat = playerAttackCombat;
    }

    public void AttackEvent(int status)
    {
        _swordCollision.gameObject.SetActive(Convert.ToBoolean(status));
    }

    public Specification GetPlayerSpecification()
    {
        return _specification;
    }

    public void TakeDamage(float damage)
    {
        _specification.Health -= Calculate.CalculateDamage(damage, _specification.Defence);
        OnEnemyTakeDamage?.Invoke(_specification.Health);

        if (_specification.Health <= 0)
        {
            OnEnemyDeathEvent?.Invoke();
            InternalDeath();
        }
    }

    private void InternalDeath()
    {
        _playerAnimation.PlayAnimation(AnimationType.PlayAnimationDeath);
        StartCoroutine(InternalReturnInSpawnPoint());
    }

    private IEnumerator InternalReturnInSpawnPoint()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("EnvDungL1");
    }

    public Specification GetSpecification()
    {
        return _specification;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }
}