using UnityEngine;

// WEEK 3: One candy GameObject is recycled instead of destroyed.
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class WitchCandyProjectile : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D body;
    private SpriteRenderer picture;
    private Vector2 direction = Vector2.up;
    private float secondsLeft;
    private bool consumed;

    public int FullnessPoints { get; private set; }
    public int SuspicionPoints { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        picture = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        secondsLeft = lifetime;
        consumed = false;
    }

    public void Launch(Vector2 origin, Vector2 facing, WitchCandyCard card)
    {
        transform.position = origin;
        body.position = origin; // Also reset Rigidbody2D physics position when reusing an instance.
        direction = facing.normalized;
        if (direction == Vector2.zero) direction = Vector2.up;
        FullnessPoints = card.FullnessPoints;
        SuspicionPoints = card.SuspicionPoints;
        if (picture != null) picture.color = card.ProjectileColor;
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + direction * movementSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        secondsLeft -= Time.deltaTime;
        if (secondsLeft <= 0f) gameObject.SetActive(false);
    }

    // The target calls this once when a candy makes contact.
    public bool Consume()
    {
        if (consumed || !gameObject.activeSelf) return false;
        consumed = true;
        gameObject.SetActive(false);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall")) gameObject.SetActive(false);
    }
}
