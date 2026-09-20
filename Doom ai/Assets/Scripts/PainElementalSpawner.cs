using UnityEngine;

public class PainElementalSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]

    // Prefab of the Lost Soul enemy that will be spawned
    public GameObject lostSoulPrefab;

    // Maximum distance at which the player must be
    // before spawning is allowed
    public float spawnRange = 20f;

    // Time between spawning Lost Souls
    public float spawnCooldown = 5f;

    // Maximum number of living Lost Souls
    // this Pain Elemental can have at one time
    public int maxSpawnedSouls = 5;

    // Timer used to control spawning frequency
    private float nextSpawnTime;

    // Reference to the player
    private Transform player;

    // Tracks how many Lost Souls this Pain Elemental owns
    private int currentSouls;

    private void Start()
    {
        // Find the player using the Player tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void Update()
    {
        // Stop if player cannot be found
        if (player == null)
            return;

        // Calculate distance to player
        float distance =
            Vector3.Distance(transform.position, player.position);

        // If player is too far away, don't spawn
        if (distance > spawnRange)
            return;

        // If maximum number of Lost Souls are alive,
        // stop spawning
        if (currentSouls >= maxSpawnedSouls)
            return;

        // Wait until cooldown has expired
        if (Time.time >= nextSpawnTime)
        {
            SpawnLostSoul();

            // Set the next allowed spawn time
            nextSpawnTime =
                Time.time + spawnCooldown;
        }
    }

    private void SpawnLostSoul()
    {
        // Spawn slightly in front of the Pain Elemental
        Vector3 spawnPosition =
            transform.position +
            transform.forward * 2f;

        // Create the Lost Soul
        GameObject soul =
            Instantiate(
                lostSoulPrefab,
                spawnPosition,
                Quaternion.identity);

        // Increase active soul count
        currentSouls++;

        // Add a tracking component so the Pain Elemental
        // knows when the Lost Soul dies
        LostSoulTracker tracker =
            soul.AddComponent<LostSoulTracker>();

        // Give the tracker a reference back to this spawner
        tracker.owner = this;
    }

    public void SoulDied()
    {
        // Called when a Lost Soul is destroyed

        currentSouls--;

        // Prevent negative values from accidental double calls
        currentSouls = Mathf.Max(0, currentSouls);
    }
}