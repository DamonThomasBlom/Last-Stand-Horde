using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Fire Rate")]
public class FireRateUpgrade : UpgradeSO
{
    public float bonus = 0.2f; // +20%

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddFireRateBonus(bonus);
    }
}
