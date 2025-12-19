using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime = 5f;

    public void Initialize(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(25f);
            }
            Destroy(gameObject);
        }
    }
}