using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Damage Flash Settings")]
    public bool enableDamageFlash = true;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    public bool includeChildren = true; // Whether to include child objects

    private Renderer[] meshRenderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;
    private Health healthComponent;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (!enableDamageFlash) return;

        // Get the Health component
        healthComponent = GetComponent<Health>();

        if (healthComponent == null)
        {
            Debug.LogWarning($"DamageFlash on {gameObject.name}: No Health component found!");
            return;
        }

        // Subscribe to the OnTakeDamage event
        healthComponent.OnTakeDamage.AddListener(OnTakeDamageHandler);

        // Get mesh renderers
        if (includeChildren)
        {
            meshRenderers = GetComponentsInChildren<Renderer>();
        }
        else
        {
            meshRenderers = GetComponents<Renderer>();
        }

        // Only proceed if we found mesh renderers
        if (meshRenderers != null && meshRenderers.Length > 0)
        {
            originalColors = new Color[meshRenderers.Length];

            // Store original colors for each mesh renderer
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                if (meshRenderers[i] != null)
                {
                    originalColors[i] = meshRenderers[i].material.color;
                }
            }
        }
        else
        {
            Debug.LogWarning($"DamageFlash on {gameObject.name}: No MeshRenderer components found.");
            enableDamageFlash = false; // Disable if no renderers
        }
    }

    void OnTakeDamageHandler()
    {
        // Trigger the flash effect when damage is taken
        if (enableDamageFlash && meshRenderers != null && meshRenderers.Length > 0)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashEffect());
        }
    }

    // Coroutine to flash the mesh
    IEnumerator FlashEffect()
    {
        // Change all mesh renderers to flash color
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material.color = flashColor;
            }
        }

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Change back to original colors
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material.color = originalColors[i];
            }
        }

        flashCoroutine = null;
    }

    // Optional: Alternative method using material property blocks (more efficient)
    IEnumerator FlashEffectPropertyBlock()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        // Set flash color
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_Color", flashColor);
                meshRenderers[i].SetPropertyBlock(propertyBlock);
            }
        }

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Restore original colors
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor("_Color", originalColors[i]);
                meshRenderers[i].SetPropertyBlock(propertyBlock);
            }
        }

        flashCoroutine = null;
    }

    // Optional: Manually trigger flash from other scripts
    public void TriggerFlash(float customDuration = -1f)
    {
        if (!enableDamageFlash || meshRenderers == null || meshRenderers.Length == 0) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        if (customDuration > 0)
        {
            flashCoroutine = StartCoroutine(CustomFlashEffect(customDuration));
        }
        else
        {
            flashCoroutine = StartCoroutine(FlashEffect());
        }
    }

    IEnumerator CustomFlashEffect(float duration)
    {
        // Change all mesh renderers to flash color
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material.color = flashColor;
            }
        }

        // Wait for the custom duration
        yield return new WaitForSeconds(duration);

        // Change back to original colors
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material.color = originalColors[i];
            }
        }

        flashCoroutine = null;
    }

    // Clean up when disabled
    void OnDisable()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            RestoreOriginalColors();
            flashCoroutine = null;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (healthComponent != null)
        {
            healthComponent.OnTakeDamage.RemoveListener(OnTakeDamageHandler);
        }

        // Ensure colors are restored
        RestoreOriginalColors();
    }

    // Helper method to restore original colors
    void RestoreOriginalColors()
    {
        if (meshRenderers == null || originalColors == null) return;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i] != null)
            {
                meshRenderers[i].material.color = originalColors[i];
            }
        }
    }

    // Editor-only: Add a button to test the flash effect
    [ContextMenu("Test Flash Effect")]
    void TestFlash()
    {
        if (Application.isPlaying && enableDamageFlash)
        {
            TriggerFlash();
        }
    }
}
