using System.Collections;
using UnityEngine;

public enum FishType
{
    Blue, Orange, Red, Blue1, Green, Shark, Grey, JellyFish, Octopus, SawShark, SeaAngler, Shrimp, SwordFish, Squid
}
public class Fish : MonoBehaviour
{
    public FishType fishType;

    [SerializeField] private string fishName;      // JSON에 등록된 이름
    [SerializeField] protected float moveSpeed = 1.5f;     // 움직이는 속도
    [SerializeField] protected float moveDistance = 2.5f;  // 좌우 이동 범위

    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected bool isMovingRight = true;
    protected Vector3 startPos;

    public bool isCaught = false;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    private void OnEnable()
    {
        isCaught = false;
    }

    protected void FixedUpdate()
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
        // 1?. 현재 물고기 스프라이트 가져오기
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite fishSprite = sr != null ? sr.sprite : null;

        //// 2?.  UI 쪽에 먼저 전달
        //if (fishSprite != null)
        //{
        //    FindObjectOfType<FishInventoryManager>().AddFish(fishSprite);
        //}
        //else
        //{
        //    Debug.LogWarning($"{name}의 SpriteRenderer를 찾지 못해서 UI 표시 불가");
        //}

        //if (!string.IsNullOrEmpty(fishName))
        //{
        //    //FishInventory.Instance.AddFish(fishName);
        //    //Debug.Log($"{fishName} 잡음! 현재 수량: {FishInventory.Instance.GetFishCount(fishName)}");
        //}
        //else
        //{
        //    Debug.LogWarning("fishName이 비어 있어서 인벤토리에 등록 안 됨");
        //}

        StartCoroutine(Vanish());
    }

    protected IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
