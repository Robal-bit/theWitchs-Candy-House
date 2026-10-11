using UnityEngine;
using UnityEngine.InputSystem;

// WEEK 4: PlayerInput sends OnMove / OnFire from an Input Actions asset.
// WEEK 3: Rigidbody2D moves the witch and collides with Tilemap walls.
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class WitchPlayerController : MonoBehaviour
{
    [SerializeField] private WitchGameManager gameManager;
    [SerializeField] private float speed = 4f;
    [SerializeField] private Transform aimMarker;
    [SerializeField] private float throwCooldown = 0.18f;
    private float nextThrowTime;
    private Rigidbody2D body;
    private Vector2 movement;
    private Vector2 facing = Vector2.up;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        if (movement.sqrMagnitude > 0.001f)
        {
            facing = movement.normalized;
            if (aimMarker != null) aimMarker.localPosition = facing * (0.8f / transform.localScale.x);
        }
    }

    public void OnFire(InputValue value)
    {
        if (!value.isPressed || gameManager == null || !gameManager.CanPlay) return;
        if (Time.time < nextThrowTime) return;
        nextThrowTime = Time.time + throwCooldown;
        Vector2 origin = body.position + facing * 0.8f;
        gameManager.ThrowSelectedCandy(origin, facing);
    }

    public void OnSelectOne(InputValue value)
    {
        if (value.isPressed && gameManager != null) gameManager.SelectFirstCard();
    }

    public void OnSelectTwo(InputValue value)
    {
        if (value.isPressed && gameManager != null) gameManager.SelectSecondCard();
    }

    public void OnSelectThree(InputValue value)
    {
        if (value.isPressed && gameManager != null) gameManager.SelectThirdCard();
    }

    private void FixedUpdate()
    {
        if (gameManager == null || !gameManager.CanPlay) return;
        Vector2 nextPosition = body.position + Vector2.ClampMagnitude(movement, 1f) * speed * Time.fixedDeltaTime;
        body.MovePosition(nextPosition);
    }
}
