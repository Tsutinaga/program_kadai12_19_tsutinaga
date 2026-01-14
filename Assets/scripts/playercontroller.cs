using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.1f;

    private Vector3 targetPosition;
    private bool isMoving = false;

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
        // 右クリックで移動先を設定
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                targetPosition.y = transform.position.y; // Y軸は現在の高さを維持
                isMoving = true;
            }
        }

        // 目標位置に向かって移動
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > stoppingDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                isMoving = false;
            }
        }

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

        // �_���[�W���̐F�ω�
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