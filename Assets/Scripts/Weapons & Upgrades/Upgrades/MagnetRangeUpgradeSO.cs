// MagnetRangeUpgradeSO.cs
using UnityEngine;

[CreateAssetMenu(fileName = "MagnetRangeUpgrade", menuName = "Upgrades/Magnet Range")]
public class MagnetRangeUpgradeSO : UpgradeSO
{
    public float bonus = 1f; // 100%

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddPickupRadiusBonus(bonus);
    }
}