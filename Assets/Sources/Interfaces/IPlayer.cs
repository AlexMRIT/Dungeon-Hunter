using UnityEngine;

public interface IPlayer
{
    public Transform GetPlayerTransform();
    public Specification GetPlayerSpecification();
    public void TakeDamage(float damage);
    public void InstallPlayerComponent(PlayerMovement playerMovement,
        PlayerAnimation playerAnimation, PlayerAttackCombat playerAttackCombat);
}