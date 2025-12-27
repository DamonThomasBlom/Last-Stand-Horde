// UpgradeUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;
    [SerializeField] UpgradeItemUI[] upgradeButtons;

    void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades(UpgradeSO[] choices)
    {
        upgradePanel.SetActive(true);

        for (int i = 0; i < upgradeButtons.Length && i < choices.Length; i++)
        {
            upgradeButtons[i].Setup(choices[i]);
        }

        Time.timeScale = 0f; // Pause game
    }

    public void HidePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}