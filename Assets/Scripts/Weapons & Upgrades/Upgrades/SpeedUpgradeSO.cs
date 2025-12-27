// SpeedUpgradeSO.cs
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/Speed")]
public class SpeedUpgradeSO : UpgradeSO
{
    public float bonus = 0.1f; // 10%

    public override void Apply(GameObject player)
    {
        StatManager.Instance.AddMoveSpeedBonus(bonus);
    }
}