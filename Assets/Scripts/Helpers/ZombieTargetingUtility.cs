using UnityEngine;

public static class ZombieTargetingUtility
{
    public static Transform FindBestClumpTarget(
        Vector3 playerPos,
        float maxSearchRadius,
        float clumpRadius,
        LayerMask enemyLayer)
    {
        Collider[] zombies = Physics.OverlapSphere(
            playerPos,
            maxSearchRadius,
            enemyLayer
        );

        Transform bestTarget = null;
        float bestScore = 0f;

        foreach (Collider z in zombies)
        {
            int nearbyCount = Physics.OverlapSphere(
                z.transform.position,
                clumpRadius,
                enemyLayer
            ).Length;

            float distance = Vector3.Distance(playerPos, z.transform.position);
            float score = nearbyCount / Mathf.Max(distance, 1f);

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = z.transform;
            }
        }

        return bestTarget;
    }
}
