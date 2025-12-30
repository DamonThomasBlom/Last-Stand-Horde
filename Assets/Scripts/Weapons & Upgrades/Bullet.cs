using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;

    float timer;

    void OnEnable() => timer = 0f;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer > lifeTime)
            PoolManager.Instance.Despawn(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>()?.TakeDamage(StatManager.Instance.GunDamage);
            other.GetComponent<ZombieController>()?.ApplyKnockback(transform.forward);

            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
