using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 moveInput;
    Health playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    void Update()
    {
        // Keyboard fallback (WASD)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(h, 0, v).normalized;

        // Move in Update so camera & movement sync
        transform.position += moveInput * StatManager.Instance.MoveSpeed * Time.deltaTime;

        playerHealth.Heal(StatManager.Instance.healthRegenPerSecond * Time.deltaTime);
        playerHealth.damageReduction = StatManager.Instance.damageReductionBonus;
    }

    //void FixedUpdate()
    //{
    //    transform.position += moveInput * StatManager.Instance.MoveSpeed * Time.fixedDeltaTime;
    //}
}
