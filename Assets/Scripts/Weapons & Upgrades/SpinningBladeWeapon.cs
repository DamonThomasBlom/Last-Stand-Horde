using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBladeWeapon : MonoBehaviour
{
    [Header("Blade Settings")]
    public GameObject bladePrefab;
    public float radius = 2.2f;
    public float rotationSpeed = 180f;
    public float activeTime = 10f;
    public float deactiveTime = 3f;

    [Header("Runtime")]
    public int bladeCount = 1;

    List<GameObject> blades = new();

    float currentRadius;
    Coroutine cycleCoroutine;

    //void Start()
    //{
    //    RebuildBlades();
    //    StartCycle();
    //}

    public void ActivateWeapon()
    {
        bladeCount = 2;
        RebuildBlades();
        StartCycle();
    }

    void StartCycle()
    {
        if (cycleCoroutine != null) StopCoroutine(cycleCoroutine);
        cycleCoroutine = StartCoroutine(BladeCycle());
    }

    IEnumerator BladeCycle()
    {
        while (true)
        {
            yield return StartCoroutine(ActivateBlades());
            yield return new WaitForSeconds(activeTime);
            yield return StartCoroutine(DeactivateBlades());
            yield return new WaitForSeconds(deactiveTime);
        }
    }

    void Update()
    {
        RotateBlades();
    }

    void RotateBlades()
    {
        for (int i = 0; i < blades.Count; i++)
        {
            float angle = (360f / blades.Count) * i + Time.time * rotationSpeed;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * currentRadius;

            blades[i].transform.localPosition = offset;
        }
    }

    public void AddBlade()
    {
        bladeCount++;
        RebuildBlades();
    }

    void RebuildBlades()
    {
        foreach (var blade in blades)
            Destroy(blade);

        blades.Clear();

        for (int i = 0; i < bladeCount; i++)
        {
            GameObject blade = Instantiate(bladePrefab, transform);
            blades.Add(blade);
        }
    }

    IEnumerator ActivateBlades()
    {
        foreach (var blade in blades) { blade.SetActive(true); }

        float expandTimeElapse = 0;
        float expandTime = 2f;

        while(expandTimeElapse < expandTime)
        {
            expandTimeElapse += Time.deltaTime;
            currentRadius = Mathf.Lerp(0, radius, expandTimeElapse / expandTime);

            yield return null;
        }

        currentRadius = radius;
    }

    IEnumerator DeactivateBlades()
    {
        float expandTimeElapse = 0;
        float expandTime = 2f;

        while (expandTimeElapse < expandTime)
        {
            expandTimeElapse += Time.deltaTime;
            currentRadius = Mathf.Lerp(radius, 0, expandTimeElapse / expandTime);

            yield return null;
        }

        currentRadius = 0;

        foreach (var blade in blades) { blade.SetActive(false); }
    }
}
