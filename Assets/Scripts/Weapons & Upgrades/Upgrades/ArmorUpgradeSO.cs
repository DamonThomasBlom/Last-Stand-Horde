using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Armor")]
public class ArmorUpgradeSO : UpgradeSO
{
    public float reductionPerStack = 0.1f;

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddDamageReduction(reductionPerStack);
    }
}
