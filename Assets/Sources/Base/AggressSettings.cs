using System;

[Serializable]
public struct AggressSettings
{
    public AggressSettings(float distanceAggressive, float distanceAttack)
    {
        DistanceAggressive = distanceAggressive;
        DistanceAttack = distanceAttack;
    }

    public float DistanceAggressive;
    public float DistanceAttack;
}