using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;  // Projectile to spawn
    public Transform firePoint;          // Where projectiles spawn
    public float attackCooldown = 2f;    // Time between shots
    public float attackRange = 20f;      // Max distance to shoot

    private float nextAttackTime = 0f;   // Tracks cooldown
    private Transform player;            // Player reference

    private void Start()
    {
        // Find player by tag
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Only attack if player is close enough and cooldown expired
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        // Face the player before shooting
        transform.LookAt(player);

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Launch projectile toward player
        Vector3 dir = (player.position - firePoint.position).normalized;
        proj.GetComponent<Projectile>().Launch(dir);
    }
}
