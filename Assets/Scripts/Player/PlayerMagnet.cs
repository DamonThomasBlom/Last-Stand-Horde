using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class PlayerMagnet : MonoBehaviour
{
    List<XPOrb> nearbyOrbs = new();

    void Update()
    {
        float effectiveRange = StatManager.Instance.PickupRadius;

        // Find nearby orbs
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectiveRange);

        foreach (Collider collider in colliders)
        {
            XPOrb orb = collider.GetComponent<XPOrb>();
            if (orb != null && !nearbyOrbs.Contains(orb))
            {
                nearbyOrbs.Add(orb);
                orb.Magnetize();
            }
        }

        // Clean up list
        nearbyOrbs.RemoveAll(orb => orb.gameObject.activeSelf == true);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Only draw in editor when not playing
        if (EditorApplication.isPlaying)
            return;

        // Optional: Also check if StatManager exists
        if (StatManager.Instance == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, StatManager.Instance.PickupRadius);
    }
#endif
}