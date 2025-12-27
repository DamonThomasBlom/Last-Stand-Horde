using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpValue = 1;
    public float baseSpeed = 10f;

    Transform player;
    float currentSpeed;
    bool magnetized;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentSpeed = baseSpeed;
    }

    private void OnEnable()
    {
        magnetized = false;
    }

    void Update()
    {
        if (magnetized && player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                currentSpeed * Time.deltaTime
            );
        }
    }

    // Called by PlayerMagnet
    public void Magnetize(float speedMultiplier)
    {
        magnetized = true;
        currentSpeed = baseSpeed * speedMultiplier;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerXP>().AddXP(xpValue);
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}