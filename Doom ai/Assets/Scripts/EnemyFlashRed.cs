using System.Collections;
using UnityEngine;

public class EnemyFlashRed : MonoBehaviour
{
    [Header("Flash Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Renderer enemyRenderer;
    private Color originalColor;

    private void Start()
    {
        enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        enemyRenderer.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        enemyRenderer.material.color = originalColor;
    }
}