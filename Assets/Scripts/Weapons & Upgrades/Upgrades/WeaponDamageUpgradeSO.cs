using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Damage")]
public class WeaponDamageUpgradeSO : UpgradeSO
{
    public float bonusPerStack = 0.15f;

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddGunDamageBonus(bonusPerStack);
    }
}