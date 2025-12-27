using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 moveInput;

    void Update()
    {
        // Keyboard fallback (WASD)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(h, 0, v).normalized;
    }

    void FixedUpdate()
    {
        transform.position += moveInput * StatManager.Instance.MoveSpeed * Time.fixedDeltaTime;
    }
}
