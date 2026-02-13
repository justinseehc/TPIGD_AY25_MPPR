using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float ballRadius = 0.5f;

    private Vector3 _direction;

    void Start()
    {
        // Initialize with a diagonal movement
        _direction = new Vector3(1, 1, 0).normalized;
    }

    void Update()
    {
        MoveAndBounce();
    }

    void MoveAndBounce()
    {
        float moveDistance = speed * Time.deltaTime;

        // Predict the next position to check for a wall hit
        // We use a Raycast to simulate "collision" without a Rigidbody
        if (Physics.Raycast(transform.position, _direction, out RaycastHit hit, moveDistance + ballRadius))
        {
            // Calculate the reflection vector based on the wall's normal
            _direction = Vector3.Reflect(_direction, hit.normal);

            // Optional: Move the ball to the hit point immediately to prevent clipping
            transform.position = hit.point + (hit.normal * ballRadius);
        }

        // Apply movement
        transform.Translate(_direction * moveDistance, Space.World);
    }
}