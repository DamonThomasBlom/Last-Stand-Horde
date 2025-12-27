using UnityEngine;

public abstract class UpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;

    [Header("Stacking")]
    public int maxStacks = 5;

    [HideInInspector]
    public int currentStacks;

    public bool CanApply => currentStacks < maxStacks;

    public void TryApply(GameObject player)
    {
        if (!CanApply)
            return;

        Apply(player);
        currentStacks++;
    }

    public abstract void Apply(GameObject player);

    public virtual void ResetStacks()
    {
        currentStacks = 0;
    }
}
