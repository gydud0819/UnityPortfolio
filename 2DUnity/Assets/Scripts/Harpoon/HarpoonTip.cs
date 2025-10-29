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
        //  방어 1단계: 비활성화/풀 null이면 아무것도 안 함
        if (!gameObject.activeInHierarchy || pool == null) return;

        //  물고기 맞았을 때
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish hit.");

            Fish fish = collision.GetComponent<Fish>();
            if (fish != null)
                fish.OnHitByHarpoon(); // 코루틴으로 사라짐
        }

        //  안전을 위해 collider 끄기 (중복 충돌 방지)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        //  작살 회수
        pool.ReturnHarpoon(gameObject);
    }
}
