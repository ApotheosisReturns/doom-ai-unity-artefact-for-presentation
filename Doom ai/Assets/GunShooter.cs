using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooter : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 20f;
    public float range = 50f;
    public float fireRate = 0.2f;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioSource gunAudio;

    [Header("References")]
    public Camera playerCamera;

    private float nextFireTime = 0f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;
        Shoot();
    }
    public GameObject enemyHitEffect;
    public HitMarker hitMarker;


    private void Shoot()
    {
        // Play effects
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (gunAudio != null)
            gunAudio.Play();

        // Hitscan raycast
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
                if (enemyHitEffect != null)
                    Instantiate(enemyHitEffect, hit.point, Quaternion.identity);

                hitMarker?.Flash();
            }

            Debug.Log("Hit: " + hit.transform.name);
        }
    }
}
