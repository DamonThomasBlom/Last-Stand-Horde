using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spinning Blade")]
public class SpinningBladeUpgradeSO : UpgradeSO
{
    public float bonusDamage = 0.2f;

    public override void Apply(GameObject player)
    {
        StatManager.Instance.LevelUpBladeWeapon(bonusDamage);
    }
}
