using UnityEngine;

public class BaronBoss : MonoBehaviour
{
    private enum BossState
    {
        Idle,
        Chase,
        Attack,
        Enraged,
        Dead
    }

    [Header("Detection")]
    public float detectionRange = 20f;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Combat")]
    public float attackRange = 15f;
    public float attackCooldown = 2f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Health")]
    public int maxHealth = 500;

    private int currentHealth;

    private BossState currentState = BossState.Idle;

    private Transform player;

    private float attackTimer;

    private bool enraged;

    private EnemyFlashRed flashEffect;
    private EnemyFlinch flinch;

    private void Start()
    {
        currentHealth = maxHealth;

        flashEffect = GetComponent<EnemyFlashRed>();
        flinch = GetComponent<EnemyFlinch>();

        GameObject playerObject =
            GameObject.FindWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void Update()
    {
        if (currentState == BossState.Dead)
            return;

        if (player == null)
            return;

        if (flinch != null && flinch.IsFlinching)
            return;

        CheckEnrage();

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdle();
                break;

            case BossState.Chase:
                HandleChase();
                break;

            case BossState.Attack:
                HandleAttack();
                break;

            case BossState.Enraged:
                HandleAttack();
                break;
        }
    }

    private void HandleIdle()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        if (distance <= detectionRange)
        {
            currentState = BossState.Chase;
        }
    }

    private void HandleChase()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        transform.LookAt(player);

        if (distance > attackRange)
        {
            Vector3 dir =
                (player.position - transform.position)
                .normalized;

            transform.position +=
                dir *
                moveSpeed *
                Time.deltaTime;
        }
        else
        {
            currentState = enraged
                ? BossState.Enraged
                : BossState.Attack;
        }
    }

    private void HandleAttack()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        transform.LookAt(player);

        if (distance > attackRange)
        {
            currentState = BossState.Chase;
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            FireProjectile();
            attackTimer = attackCooldown;
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null ||
            firePoint == null)
            return;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                firePoint.rotation);

        Vector3 dir =
            (player.position - firePoint.position)
            .normalized;

        projectile
            .GetComponent<Projectile>()
            .Launch(dir);
    }

    private void CheckEnrage()
    {
        if (enraged)
            return;

        if (currentHealth <= maxHealth / 2)
        {
            enraged = true;

            moveSpeed = 4f;
            attackCooldown = 1f;

            currentState = BossState.Enraged;

            Debug.Log("BARON ENRAGED");
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentState == BossState.Dead)
            return;

        currentHealth -= amount;

        flashEffect?.Flash();
        flinch?.Flinch();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentState = BossState.Dead;

        Debug.Log("BARON DEFEATED");

        Destroy(gameObject);
    }
}