using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI waveText;
    public GameObject gameClearPanel;
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (gameClearPanel) gameClearPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void UpdateHP(float current, float max)
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {Mathf.CeilToInt(current)} / {max}";
        }
    }

    public void UpdateWave(int wave)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {wave}";
        }
    }

    public void ShowGameClear()
    {
        if (gameClearPanel) gameClearPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }
}