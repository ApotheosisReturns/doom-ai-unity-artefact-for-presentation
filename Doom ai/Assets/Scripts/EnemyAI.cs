using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // --- Enemy finite state machine ---
    // These states represent the enemy's behaviour at any moment.
    private enum EnemyState
    {
        Idle,       // Enemy is not aware of the player
        Chase,      // Enemy sees player and moves toward them
        Attack,     // Enemy is close enough to attack
        Pain,       // Enemy has been hit and briefly stunned
        Dead        // Enemy is dead and no longer acts
    }

    [Header("References")]
    public Transform player;   // Reference to the player transform

    [Header("Movement")]
    public float moveSpeed = 3f;     // Speed when chasing the player
    public float sightRange = 15f;   // Max distance enemy can detect player
    public float attackRange = 2f;   // Distance required to attack

    [Header("Combat")]
    public float attackCooldown = 1f; // Time between melee attacks
    private float attackTimer;        // Internal cooldown timer

    [Header("Health")]
    public int maxHealth = 100;       // Maximum HP
    private int currentHealth;        // Current HP

    // Current behaviour state
    private EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        // Initialize health when enemy spawns
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Main behaviour loop — runs every frame
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
                // No behaviour when dead
                break;
        }
    }

    // --- IDLE STATE ---
    // Enemy stands still until it sees the player.
    private void HandleIdle()
    {
        if (CanSeePlayer())
            currentState = EnemyState.Chase; // Transition to chase
    }

    // --- CHASE STATE ---
    // Enemy moves directly toward the player.
    private void HandleChase()
    {
        // If player is no longer visible, return to idle
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Idle;
            return;
        }

        // Check distance to player
        float dist = Vector3.Distance(transform.position, player.position);

        // If close enough, switch to attack state
        if (dist <= attackRange)
        {
            currentState = EnemyState.Attack;
            return;
        }

        // Move toward player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Face the player
        transform.LookAt(player);
    }

    // --- ATTACK STATE ---
    // Enemy performs melee attacks when close enough.
    private void HandleAttack()
    {
        // If player is out of sight, stop attacking
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        // If player moved away, return to chase
        if (dist > attackRange)
        {
            currentState = EnemyState.Chase;
            return;
        }

        // Count down attack cooldown
        attackTimer -= Time.deltaTime;

        // Perform attack when cooldown expires
        if (attackTimer <= 0f)
        {
            Debug.Log("Enemy attacks player");

            // TODO: Apply damage to player here
            // player.GetComponent<PlayerHealth>().TakeDamage(10);

            attackTimer = attackCooldown; // Reset cooldown
        }
    }

    // --- PAIN STATE ---
    // Enemy briefly stunned after taking damage.
    private void HandlePain()
    {
        // If enemy still sees player, resume chase
        if (CanSeePlayer())
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Idle;
    }

    // --- LINE OF SIGHT CHECK ---
    // Uses raycast to determine if enemy can see the player.
    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dir = (player.position - transform.position);

        // Too far away to see
        if (dir.magnitude > sightRange) return false;

        // Raycast from enemy's eye level toward player
        Ray ray = new Ray(transform.position + Vector3.up, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            // If ray hits the player, enemy has line of sight
            return hit.transform == player;
        }

        return false;
    }

    // --- DAMAGE HANDLING ---
    public void TakeDamage(int amount)
    {
        if (currentState == EnemyState.Dead) return;

        currentHealth -= amount;

        // If health reaches zero, die
        if (currentHealth <= 0)
        {
            currentState = EnemyState.Dead;

            // Disable enemy object (or play death animation)
            gameObject.SetActive(false);
        }
        else
        {
            // Enter pain state when hit
            currentState = EnemyState.Pain;
        }
    }
}
