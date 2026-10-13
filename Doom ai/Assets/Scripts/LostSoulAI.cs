using UnityEngine;

public class LostSoulAI : MonoBehaviour
{
    private enum State
    {
        Chase,
        Charge,
        Recover,
        Dead
    }

    [Header("Movement")]
    public float moveSpeed = 4f;

    [Header("Charge Attack")]
    public float chargeRange = 8f;
    public float chargeSpeed = 15f;
    public float chargeDuration = 0.5f;
    public float recoveryTime = 1f;
    public float attackDamage = 15f;

    [Header("Health")]
    public int maxHealth = 30;

    private int currentHealth;

    private Transform player;

    private State currentState = State.Chase;

    private Vector3 chargeDirection;

    private float stateTimer;

    // Prevent multiple hits during a single charge
    private bool hasHitPlayer;

    private EnemyFlashRed flashEffect;
    private EnemyFlinch flinch;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObject =
            GameObject.FindWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;

        flashEffect = GetComponent<EnemyFlashRed>();
        flinch = GetComponent<EnemyFlinch>();
    }

    private void Update()
    {
        if (currentState == State.Dead)
            return;

        if (flinch != null && flinch.IsFlinching)
            return;

        switch (currentState)
        {
            case State.Chase:
                HandleChase();
                break;

            case State.Charge:
                HandleCharge();
                break;

            case State.Recover:
                HandleRecover();
                break;
        }
    }

    private void HandleChase()
    {
        if (player == null)
            return;

        transform.LookAt(player);

        Vector3 dir =
            (player.position - transform.position)
            .normalized;

        transform.position +=
            dir *
            moveSpeed *
            Time.deltaTime;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        if (distance <= chargeRange)
        {
            chargeDirection = dir;

            currentState = State.Charge;

            stateTimer = chargeDuration;

            // Reset hit flag for new charge
            hasHitPlayer = false;
        }
    }

    private void HandleCharge()
    {
        transform.position +=
            chargeDirection *
            chargeSpeed *
            Time.deltaTime;

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            currentState = State.Recover;
            stateTimer = recoveryTime;
        }
    }

    private void HandleRecover()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            currentState = State.Chase;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState != State.Charge)
            return;

        if (hasHitPlayer)
            return;

        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            hasHitPlayer = true;

            // Damage player
            playerHealth.TakeDamage(attackDamage);

            // Bounce backward after impact
            transform.position -= chargeDirection * 2f;

            // Enter recovery state
            currentState = State.Recover;
            stateTimer = recoveryTime;
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentState == State.Dead)
            return;

        currentHealth -= amount;

        flashEffect?.Flash();
        flinch?.Flinch();

        if (currentHealth <= 0)
        {
            currentState = State.Dead;

            Destroy(gameObject);
        }
    }
}