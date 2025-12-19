using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            FireAtNearestEnemy();
            fireTimer = 0f;
        }
    }

    void FireAtNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0) return;

        // ç≈Ç‡ãﬂÇ¢ìGÇíTÇ∑
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            // íeÇê∂ê¨
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                Vector3 direction = (nearest.transform.position - transform.position).normalized;
                bulletScript.Initialize(direction, bulletSpeed);
            }
        }
    }
}