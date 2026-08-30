using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;   // Maximum health
    public float currentHealth;      // Current health

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Reduce health
        currentHealth -= amount;

        Debug.Log("Player took damage: " + amount);

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("PLAYER DEAD");

        // TODO: Disable movement, play animation, reload scene, etc.
    }
}
