using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isGameOver = false;
    private bool isGameClear = false;

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

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameOver();
    }

    public void GameClear()
    {
        if (isGameClear) return;

        isGameClear = true;
        Debug.Log("GAME CLEAR!");
        Time.timeScale = 0f;
        UIManager.Instance?.ShowGameClear();
    }
}