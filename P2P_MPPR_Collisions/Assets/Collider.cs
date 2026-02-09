using UnityEngine;

public class CustomBallMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speedX = 10f;
    public float speedY = 10f;
    public Vector2 areaSize = new Vector2(20f, 10f); // Bounds of your "room"
    public LayerMask wallLayer; // Optional: if you want to use objects as walls

    private Vector2 _direction;
    private bool _isMoving = false;
    private float _skinWidth = 0.1f; // Prevents "wall hugging" by adding a small buffer

    void Update()
    {
        // Start the test
        if (Input.GetKeyDown(KeyCode.Space) && !_isMoving)
        {
            StartBall();
        }

        if (_isMoving)
        {
            MoveBall();
        }
    }

    void StartBall()
    {
        transform.position = new Vector3(0f, 0f, 1f);
        // Launch at a random diagonal angle (never purely vertical/horizontal)
        float angle = Random.Range(30f, 60f);
        _direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
        _isMoving = true;
    }

    void MoveBall()
    {
        float moveDistance = (speedX + speedY) * Time.deltaTime;
        Vector2 nextPosition = (Vector2)transform.position + (_direction * moveDistance);

        // 1. Check Bounds (Manual Math Collision)
        CheckBoundaryCollision(ref nextPosition);

        // 2. Finalize Position
        //transform.position = nextPosition;
        transform.position = new Vector3(nextPosition.x, nextPosition.y, 1f);
    }

    void CheckBoundaryCollision(ref Vector2 nextPos)
    {
        float halfWidth = areaSize.x / 2f;
        float halfHeight = areaSize.y / 2f;

        // X-axis collision (Left/Right walls)
        if (Mathf.Abs(nextPos.x) > halfWidth - _skinWidth)
        {
            _direction.x *= -1; // Reflect
            // Snap back inside to prevent wall hugging/sticking
            nextPos.x = Mathf.Sign(nextPos.x) * (halfWidth - _skinWidth);

            speedX *= 1.1f;
            Debug.Log("Speed Increased!");
        }

        // Y-axis collision (Top/Bottom walls)
        if (Mathf.Abs(nextPos.y) > halfHeight - _skinWidth)
        {
            _direction.y *= -1; // Reflect
            // Snap back inside
            nextPos.y = Mathf.Sign(nextPos.y) * (halfHeight - _skinWidth);

            speedY *= 1.1f;
            Debug.Log("Speed Increased!");
        }
    }

    // Visualizing the area in the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, areaSize);
    }
}