using System.Collections.Generic;
using UnityEngine;

public class BladeDamage : MonoBehaviour
{
    public float debugDamage = 4f;
    public float hitCooldown = 0.25f;
    public float knockbackForce = 8f;

    private Dictionary<Health, float> hitTimers = new Dictionary<Health, float>();

    void Update()
    {
        // Tick down all timers
        var keys = new List<Health>(hitTimers.Keys);

        foreach (var health in keys)
        {
            hitTimers[health] -= Time.deltaTime;

            if (hitTimers[health] <= 0f)
                hitTimers.Remove(health);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        Health health = other.GetComponent<Health>();
        if (health == null) return;

        // Already hit recently
        if (hitTimers.ContainsKey(health))
            return;

        float damage = StatManager.Instance.SpinningBladeDamage;
        health.TakeDamage(damage);

        ZombieController zc = other.GetComponent<ZombieController>();
        if (zc != null)
        {
            Vector3 dir =
                (other.transform.position - transform.parent.position).normalized;
            zc.ApplyKnockback(dir, knockbackForce);
        }

        // Start cooldown for this enemy
        hitTimers.Add(health, hitCooldown);
    }
}
