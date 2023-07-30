using UnityEngine;

internal sealed class CollisionSwordHandlerForEnemy : MonoBehaviour
{
    [SerializeField] private BaseEnemy _basePlayer;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out BasePlayer baseEnemy))
        {
            baseEnemy.TakeDamage(_basePlayer.EnemySpecification.Damage);
        }
    }
}