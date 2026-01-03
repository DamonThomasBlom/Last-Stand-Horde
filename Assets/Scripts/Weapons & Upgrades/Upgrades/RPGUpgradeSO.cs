using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/RPG Weapon")]
public class RPGUpgradeSO : UpgradeSO
{
    public const string LVL1_DESCRIPTION = "Unlock RPG weapon";
    public const string EVEN_DESCRIPTION = "Fire an extra rocket";
    public const string ODD_DESCRIPTION = "Double your rocket damage";

    public float bonusDamage = 1f; // 100% Doubles damage

    public override void Apply(GameObject player)
    {
        // LEVEL 1 — UNLOCK WEAPON
        if (currentStacks == 0)
        {
            StatManager.Instance.UnlockRPG();
            description = EVEN_DESCRIPTION;
            return;
        }

        // EVEN LEVELS — EXTRA ROCKET
        if (currentStacks % 2 == 0)
        {
            StatManager.Instance.AddRpgRocket();
            description = ODD_DESCRIPTION;
        }
        // ODD LEVELS — DAMAGE
        else
        {
            StatManager.Instance.MultiplyRpgDamage(bonusDamage);
            description = EVEN_DESCRIPTION;
        }
    }

    private void OnEnable()
    {
        description = LVL1_DESCRIPTION;
    }
}
