using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 15f;

    [Header("Wave Settings")]
    public int enemiesPerWave = 10;
    public int totalWaves = 5;

    private int currentWave = 1;
    private int enemiesSpawnedThisWave = 0;
    private float spawnTimer = 0f;

    void Update()
    {
        if (currentWave > totalWaves) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && enemiesSpawnedThisWave < enemiesPerWave)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        // Wave終了チェック
        if (enemiesSpawnedThisWave >= enemiesPerWave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            NextWave();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0.5f;

        Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemiesSpawnedThisWave++;
    }

    void NextWave()
    {
        currentWave++;
        enemiesSpawnedThisWave = 0;

        UIManager.Instance?.UpdateWave(currentWave);

        if (currentWave > totalWaves)
        {
            GameManager.Instance?.GameClear();
        }
    }

    public int GetCurrentWave() => currentWave;
}