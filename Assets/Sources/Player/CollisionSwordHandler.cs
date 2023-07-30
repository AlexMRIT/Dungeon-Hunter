using UnityEngine;

internal sealed class CollisionSwordHandler : MonoBehaviour
{
    [SerializeField] private BasePlayer _basePlayer;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out BaseEnemy baseEnemy))
        {
            baseEnemy.TakeDamage(_basePlayer.GetPlayerSpecification().Damage);
        }
    }
}