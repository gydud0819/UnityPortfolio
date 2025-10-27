using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;         // �����̴� �ӵ�
    [SerializeField] private float moveDistance = 2.5f;      // �¿� �̵� ����

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private bool isMovingRight = true;
    private Vector3 startPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float moveDir = isMovingRight ? 1f : -1f;
        rigid.linearVelocity = new Vector2(moveDir * moveSpeed, rigid.linearVelocity.y);

        float distanceMoved = transform.position.x - startPos.x;

        if (Mathf.Abs(distanceMoved) >= moveDistance)
        {
            isMovingRight = !isMovingRight;
            startPos = transform.position;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
