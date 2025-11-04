using UnityEngine;

public class HarpoonTip : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 6f;

    private Vector3 direction;
    private HarpoonPool pool;

    public void Fire(Vector3 dir, HarpoonPool poolRef)
    {
        startPos = transform.position;
        direction = dir.normalized;
        pool = poolRef;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            pool.ReturnHarpoon(gameObject);
        }
    }

    public void SetPool(HarpoonPool poolRef)
    {
        pool = poolRef;
        startPos = transform.position; // 시작 위치 저장
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy || pool == null) return;

        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish hit.");
            Debug.Log("충돌한 오브젝트 이름: " + collision.gameObject.name);

            Fish fish = collision.GetComponent<Fish>();
            if (fish != null)
            {
                // 물고기 오브젝트의 SpriteRenderer 가져오기
                SpriteRenderer sr = collision.GetComponent<SpriteRenderer>();

                if (sr != null && sr.sprite != null)
                {
                    Sprite fishSprite = sr.sprite;
                    Debug.Log($"[Harpoon] 잡은 물고기 스프라이트: {fishSprite.name}");

                    // 인벤토리로 전달
                    FindObjectOfType<FishInventoryManager>().AddFish(fishSprite);
                }
                else
                {
                    Debug.LogWarning("[Harpoon] SpriteRenderer 또는 sprite가 null입니다!");
                }

                // 실제 피격 처리
                fish.OnHitByHarpoon();
            }

            // 충돌 직후 콜라이더 비활성화
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;

            // 작살 회수 타이밍 약간 딜레이
            Invoke(nameof(ReturnToPool), 0.1f);
        }
        else
        {
            // 벽 등에 부딪힌 경우 즉시 회수
            pool.ReturnHarpoon(gameObject);
        }

        //if (collision.CompareTag("Fish"))
        //{
        //    Debug.Log("Fish hit.");
        //    Debug.Log("충돌한 오브젝트 이름: " + collision.gameObject.name);

        //    Fish fish = collision.GetComponent<Fish>();
        //    if (fish != null)
        //    {
        //        Sprite fishSprite = collision.GetComponentInChildren<SpriteRenderer>()?.sprite;
        //        FindObjectOfType<FishInventoryManager>().AddFish(fishSprite);
        //        fish.OnHitByHarpoon();


        //    }

        //    // 충돌 즉시 콜라이더 비활성화
        //    Collider2D col = GetComponent<Collider2D>();
        //    if (col != null)
        //        col.enabled = false;

        //    // ? 작살 회수 타이밍 0.1초 딜레이
        //    Invoke(nameof(ReturnToPool), 0.1f);
        //}
        //else
        //{
        //    // 그냥 벽 등에 부딪힌 경우 즉시 회수
        //    pool.ReturnHarpoon(gameObject);
        //}

        //if (collision.CompareTag("Fish"))
        //{
        //    Debug.Log("Fish hit.");
        //    Debug.Log("충돌한 오브젝트 이름: " + collision.gameObject.name);

        //    SpriteRenderer sr = collision.GetComponentInChildren<SpriteRenderer>();
        //    if (sr == null)
        //    {
        //        Debug.LogWarning("SpriteRenderer 자체가 없음");
        //    }
        //    else
        //    {
        //        Debug.Log($"SpriteRenderer.sprite: {sr.sprite}");
        //    }

        //    Fish fish = collision.GetComponent<Fish>();
        //    if (fish != null)
        //    {
        //        Sprite fishSprite = sr != null ? sr.sprite : null;
        //        FindObjectOfType<FishInventoryManager>().AddFish(fishSprite);
        //        fish.OnHitByHarpoon();
        //    }

        //    Collider2D col = GetComponent<Collider2D>();
        //    if (col != null)
        //        col.enabled = false;

        //    Invoke(nameof(ReturnToPool), 0.1f);
        //}
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnHarpoon(gameObject);
    }
}
