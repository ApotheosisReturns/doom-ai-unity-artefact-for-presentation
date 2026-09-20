using UnityEngine;

public class LostSoulTracker : MonoBehaviour
{
    // Reference to the Pain Elemental that created this soul
    public PainElementalSpawner owner;

    private void OnDestroy()
    {
        // Notify the owner when this Lost Soul dies

        if (owner != null)
        {
            owner.SoulDied();
        }
    }
}