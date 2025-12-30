using System.Collections;
using UnityEngine;

public class ExplodeVisual : MonoBehaviour
{
    public float explosionRadius;

    public void Explode(float explosionRadius)
    {
        this.explosionRadius = explosionRadius;
        StartCoroutine(ExplosionVisual());
    }

    IEnumerator ExplosionVisual()
    {
        float t = 0f;

        while (t < 0.2f)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0f, explosionRadius * 2f, t / 0.2f);
            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        Destroy(gameObject);
    }
}
