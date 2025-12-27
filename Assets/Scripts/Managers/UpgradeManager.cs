// UpgradeManager.cs
using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeSO> allUpgrades;
    [SerializeField] UpgradeUI upgradeUI;

    void Start()
    {
        // Reset all stacks
        foreach (var upgrade in allUpgrades)
        {
            upgrade.ResetStacks();
        }

        if (upgradeUI == null)
            upgradeUI = FindObjectOfType<UpgradeUI>();
    }

    public void ShowUpgrades()
    {
        // Pick 3 unique random upgrades
        List<UpgradeSO> choices = new();
        List<UpgradeSO> available = new List<UpgradeSO>(allUpgrades);

        // Remove the choices that are maxed
        available.RemoveAll(u => !u.CanApply);

        // Safety check
        if (available.Count == 0)
        {
            Debug.LogWarning("No upgrades available!");
            return;
        }

        // Get 3 unique upgrades (or less if not enough)
        int choicesToShow = Mathf.Min(3, available.Count);
        for (int i = 0; i < choicesToShow; i++)
        {
            int randomIndex = Random.Range(0, available.Count);
            choices.Add(available[randomIndex]);
            available.RemoveAt(randomIndex); // Prevent duplicates
        }

        // Show in UI
        upgradeUI.ShowUpgrades(choices.ToArray());
    }
}