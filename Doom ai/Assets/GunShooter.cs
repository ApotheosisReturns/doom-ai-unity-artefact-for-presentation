using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooter : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 20f;
    public float range = 50f;
    public float fireRate = 0.2f;

    [Header("References")]
    public Camera playerCamera;

    private float nextFireTime = 0f;

    private void Update()
    {
        // Poll the Input System directly
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

    private void Shoot()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }

            Debug.Log("Hit: " + hit.transform.name);
        }
    }
}
