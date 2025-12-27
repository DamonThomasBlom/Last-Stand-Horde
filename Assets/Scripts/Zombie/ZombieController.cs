using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float speed = 2f;
    public float separationRadius = 1f;
    public float separationStrength = 2f;
    public float knockbackForce = 4f;
    public GameObject XPOrbPrefab;

    [Header("Damage Settings")]
    public int damage = 1;
    public float attackCooldown = 1f; // Can attack once per second
    public float attackRange = 1.5f; // How close they need to be to attack

    Vector3 knockbackVelocity;
    Transform player;
    float startY; // Store initial Y position
    float attackTimer;

    static List<ZombieController> all = new();

    void OnEnable()
    {
        all.Add(this);
        attackTimer = 0f;
    }

    void OnDisable() => all.Remove(this);

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        GetComponentInChildren<Health>().OnDie.AddListener(OnDie);
        startY = transform.position.y; // Store the initial Y position
    }

    void OnDie()
    {
        PoolManager.Instance.Spawn(XPOrbPrefab, transform.position, Quaternion.identity);
    }

    public void ApplyKnockback(Vector3 dir)
    {
        knockbackVelocity = dir * knockbackForce;
        knockbackVelocity.y = 0; // Ensure knockback is horizontal only
    }

    void Update()
    {
        // Get player position but ignore Y difference
        Vector3 playerPos = player.position;
        playerPos.y = startY; // Use zombie's Y level, not player's

        Vector3 moveDir = (playerPos - transform.position).normalized;
        Vector3 separation = Vector3.zero;

        foreach (var other in all)
        {
            if (other == this) continue;

            Vector3 diff = transform.position - other.transform.position;
            float dist = diff.magnitude;

            if (dist < separationRadius && dist > 0)
            {
                diff.y = 0; // Make separation horizontal only
                separation += diff.normalized * (1 - dist / separationRadius);
            }
        }

        Vector3 finalDir = (moveDir + separation * separationStrength).normalized;
        Vector3 velocity = finalDir * speed + knockbackVelocity;
        velocity.y = 0; // Ensure no vertical movement

        // Move and lock Y position
        Vector3 newPos = transform.position + velocity * Time.deltaTime;
        newPos.y = startY; // Lock to original Y position
        transform.position = newPos;

        // Decay knockback
        knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, 10f * Time.deltaTime);
        knockbackVelocity.y = 0; // Keep knockback horizontal

        // Handle attacking
        attackTimer -= Time.deltaTime;
        TryAttackPlayer();
    }

    void TryAttackPlayer()
    {
        // Check if player is in range and cooldown is ready
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackCooldown; // Reset cooldown
        }
    }

    void AttackPlayer()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            // Optional: Apply small knockback to player
            //Vector3 knockbackDir = (player.position - transform.position).normalized;
            //knockbackDir.y = 0;

            // If player has a PlayerMovement script with knockback support
            //PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            //if (playerMovement != null)
            //    playerMovement.ApplyKnockback(knockbackDir * 2f); // Smaller knockback than bullets

            Debug.Log($"Zombie hit player for {damage} damage!");
        }
    }

    // Visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}