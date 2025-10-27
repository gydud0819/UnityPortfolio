using UnityEngine;

public class JellyFish : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;         // õõ�� �����̰�
    [SerializeField] private float moveDistance = 1.5f;      // ���Ʒ��� �Դٰ����� �Ÿ�

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
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, moveDir * moveSpeed); // �� X��� Y�� �̵�

        float distanceMoved = transform.position.y - startPos.y;

        if (Mathf.Abs(distanceMoved) >= moveDistance)
        {
            isMovingUp = !isMovingUp;
            startPos = transform.position;
        }
    }
}
