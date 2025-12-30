using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;

    [Header("Spawn Area")]
    public float spawnRadius = 15f;

    [Header("Wave Timing")]
    public float waveDuration = 30f;
    public float restDuration = 3f;

    [Header("Spawn Rate")]
    public float baseMinSpawnInterval = 0.2f;
    public float baseMaxSpawnInterval = 1.5f;

    [Header("Wave Scaling")]
    public float spawnRateMultiplierPerWave = 0.85f; // lower = harder

    [Header("Pressure Curve")]
    public AnimationCurve currentPressureCurve;

    public List<AnimationCurve> pressureCurveList;

    [HideInInspector] public int CurrentWave;
    [HideInInspector] public float CurrentPressure;
    [HideInInspector] public float CurrentSpawnInterval;
    [HideInInspector] public float WaveProgress;
    [HideInInspector] public bool WaitingForClear;

    float waveTimer;
    float spawnTimer;
    public int waveIndex;
    bool waveActive;
    bool waitingForClear;

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        if (!waveActive)
            return;

        waveTimer += Time.deltaTime;

        WaveProgress = Mathf.Clamp01(waveTimer / waveDuration);
        CurrentPressure = currentPressureCurve.Evaluate(WaveProgress);

        float waveMultiplier = Mathf.Pow(spawnRateMultiplierPerWave, waveIndex);

        float minInterval = baseMinSpawnInterval * waveMultiplier;
        float maxInterval = baseMaxSpawnInterval * waveMultiplier;

        CurrentSpawnInterval = Mathf.Lerp(maxInterval, minInterval, CurrentPressure);

        spawnTimer += Time.deltaTime;

        if (!waitingForClear && spawnTimer >= CurrentSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnZombie();
        }

        // Peak reached → stop spawning
        if (WaveProgress >= 1f && !waitingForClear)
        {
            waitingForClear = true;
        }

        // Wait until player clears the horde
        if (waitingForClear)
        {
            if (ZombieController.all.Count == 0)
            {
                Invoke(nameof(StartNextWave), restDuration);
                waitingForClear = false;
                waveActive = false;
            }
        }

        CurrentWave = waveIndex;
        WaitingForClear = waitingForClear;
    }

    void SetRandomPressureCurve()
    {
        currentPressureCurve = pressureCurveList[Random.Range(0, pressureCurveList.Count)];
    }

    void SpawnZombie()
    {
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = player.position + new Vector3(circle.x, 0, circle.y);

        PoolManager.Instance.Spawn(zombiePrefab, pos, Quaternion.identity);
    }

    void StartWave()
    {
        SetRandomPressureCurve();
        waveActive = true;
        waveTimer = 0f;
        spawnTimer = 0f;
    }

    void StartNextWave()
    {
        waveIndex++;
        StartWave();
    }
}
