using System.Collections;
using UnityEngine;

public class EnemyFlinch : MonoBehaviour
{
    [Header("Flinch Settings")]
    public float flinchDuration = 0.25f;

    private bool isFlinching = false;

    public bool IsFlinching
    {
        get { return isFlinching; }
    }

    public void Flinch()
    {
        if (!isFlinching)
        {
            StartCoroutine(FlinchRoutine());
        }
    }

    private IEnumerator FlinchRoutine()
    {
        isFlinching = true;

        yield return new WaitForSeconds(flinchDuration);

        isFlinching = false;
    }
}