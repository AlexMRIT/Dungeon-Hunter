using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class StatsBar : MonoBehaviour
{
    [SerializeField] private GameObject _canvasStats;
    [SerializeField] private Slider _healthBar;

    private ISpecification _specification;
    private IEntityEvent _entityEvent;
    private float _originalHealth;

    private void Start()
    {
        _specification = GetComponent<ISpecification>();
        _entityEvent = GetComponent<IEntityEvent>();

        _originalHealth = _specification.GetSpecification().Health;
        _entityEvent.OnEnemyTakeDamage += StatsEnemyTakeDamage;
    }

    private void OnDisable()
    {
        _entityEvent.OnEnemyTakeDamage -= StatsEnemyTakeDamage;
    }

    private void StatsEnemyTakeDamage(float health)
    {
        try
        {
            _healthBar.value = unchecked(health / _originalHealth);

            if (health <= 0)
            {
                _canvasStats.SetActive(false);
                Destroy(this);
            }
        }
        catch (DivideByZeroException divideException)
        {
            Debug.LogError(divideException.Message);
        }
    }
}