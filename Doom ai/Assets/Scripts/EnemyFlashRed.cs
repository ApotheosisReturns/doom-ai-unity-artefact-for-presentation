using System.Collections;
using UnityEngine;

public class EnemyFlashRed : MonoBehaviour
{
    [Header("Flash Settings")]

    // Colour the enemy will flash when damaged
    public Color flashColor = Color.red;

    // How long the flash effect lasts
    public float flashDuration = 0.1f;

    // Reference to the enemy's renderer component
    private Renderer enemyRenderer;

    // Stores the enemy's original colour
    private Color originalColor;

    private void Start()
    {
        // Search this object and all child objects for a Renderer
        // This allows the script to work with more complex enemy hierarchies
        enemyRenderer = GetComponentInChildren<Renderer>();

        // Store the original colour so we can restore it later
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    // Called by EnemyAI when damage is taken
    public void Flash()
    {
        // Start the flash coroutine
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Change material colour to red
        enemyRenderer.material.color = flashColor;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Restore original colour
        enemyRenderer.material.color = originalColor;
    }
}