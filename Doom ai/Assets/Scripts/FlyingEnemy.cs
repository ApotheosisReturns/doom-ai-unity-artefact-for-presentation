using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    // --- Movement ---
    public float moveSpeed = 3f;       // Chase speed
    public float hoverHeight = 1f;     // Hover bob amplitude
    public float hoverSpeed = 2f;      // Hover bob frequency

    // --- Combat ---
    public float attackRange = 15f;    // Max shooting distance
    public float attackCooldown = 2f;  // Time between shots
    public GameObject projectilePrefab;
    public Transform firePoint;

    // --- Health ---
    public int maxHealth = 50;
    private int currentHealth;

    private Transform player;          // Player reference
    private float nextAttackTime = 0f; // Attack cooldown timer
    private float baseY;               // Base hover height

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentHealth = maxHealth;

        // Store starting Y position for hovering
        baseY = transform.position.y;
    }

    private void Update()
    {
        if (player == null) return;

        Hover();
        ChasePlayer();
        TryAttack();
    }

    private void Hover()
    {
        // Sin wave vertical bobbing
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        Vector3 pos = transform.position;
        pos.y = baseY + hoverOffset;
        transform.position = pos;
    }

    private void ChasePlayer()
    {
        // Move directly toward player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Always face the player
        transform.LookAt(player);
    }

    private void TryAttack()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Shoot if close enough and cooldown expired
        if (dist <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Launch toward player
        Vector3 dir = (player.position - firePoint.position).normalized;
        proj.GetComponent<Projectile>().Launch(dir);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
