using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Health Regen")]
public class HealthRegenUpgradeSO : UpgradeSO
{
    public float regenPerStack = 2.5f;

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddHealthRegen(regenPerStack);
    }
}
