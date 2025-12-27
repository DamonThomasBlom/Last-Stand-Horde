using UnityEngine;

public class CameraZoomOutOverTime : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;
    public Transform player;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f; // Units per second
    public float maxZoomOrth = 15;
    public float minZoomOrth = 5;
    public float maxZoomPers = 15;
    public float minZoomPers = 5;
    public bool zoomEnabled = true;

    [Header("Offset (for Perspective)")]
    public Vector3 cameraOffset = new Vector3(0, 10, -10); // For perspective cameras

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    private float currentZoom;
    private float timeElapsed;

    float maxZoom = 20f;    // Maximum zoom distance/ortho size
    float minZoom = 5f;     // Starting zoom distance/ortho size

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        // Initialize based on camera type
        if (cam.orthographic)
        {
            maxZoom = maxZoomOrth;
            minZoom = minZoomOrth;

            currentZoom = cam.orthographicSize;
            minZoom = Mathf.Min(minZoom, currentZoom);
        }
        else
        {
            maxZoom = maxZoomPers;
            minZoom = minZoomPers;

            // For perspective, zoom is distance from player
            currentZoom = Vector3.Distance(transform.position, player.position);
            minZoom = Mathf.Min(minZoom, currentZoom);
        }
    }

    void Update()
    {
        if (!zoomEnabled || player == null) return;

        timeElapsed += Time.deltaTime;

        // Calculate target zoom based on time
        float targetZoom = Mathf.Min(maxZoom, minZoom + (timeElapsed * zoomSpeed));

        // Smoothly interpolate current zoom
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, smoothSpeed * Time.deltaTime);

        ApplyZoom();
    }

    void ApplyZoom()
    {
        if (cam.orthographic)
        {
            // Orthographic camera: adjust orthographicSize
            cam.orthographicSize = currentZoom;
        }
        else
        {
            // Perspective camera: adjust distance while maintaining offset angle
            Vector3 desiredPosition = player.position + (cameraOffset.normalized * currentZoom);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }

    // Alternative: Zoom based on player level or wave
    public void ZoomBasedOnWave(int wave)
    {
        float targetZoom = Mathf.Min(maxZoom, minZoom + (wave * zoomSpeed));
        currentZoom = targetZoom;
        ApplyZoom();
    }

    // Manual zoom control
    public void SetZoom(float zoomAmount)
    {
        currentZoom = Mathf.Clamp(zoomAmount, minZoom, maxZoom);
        ApplyZoom();
    }

    // Reset zoom
    public void ResetZoom()
    {
        currentZoom = minZoom;
        timeElapsed = 0f;
        ApplyZoom();
    }

    // Toggle zoom
    public void ToggleZoom(bool enable)
    {
        zoomEnabled = enable;
    }

    // For debugging in editor
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, currentZoom);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, maxZoom);
        }
    }
}
