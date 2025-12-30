using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 3;
    public float damageReduction = 0f; // Basically armor 0-1 (100%)
    public GameObject poolPrefab;
    public Slider healthSlider;

    [Header("Events")]
    public UnityEvent OnDie = new UnityEvent();
    public UnityEvent OnTakeDamage = new UnityEvent();

    [Header("Dubug")]
    public bool invincible;

    float currentHealth;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public float GetHealth() { return currentHealth; }

    public void AddHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    public void SetFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(float dmg)
    {
        if (invincible) return;

        dmg *= (1f - damageReduction);

        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
        else
            OnTakeDamage.Invoke();

        UpdateHealthSlider();
    }

    public void Heal(float health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    void Die()
    {
        OnDie.Invoke();

        if (poolPrefab != null)
        {
            PoolManager.Instance.Despawn(gameObject);
            return;
        }

        //Destroy(gameObject);
    }

    void UpdateHealthSlider()
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
