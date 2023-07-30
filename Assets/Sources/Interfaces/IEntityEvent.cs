public interface IEntityEvent
{
    public event CallEvent OnEnemyDeathEvent;
    public event CallEvent<float> OnEnemyTakeDamage;
}