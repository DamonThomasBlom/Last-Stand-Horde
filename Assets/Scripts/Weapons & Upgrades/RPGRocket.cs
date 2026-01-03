using System.Collections;
using UnityEngine;

public class RPGRocket : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 7f;
    public float lifeTime = 2f;

    [Header("Explosion")]
    public float explosionRadius = 3f;
    public float explosionDamage = 20f;
    public float knockbackForce = 8f;
    public LayerMask enemyLayer;

    [Header("Debug")]
    public bool showDebugExplosion = true;
    public ExplodeVisual explodeVisualPrefab;

    float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer > lifeTime)
            Explode();
    }

    void OnTriggerEnter(Collider other)
    {
        // Hit anything → explode
        if (other.CompareTag("Enemy"))
            Explode();
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in hits)
        {
            if (hit.tag != "Enemy") continue;

            Health health = hit.GetComponent<Health>();
            if (health == null) continue;

            // Distance-based falloff (VERY IMPORTANT for balance)
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float t = Mathf.InverseLerp(explosionRadius, 0f, distance);
            float damage = explosionDamage * t * (1 + StatManager.Instance.rpgDamageMultiplier);

            Debug.Log("Rocket Damage - " + damage.ToString("F0"));
            health.TakeDamage(damage);

            ZombieController zc = hit.GetComponent<ZombieController>();
            if (zc != null)
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                zc.ApplyKnockback(dir, knockbackForce * t);
            }
        }

        // Optional debug visual
        if (showDebugExplosion)
            DebugDrawExplosion();

        PoolManager.Instance.Despawn(gameObject);
    }

    void ExplosionVisual()
    {
        ExplodeVisual explosion = Instantiate(explodeVisualPrefab, transform.position, Quaternion.identity);
        explosion.Explode(explosionRadius);
    }

    void DebugDrawExplosion()
    {
        // Draws a visible sphere for a short time
        Debug.DrawRay(transform.position, Vector3.up * explosionRadius, Color.red, 0.5f);
        ExplosionVisual();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!showDebugExplosion) return;

        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
