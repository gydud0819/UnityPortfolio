using UnityEngine;

public class JellyFish : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;         // 천천히 움직이게
    [SerializeField] private float moveDistance = 1.5f;      // 위아래로 왔다갔다할 거리

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private bool isMovingUp = true;
    private Vector3 startPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float moveDir = isMovingUp ? 1f : -1f;
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, moveDir * moveSpeed); // ← X대신 Y축 이동

        float distanceMoved = transform.position.y - startPos.y;

        if (Mathf.Abs(distanceMoved) >= moveDistance)
        {
            isMovingUp = !isMovingUp;
            startPos = transform.position;
        }
    }
}
