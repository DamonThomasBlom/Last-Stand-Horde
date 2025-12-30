using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/XP Upgrade")]
public class XPUpgradeSO : UpgradeSO
{
    public float bonus = .1f;

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddBonusXP(bonus);
    }
}
