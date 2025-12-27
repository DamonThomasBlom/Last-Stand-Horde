using UnityEngine;

public class StatManager : MonoBehaviour
{
    #region SINGLETON

    public static StatManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [Header("Base Stats")]
    //public int baseProjectiles = 1;

    public float baseDamage = 1f;
    public float baseFireRate = 0.5f; // seconds between shots
    public float baseBulletRange = 10f;
    public float baseMoveSpeed = 5f;
    public float basePickupRadius = 2.5f;

    [Header("Bonus Stats (Additive %)")]
    [Range(0, 5)] public float damageBonus;        // 0.25 = +25%
    [Range(0, 5)] public float fireRateBonus;      // affects cooldown
    [Range(0, 5)] public float moveSpeedBonus;
    [Range(0, 5)] public float pickupRadiusBonus;
    [Range(0, 0.9f)] public float damageReductionBonus;

    [Header("Flat Bonuses")]
    public int projectileBonus;
    public float healthRegenPerSecond;

    [Header("Caps")]
    public float maxWeaponDamageBonus = 1.5f; // +150%
    public float maxProjectiles = 6;
    public float maxMoveSpeedBonus = 0.5f; // +50%
    public float maxFireRateBonus = 1.5f;  // soft cap
    public float maxHealthRegen = 20f;
    public float maxDamageReduction = 0.6f;

    // ---- FINAL CALCULATED STATS ----

    public float GunDamage =>
        baseDamage * (1f + Mathf.Min(damageBonus, maxWeaponDamageBonus));

    public float FireRate =>
        baseFireRate / (1f + Mathf.Min(fireRateBonus, maxFireRateBonus));

    public float BulletRange =>
    baseBulletRange;

    //public int Projectiles =>
    //    Mathf.Min(baseProjectiles + projectileBonus, maxProjectiles);

    public float MoveSpeed =>
        baseMoveSpeed * (1f + Mathf.Min(moveSpeedBonus, maxMoveSpeedBonus));

    public float PickupRadius =>
        basePickupRadius * (1f + pickupRadiusBonus);

    // ---- UPGRADE METHODS ----

    public void AddGunDamageBonus(float percent)
    {
        damageBonus += percent;
    }

    public void AddFireRateBonus(float percent)
    {
        fireRateBonus += percent;
    }

    public void AddProjectile()
    {
        projectileBonus++;
    }

    public void AddMoveSpeedBonus(float percent)
    {
        moveSpeedBonus += percent;
    }

    public void AddPickupRadiusBonus(float percent)
    {
        pickupRadiusBonus += percent;
    }

    public void AddHealthRegen(float amount)
    {
        healthRegenPerSecond = Mathf.Min(healthRegenPerSecond + amount, maxHealthRegen);
    }

    public void AddDamageReduction(float amount)
    {
        damageReductionBonus = Mathf.Min(damageReductionBonus + amount, maxDamageReduction);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        damageBonus = Mathf.Max(0, damageBonus);
        fireRateBonus = Mathf.Max(0, fireRateBonus);
        moveSpeedBonus = Mathf.Max(0, moveSpeedBonus);
        pickupRadiusBonus = Mathf.Max(0, pickupRadiusBonus);
    }
#endif
}
