using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    Dictionary<GameObject, Queue<GameObject>> pools = new();
    Dictionary<GameObject, GameObject> instanceToPrefab = new(); // Maps instances to their prefabs

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void BatchSpawn(GameObject prefab, int count)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject newPooledItem = Instantiate(prefab);
            newPooledItem.SetActive(false);

            // Store reference to prefab
            if (!instanceToPrefab.ContainsKey(newPooledItem))
                instanceToPrefab.Add(newPooledItem, prefab);

            pools[prefab].Enqueue(newPooledItem);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        GameObject obj;

        // Try to get from pool
        while (pools[prefab].Count > 0)
        {
            obj = pools[prefab].Dequeue();

            // Safety check - object might have been destroyed elsewhere
            if (obj == null)
                continue;

            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);

            // Ensure mapping exists
            if (!instanceToPrefab.ContainsKey(obj))
                instanceToPrefab.Add(obj, prefab);

            return obj;
        }

        // Create new if pool is empty
        obj = Instantiate(prefab, pos, rot);

        // Store reference to prefab
        if (!instanceToPrefab.ContainsKey(obj))
            instanceToPrefab.Add(obj, prefab);

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);

        // Find which prefab this instance belongs to
        if (instanceToPrefab.TryGetValue(obj, out GameObject prefab))
        {
            if (!pools.ContainsKey(prefab))
                pools[prefab] = new Queue<GameObject>();

            pools[prefab].Enqueue(obj);
        }
        else
        {
            // Object wasn't created through pool system
            Debug.LogWarning($"PoolManager: Object {obj.name} was not spawned through pool. Destroying instead.");
            Destroy(obj);
        }
    }

    // Optional helper method
    public void DespawnAfterDelay(GameObject obj, float delay)
    {
        StartCoroutine(DelayedDespawn(obj, delay));
    }

    IEnumerator DelayedDespawn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Despawn(obj);
    }

    // Clean up destroyed objects from tracking
    void Update()
    {

    }
}