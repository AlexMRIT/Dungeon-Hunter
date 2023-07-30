using UnityEngine;

#pragma warning disable

public sealed class EnemyGirl : BaseEnemy
{
    public override IPlayer Player => _player;

    private IPlayer _player;

    public override void Initialize(Specification specification, AggressSettings aggressSettings)
    {
        EnemySpecification = specification;
        EnemyAggressiveSettings = aggressSettings;

        OnEnemyDeathEvent += EnemyGirl_OnEnemyDeathEvent;
    }

    private void EnemyGirl_OnEnemyDeathEvent()
    {
        Dispose();

        OnEnemyDeathEvent -= EnemyGirl_OnEnemyDeathEvent;
    }

    public override void SetTarget(IPlayer player)
    {
        _player = player;
    }
}