using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishData fishData;          // 물고기 데이터
    [SerializeField] private float moveSpeed = 1.5f;         // 움직이는 속도
    [SerializeField] private float moveDistance = 2.5f;      // 좌우 이동 범위

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

    public void OnHitByHarpoon()
    { 
        if(fishData != null)
        {
            FishInventory.Instance.AddFish(fishData);
            Debug.Log($"{fishData.fishName} 잡음. 현재 수량: {FishInventory.Instance.GetFishCount(fishData)}");
        }
        StartCoroutine(Vanish());
    }


    private IEnumerator Vanish()
    {
        // 잡히는 연출
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
