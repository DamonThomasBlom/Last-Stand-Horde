using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= StatManager.Instance.FireRate)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        ZombieController[] zombies = FindObjectsOfType<ZombieController>();
        if (zombies.Length == 0) return;

        ZombieController closest = null;
        float minDist = Mathf.Infinity;

        foreach (var z in zombies)
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
        PoolManager.Instance.Spawn(bulletPrefab, transform.position + dir, Quaternion.LookRotation(dir));
    }
}
