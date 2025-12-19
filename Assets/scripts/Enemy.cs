using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float maxHP = 50f;
    private float currentHP;
    public float moveSpeed = 2f;
    private Transform player;
    private Material enemyMaterial;
    private Color originalColor = Color.red;
    private float damageFlashTime = 0.2f;
    private float damageTimer = 0f;
    private Vector3 originalScale;
    private float shrinkDuration = 0.3f;
    private bool isDying = false;

    void Start()
    {
        currentHP = maxHP;
        originalScale = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            enemyMaterial = renderer.material;
            enemyMaterial.color = originalColor;
        }
    }

    void Update()
    {
        if (isDying) return;
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0 && enemyMaterial != null)
            {
                enemyMaterial.color = originalColor;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (enemyMaterial != null && !isDying)
        {
            enemyMaterial.color = Color.yellow;
            damageTimer = damageFlashTime;
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDying = true;
        StartCoroutine(ShrinkAndDestroy());
    }

    IEnumerator ShrinkAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / shrinkDuration;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}