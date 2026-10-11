using UnityEngine;

// WEEK 3: A trigger collider identifies pooled candy by tag.
public class WitchChildTarget : MonoBehaviour
{
    [SerializeField] private WitchGameManager gameManager;
    [SerializeField] private bool isHansel = true;

    private SpriteRenderer picture;
    private float flashTime;

    private void Awake()
    {
        picture = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (flashTime <= 0f) return;
        flashTime -= Time.deltaTime;
        if (flashTime <= 0f && picture != null) picture.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("CandyProjectile")) return;
        WitchCandyProjectile candy = other.GetComponent<WitchCandyProjectile>();
        if (candy == null) return;
        int points = candy.FullnessPoints;
        int suspicion = candy.SuspicionPoints;
        if (candy.Consume() && gameManager != null)
        {
            flashTime = 0.15f;
            if (picture != null) picture.color = new Color(0.5f, 1f, 0.7f);
            gameManager.RegisterCandyHit(isHansel, points, suspicion);
        }
    }
}
