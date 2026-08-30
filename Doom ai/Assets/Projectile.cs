using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Vector3 direction;

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
