using System;

[Serializable]
public struct Specification
{
    public Specification(float health, float moveSpeed,
        float attackSpeed, float damage, float defence)
    {
        Health = health;
        MoveSpeed = moveSpeed;
        AttackSpeed = attackSpeed;
        Damage = damage;
        Defence = defence;
    }

    public float Health;
    public float MoveSpeed;
    public float AttackSpeed;
    public float Damage;
    public float Defence;
}