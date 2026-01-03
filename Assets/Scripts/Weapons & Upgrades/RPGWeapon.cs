using System.Collections;
using UnityEngine;

public class RPGWeapon : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float fireRate = 5f;
    public float multipleRocketDelay = 1.5f;
    public LayerMask enemyLayer;
    public float targetSearchRadius = 15f;
    public float clumpRadius = 2.5f;

    float timer;

    private bool isFiring;

    void Update()
    {
        if (!StatManager.Instance.hasRPG || isFiring)
            return;

        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            timer = 0f;
            StartCoroutine(FireRocketsWithDelay());
        }
    }

    IEnumerator FireRocketsWithDelay()
    {
        isFiring = true;

        int rocketCount = StatManager.Instance.rpgRocketCount;

        for (int i = 0; i < rocketCount; i++)
        {
            FireRocket();
            yield return new WaitForSeconds(multipleRocketDelay);
        }

        isFiring = false;
    }

    void FireRocket()
    {
        Transform target = ZombieTargetingUtility.FindBestClumpTarget(
            transform.position,
            targetSearchRadius,
            clumpRadius,
            enemyLayer
        );

        if (target == null)
            return;

        GameObject rocket = PoolManager.Instance.Spawn(
            rocketPrefab,
            transform.position,
            Quaternion.identity
        );

        Vector3 dir = (target.position - transform.position).normalized;
        rocket.transform.forward = dir;
    }

    void Shoot()
    {
        if (ZombieController.all.Count == 0) return;

        ZombieController closest = null;
        float minDist = Mathf.Infinity;

        foreach (var z in ZombieController.all)
        {
            float d = Vector3.Distance(transform.position, z.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = z;
            }
        }

        if (closest == null || minDist > StatManager.Instance.BulletRange) return;

        Vector3 dir = (closest.transform.position - transform.position).normalized;
        PoolManager.Instance.Spawn(rocketPrefab, transform.position + dir, Quaternion.LookRotation(dir));
    }
}
