using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ballTransform;
    [SerializeField] private SpriteRenderer topWall;
    [SerializeField] private SpriteRenderer bottomWall;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float reactionDelay = 0.15f; // Higher = slower reaction
    [SerializeField] private float errorMargin = 0.5f;   // How far the ball must be to trigger movement

    private float paddleHalfHeight;
    private float currentVelocity; // Required for SmoothDamp

    void Start()
    {
        paddleHalfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        if (ballTransform == null) return;

        HandleAIMovement();
    }

    private void HandleAIMovement()
    {
        float targetY = transform.position.y;

        // Core Logic: Only move if the ball is outside our "comfort zone" (the errorMargin)
        // This prevents the paddle from being 100% perfectly centered.
        float distanceToBall = ballTransform.position.y - transform.position.y;

        if (Mathf.Abs(distanceToBall) > errorMargin)
        {
            // Calculate smooth movement toward the ball's Y position
            targetY = Mathf.SmoothDamp(
                transform.position.y,
                ballTransform.position.y,
                ref currentVelocity,
                reactionDelay,
                speed
            );
        }

        // Apply Boundaries (same as player paddle)
        float topLimit = topWall.bounds.min.y - paddleHalfHeight;
        float bottomLimit = bottomWall.bounds.max.y + paddleHalfHeight;

        float clampedY = Mathf.Clamp(targetY, bottomLimit, topLimit);

        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}