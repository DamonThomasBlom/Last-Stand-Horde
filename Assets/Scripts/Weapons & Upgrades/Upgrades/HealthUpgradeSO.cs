// HealthUpgradeSO.cs
using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Upgrades/Health")]
public class HealthUpgradeSO : UpgradeSO
{
    public int healthIncrease = 25;
    public int maxHealthIncrease = 25;

    public override void Apply(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        if (health != null)
        {
            health.maxHealth += maxHealthIncrease;
            health.AddHealth(healthIncrease);
            Debug.Log($"Health increased! Max: {health.maxHealth}, Current: {health.GetHealth()}");
        }
    }
}