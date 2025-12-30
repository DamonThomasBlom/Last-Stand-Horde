using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationSmoothTime = 0.1f;

    [Header("References")]
    public Animator animator;

    public float isRunningThreshold = 0.1f;
    public float speedMultiplier = 1f;

    private Vector3 lastPosition;
    private float currentSpeed;
    private float speedVelocity; // For smoothing

    void Start()
    {
        lastPosition = transform.position;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        // Calculate how much we've moved since last frame
        Vector3 positionChange = transform.position - lastPosition;
        float rawSpeed = positionChange.magnitude / Time.deltaTime;

        // Smooth the speed value for animations
        currentSpeed = Mathf.SmoothDamp(currentSpeed, rawSpeed, ref speedVelocity, animationSmoothTime);

        // Update animator parameter
        if (animator != null)
        {
            if (currentSpeed > isRunningThreshold)
                animator.SetBool("Running", true);
            else
                animator.SetBool("Running", false);

            animator.SetFloat("Speed", Mathf.Max(currentSpeed * speedMultiplier, .5f));

            // Optional: Rotate to face movement direction if we're moving
            if (positionChange.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionChange.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                                                    10f * Time.deltaTime);
            }
        }

        // Store position for next frame
        lastPosition = transform.position;
    }
}
