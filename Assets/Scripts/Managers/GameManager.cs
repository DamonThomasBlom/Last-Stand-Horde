using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Health PlayerHealth;
    public GameObject LosePanel;

    private void Start()
    {
        Time.timeScale = 1;

        PlayerHealth.OnDie.AddListener(() =>
        {
            Time.timeScale = 0;
            LosePanel.SetActive(true);
        });
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
