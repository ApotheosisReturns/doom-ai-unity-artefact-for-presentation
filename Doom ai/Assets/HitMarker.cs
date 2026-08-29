using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public Image marker;
    public float flashTime = 0.1f;

    private float timer;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / flashTime;
            marker.color = new Color(1, 1, 1, alpha);
        }
    }

    public void Flash()
    {
        timer = flashTime;
        marker.color = new Color(1, 1, 1, 1);
    }
}
