using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 20f;
    public float range = 50f;
    public float fireRate = 0.2f;

    [Header("References")]
    public Camera playerCamera;

    private float nextFireTime = 0f;

    // Input System callback
    public void OnFire(InputValue value)
    {
        if (value.isPressed)
            TryShoot();
        Debug.Log("Fire input received!");

    }
    private void Start()
    {
        Debug.Log("PlayerWeapon started");
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
            // Check if the object has an EnemyAI script
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }

            Debug.Log("Hit: " + hit.transform.name);
        }
    }
}
