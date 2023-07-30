using UnityEngine;

public static class Calculate
{
    public static float CalculateDamage(float damage, float armor)
    {
        float result = damage - armor;
        return result > 0 ? result : 1.0f;
    }

    public static float CalculateDistance(Transform first, Transform second)
    {
        Vector3 positionFirst = first.position;
        Vector3 positionSecond = second.position;

        return Vector3.Distance(positionFirst, positionSecond);
    }

    public static bool CheckForAggression(float distance, AggressSettings aggressSettings)
    {
        return distance <= aggressSettings.DistanceAggressive;
    }

    public static bool CheckForAttacking(float distance, AggressSettings aggressSettings)
    {
        return distance <= aggressSettings.DistanceAttack;
    }

    public static Vector3 GetDirection(Transform current, Transform target)
    {
        return new Vector3(
            target.position.x - current.position.x,
            0.0f,
            target.position.z - current.position.z);
    }
}