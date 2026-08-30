using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;     // Movement speed
    public float damage = 10f;    // Damage dealt to player
    public float lifetime = 5f;   // Auto-destroy timer

    private Vector3 direction;    // Direction projectile travels

    public void Launch(Vector3 dir)
    {
        // Normalize direction and store it
        direction = dir.normalized;

        // Destroy projectile after lifetime expires
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move projectile forward every frame
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit the player
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            // Apply damage
            player.TakeDamage(damage);
        }

        // Destroy projectile on any collision
        Destroy(gameObject);
    }
}
