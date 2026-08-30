using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    public float attackRange = 25f;

    private float nextAttackTime = 0f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        Debug.Log("Enemy is attacking!");

        transform.LookAt(player);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Vector3 dir = (player.position - firePoint.position).normalized;

        proj.GetComponent<Projectile>().Launch(dir);
    }
}
