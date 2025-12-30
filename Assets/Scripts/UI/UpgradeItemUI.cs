using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public void Setup(UpgradeSO upgrade)
    {
        icon.sprite = upgrade.icon;
        icon.color = upgrade.iconColour;
        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            upgrade.TryApply(GameObject.FindWithTag("Player"));
            FindObjectOfType<UpgradeUI>().HidePanel();
        });
    }
}