using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float hoverHeight = 1f;
    public float hoverSpeed = 2f;

    [Header("Combat")]
    public float attackRange = 15f;
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Health")]
    public int maxHealth = 50;
    private int currentHealth;

    private Transform player;
    private float nextAttackTime = 0f;
    private float baseY;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentHealth = maxHealth;
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
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        Vector3 pos = transform.position;
        pos.y = baseY + hoverOffset;
        transform.position = pos;
    }

    private void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        transform.LookAt(player);
    }

    private void TryAttack()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Vector3 dir = (player.position - firePoint.position).normalized;

        proj.GetComponent<Projectile>().Launch(dir);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
