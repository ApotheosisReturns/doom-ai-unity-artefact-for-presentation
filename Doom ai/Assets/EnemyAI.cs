using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Pain,
        Dead
    }

    [Header("References")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float sightRange = 15f;
    public float attackRange = 2f;

    [Header("Combat")]
    public float attackCooldown = 1f;
    private float attackTimer;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    private EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
            case EnemyState.Pain:
                HandlePain();
                break;
            case EnemyState.Dead:
                // No behaviour
                break;
        }
    }

    private void HandleIdle()
    {
        if (CanSeePlayer())
            currentState = EnemyState.Chase;
    }

    private void HandleChase()
    {
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange)
        {
            currentState = EnemyState.Attack;
            return;
        }

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player);
    }

    private void HandleAttack()
    {
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            // DOOM-style simple attack (hitscan or melee)
            Debug.Log("Enemy attacks player");
            attackTimer = attackCooldown;
        }
    }

    private void HandlePain()
    {
        // Brief stun, then return to Chase or Idle
        // For now, just go back to Chase if player visible
        if (CanSeePlayer())
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Idle;
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dir = (player.position - transform.position);
        if (dir.magnitude > sightRange) return false;

        Ray ray = new Ray(transform.position + Vector3.up, dir.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            return hit.transform == player;
        }

        return false;
    }

    public void TakeDamage(int amount)
    {
        if (currentState == EnemyState.Dead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentState = EnemyState.Dead;
            // Death behaviour (disable collider, play animation, etc.)
            gameObject.SetActive(false);
        }
        else
        {
            currentState = EnemyState.Pain;
        }
    }
}
