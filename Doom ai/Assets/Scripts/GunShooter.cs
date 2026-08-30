using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooter : MonoBehaviour
{
    // --- Weapon Settings ---
    public float damage = 20f;       // Damage dealt per shot
    public float range = 50f;        // Max raycast distance
    public float fireRate = 0.2f;    // Time between shots

    // --- Visual & Audio Effects ---
    public ParticleSystem muzzleFlash;   // Muzzle flash particle system
    public AudioSource gunAudio;         // Gunshot sound

    // --- References ---
    public Camera playerCamera;          // Camera used for raycasting
    public HitMarker hitMarker;          // UI hitmarker flash
    public GameObject enemyHitEffect;    // Particle effect spawned on enemy hit

    private float nextFireTime = 0f;     // Tracks fire cooldown

    private void Update()
    {
        // Poll the Input System directly for left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
            TryShoot();
    }

    private void TryShoot()
    {
        // Prevent firing faster than fireRate
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;
        Shoot();
    }

    private void Shoot()
    {
        // Play muzzle flash + sound
        muzzleFlash?.Play();
        gunAudio?.Play();

        // Create a ray from the camera forward
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Raycast to detect enemies
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // IMPORTANT: Get enemy even if collider is on a child object
            EnemyAI enemy = hit.transform.GetComponentInParent<EnemyAI>();

            if (enemy != null)
            {
                // Apply damage
                enemy.TakeDamage((int)damage);

                // Spawn hit effect at impact point
                if (enemyHitEffect != null)
                    Instantiate(enemyHitEffect, hit.point, Quaternion.identity);

                // Flash hitmarker
                hitMarker?.Flash();
            }
        }
    }
}
