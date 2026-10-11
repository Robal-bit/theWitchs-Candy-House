using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        // Use this one for slide 9.
        Destroy(gameObject, 2f);

        // Comment out the line above before slide 13. Pooling replaces
        // Destroy() entirely, otherwise pooled bullets break after 2 seconds.

        // Use this one for slide 17. Keep Destroy() above commented out.
        // Invoke(nameof(ReturnToPool), 2f);
    }

    // Use this one for slide 17.
    // private void ReturnToPool()
    // {
    //     gameObject.SetActive(false);
    // }
}
