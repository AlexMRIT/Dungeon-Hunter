public interface IEnemyEvent : IEntityEvent
{
    public event CallEvent<bool> OnEnemyAggressive;
}