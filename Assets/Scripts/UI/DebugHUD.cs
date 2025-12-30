using TMPro;
using UnityEngine;

public class DebugHUD : MonoBehaviour
{
    [Header("UI References")]
    public GameObject debugPanel;
    public TMP_Text debugText;

    [Header("Player References")]
    public PlayerXP playerXP;
    public Health playerHealth;
    public MonoBehaviour playerShooting;
    public MonoBehaviour playerMovement;

    [Header("Game References")]
    public ZombieSpawner spawner;
    public UpgradeManager upgradeManager;

    // Toggle key
    public KeyCode toggleKey = KeyCode.F3;

    public bool startOn;

    void Start()
    {
        if (debugPanel != null)
            debugPanel.SetActive(false);

        // Auto-find references if not set
        if (playerXP == null) playerXP = FindObjectOfType<PlayerXP>();
        if (playerHealth == null) playerHealth = FindObjectOfType<Health>();
        if (spawner == null) spawner = FindObjectOfType<ZombieSpawner>();
        if (upgradeManager == null) upgradeManager = FindObjectOfType<UpgradeManager>();

        debugPanel.SetActive(startOn);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (debugPanel != null)
                debugPanel.SetActive(!debugPanel.activeSelf);
        }

        if (debugPanel != null && debugPanel.activeSelf)
        {
            UpdateDebugText();
        }
    }

    void UpdateDebugText()
    {
        if (debugText == null) return;

        string debugInfo = "";

        // Game Time
        debugInfo += $"<size=50><b>BALANCE DEBUG HUD</b></size>\n";
        debugInfo += $"<color=yellow>Game Time:</color> {Time.timeSinceLevelLoad:F1}s\n";
        debugInfo += $"<color=yellow>Time Scale:</color> {Time.timeScale:F2}\n\n";

        // Player Stats
        debugInfo += $"<size=40><b>PLAYER STATS</b></size>\n";
        if (playerHealth != null)
        {
            debugInfo += $"<color=#00FF00>Health:</color> {playerHealth.GetHealth().ToString("F0")}/{playerHealth.maxHealth} ";
            debugInfo += $"({(playerHealth.GetHealth() / (float)playerHealth.maxHealth) * 100:F0}%)\n";
        }

        if (playerXP != null)
        {
            debugInfo += $"<color=#FF9900>Level:</color> {playerXP.level} ";
            debugInfo += $"<color=#FF9900>XP:</color> {playerXP.currentXP}/{playerXP.xpToNext} ";
            debugInfo += $"({(playerXP.currentXP / (float)playerXP.xpToNext) * 100:F0}%)\n";
        }

        if (StatManager.Instance != null)
        {
            var stats = StatManager.Instance;

            debugInfo += $"<color=#FF4444>Damage:</color> {stats.GunDamage:F1}\n";
            debugInfo += $"<color=#FF4444>Fire Rate:</color> {stats.FireRate:F2}s ";
            debugInfo += $"({1f / stats.FireRate:F1}/sec)\n";

            debugInfo += $"<color=#4444FF>Move Speed:</color> {stats.MoveSpeed:F1}\n";
            //debugInfo += $"<color=#44AAFF>Projectiles:</color> {stats.Projectiles}\n";
            debugInfo += $"<color=#44FFAA>Pickup Radius:</color> {stats.PickupRadius:F1}\n";

            debugInfo += $"<color=#888888>DMG Bonus:</color> {stats.damageBonus * 100:F0}%\n";
            debugInfo += $"<color=#888888>Fire Bonus:</color> {stats.fireRateBonus * 100:F0}%\n";

            //float dps = stats.Damage * (1f / stats.FireRate) * stats.Projectiles;
            float dps = stats.GunDamage * (1f / stats.FireRate);
            debugInfo += $"<color=#FF8888>DPS:</color> {dps:F1}\n";
        }

        // Movement stats
        if (playerMovement != null)
        {
            var movementType = playerMovement.GetType();
            var speedField = movementType.GetField("moveSpeed");
            if (speedField != null)
                debugInfo += $"<color=#4444FF>Speed:</color> {speedField.GetValue(playerMovement):F1}\n";
        }

        // Zombie Info
        debugInfo += $"\n<size=40><b>ZOMBIE STATS</b></size>\n";
        int zombieCount = FindObjectsOfType<ZombieController>().Length;
        debugInfo += $"<color=#FF00FF>Active Zombies:</color> {zombieCount}\n";

        // Get average zombie stats
        var zombies = FindObjectsOfType<ZombieController>();
        if (zombies.Length > 0)
        {
            float avgSpeed = 0;
            int avgDamage = 0;
            foreach (var z in zombies)
            {
                avgSpeed += z.speed;
                // Assuming ZombieController has damage field
                var damageField = z.GetType().GetField("damage");
                if (damageField != null)
                    avgDamage += (int)damageField.GetValue(z);
            }
            debugInfo += $"<color=#FF00FF>Avg Speed:</color> {avgSpeed / zombies.Length:F2}\n";
            debugInfo += $"<color=#FF00FF>Avg Damage:</color> {avgDamage / zombies.Length}\n";
        }

        // Spawner Info
        if (spawner != null)
        {
            debugInfo += $"\n<size=40><b>WAVE DEBUG</b></size>\n";

            debugInfo += $"<color=#00FFFF>Wave:</color> {spawner.CurrentWave}\n";
            debugInfo += $"<color=#00FFFF>Wave Progress:</color> {(spawner.WaveProgress * 100f):F0}%\n";
            debugInfo += $"<color=#00FFFF>Pressure:</color> {spawner.CurrentPressure:F2}\n";

            debugInfo += $"<color=#00FFFF>Spawn Interval:</color> {spawner.CurrentSpawnInterval:F2}s ";
            debugInfo += $"({(1f / Mathf.Max(0.01f, spawner.CurrentSpawnInterval)):F1}/sec)\n";

            debugInfo += $"<color=#00FFFF>Waiting For Clear:</color> {spawner.WaitingForClear}\n";
        }

        // Performance
        debugInfo += $"\n<size=40><b>PERFORMANCE</b></size>\n";
        debugInfo += $"<color=#AAAAAA>FPS:</color> {1f / Time.deltaTime:F0}\n";
        debugInfo += $"<color=#AAAAAA>Objects:</color> {FindObjectsOfType<GameObject>().Length}\n";

        // Current Upgrades
        debugInfo += $"\n<size=40><b>UPGRADES ACTIVE</b></size>\n";
        // You'd need to track this in UpgradeManager

        debugText.text = debugInfo;
    }

    // Quick cheat commands for testing
    void OnGUI()
    {
        if (!debugPanel.activeSelf) return;

        GUILayout.BeginArea(new Rect(10, 200, 300, 400));

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 20;
        buttonStyle.fixedHeight = 60;

        if (GUILayout.Button("+100 XP", buttonStyle))
            if (playerXP != null) playerXP.AddXP(100);

        if (GUILayout.Button("Full Heal", buttonStyle))
            if (playerHealth != null) playerHealth.SetFullHealth();

        if (GUILayout.Button("Kill All Zombies", buttonStyle))
        {
            var zombies = FindObjectsOfType<ZombieController>();
            foreach (var z in zombies)
                z.GetComponent<Health>()?.TakeDamage(999);
        }

        if (GUILayout.Button("Level Up", buttonStyle))
            if (playerXP != null) playerXP.AddXP(playerXP.xpToNext);

        if (GUILayout.Button("Toggle God Mode", buttonStyle))
            if (playerHealth != null)
                playerHealth.invincible = !playerHealth.invincible;

        GUILayout.EndArea();
    }
}
