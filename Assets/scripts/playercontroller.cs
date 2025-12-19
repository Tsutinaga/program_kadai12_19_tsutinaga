using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("HP")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Damage Effect")]
    public Material playerMaterial;
    private Color originalColor;
    private float damageFlashTime = 0.2f;
    private float damageTimer = 0f;

    void Start()
    {
        currentHP = maxHP;
        if (playerMaterial != null)
        {
            originalColor = playerMaterial.color;
        }
    }

    void Update()
    {
        // 移動
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.position += move;

        // ダメージエフェクトのタイマー
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0 && playerMaterial != null)
            {
                playerMaterial.color = originalColor;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        UIManager.Instance?.UpdateHP(currentHP, maxHP);

        // ダメージ時の色変化
        if (playerMaterial != null)
        {
            playerMaterial.color = Color.red;
            damageTimer = damageFlashTime;
        }

        Debug.Log($"Player HP: {currentHP}");

        if (currentHP <= 0)
        {
            GameManager.Instance?.GameOver();
        }
    }

    public float GetCurrentHP() => currentHP;
}