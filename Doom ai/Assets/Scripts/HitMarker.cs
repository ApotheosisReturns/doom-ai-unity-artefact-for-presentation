using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public Image marker;          // UI image used as hitmarker
    public float flashTime = 0.1f; // How long the flash lasts

    private float timer;          // Internal timer for fade-out

    void Update()
    {
        // Fade out the hitmarker over time
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            float alpha = timer / flashTime;

            // Update alpha value
            marker.color = new Color(1, 1, 1, alpha);
        }
    }

    public void Flash()
    {
        // Reset timer and make hitmarker fully visible
        timer = flashTime;
        marker.color = new Color(1, 1, 1, 1);
    }
}
