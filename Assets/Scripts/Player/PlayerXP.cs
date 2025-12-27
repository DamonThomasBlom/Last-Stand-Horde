using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    public int level = 1;
    public int currentXP;
    public int xpToNext = 5;

    [Header("UI References")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText; // Optional: shows "XP: 120/200"

    public UpgradeManager upgradeManager;

    void Start()
    {
        if (upgradeManager == null)
            upgradeManager = FindObjectOfType<UpgradeManager>();

        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= xpToNext)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNext;
        xpToNext = Mathf.RoundToInt(xpToNext * 1.5f); // Scale difficulty

        // Show upgrade choices
        if (upgradeManager != null)
            upgradeManager.ShowUpgrades();
        else
            Debug.LogWarning("No UpgradeManager found!");

        Debug.Log($"Level Up! Now level {level}");
    }

    private void UpdateUI()
    {
        // Update level text with "Lvl: 23" format
        if (levelText != null)
            levelText.text = $"LVL: {level}";

        // Update XP slider
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNext;
            xpSlider.value = currentXP;
        }

        // Update XP text (optional)
        if (xpText != null)
            xpText.text = $"XP: {currentXP}/{xpToNext}";
    }
}
