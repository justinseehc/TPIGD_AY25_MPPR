using UnityEngine;

public class Paddle_NP : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;

    [SerializeField] SpriteRenderer topWall;
    [SerializeField] SpriteRenderer bottomWall;

    SpriteRenderer paddle;

    float halfHeight;
    float topLimit;
    float bottomLimit;

    void Awake()
    {
        paddle = GetComponent<SpriteRenderer>();
        halfHeight = paddle.bounds.extents.y;

        topLimit = topWall.bounds.min.y - halfHeight;
        bottomLimit = bottomWall.bounds.max.y + halfHeight;
    }

    void Update()
    {
        float input = 0f;
        if (Input.GetKey(up)) input = 1f;
        if (Input.GetKey(down)) input = -1f;

        if (input == 0f) return;

        Vector3 pos = transform.position;
        pos.y += input * speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, bottomLimit, topLimit);
        transform.position = pos;
    }
}
