using System.Collections;
using UnityEngine;

public class EnemyFlinch : MonoBehaviour
{
    [Header("Flinch Settings")]

    // How long the enemy stays stunned
    public float flinchDuration = 0.25f;

    // Tracks whether the enemy is currently flinching
    private bool isFlinching = false;

    // Public read-only property
    // Other scripts can check flinch state but cannot modify it directly
    public bool IsFlinching
    {
        get { return isFlinching; }
    }

    // Called by EnemyAI when damage is taken
    public void Flinch()
    {
        // Prevent multiple flinch coroutines from stacking
        if (!isFlinching)
        {
            StartCoroutine(FlinchRoutine());
        }
    }

    private IEnumerator FlinchRoutine()
    {
        // Enter flinch state
        isFlinching = true;

        // Enemy remains stunned for this duration
        yield return new WaitForSeconds(flinchDuration);

        // Return to normal behaviour
        isFlinching = false;
    }
}