using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;
    public float spawnRadius = 10f;
    public float spawnRate = 1f;
    public float increaseZombieCountTime = 10; // Increase zombie count every x seconds

    int zombieSpawnCount = 1;
    float spawnTimer;
    float increaseZombieCountTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        increaseZombieCountTimer += Time.deltaTime;

        // Increase zombie spawn count for more difficulty
        if (increaseZombieCountTimer > increaseZombieCountTime)
        {
            increaseZombieCountTimer = 0;
            zombieSpawnCount++;
        }

        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0f;

            for (int i = 0; i < zombieSpawnCount; i++) 
            {
                SpawnZombie();
            }
        }
    }

    void SpawnZombie()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = player.position + new Vector3(circle.x, 0, circle.y);

        PoolManager.Instance.Spawn(zombiePrefab, pos, Quaternion.identity);
    }
}
