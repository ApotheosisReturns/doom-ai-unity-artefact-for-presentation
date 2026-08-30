using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 20f;        // Amount of damage dealt per shot
    public float range = 50f;         // Maximum raycast distance
    public float fireRate = 0.2f;     // Minimum time between shots

    [Header("References")]
    public Camera playerCamera;       // Camera used to aim the raycast

    private float nextFireTime = 0f;  // Internal timer to enforce fire rate

    // --- Input System Callback ---
    // This method is automatically called when the "Fire" action is triggered.
    // InputValue tells us whether the button is pressed or released.
    public void OnFire(InputValue value)
    {
        // Only shoot when the button is pressed (not released)
        if (value.isPressed)
            TryShoot();
    }

    // --- Fire Rate Check ---
    // Ensures the player cannot shoot faster than fireRate.
    private void TryShoot()
    {
        // If current time is still below next allowed fire time, do nothing.
        if (Time.time < nextFireTime)
            return;

        // Set next allowed fire time
        nextFireTime = Time.time + fireRate;

        // Perform the actual shooting logic
        Shoot();
    }

    // --- Hitscan Shooting Logic ---
    private void Shoot()
    {
        // Create a ray starting at the camera and going forward.
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // Try to get an EnemyAI component from the object hit.
            // IMPORTANT: GetComponentInParent allows detection even if collider is on a child.
            EnemyAI enemy = hit.transform.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage((int)damage);

                // Debug message for testing
                Debug.Log("Hit: " + hit.transform.name);
            }
        }
    }
}
