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

            Fish fish = collision.GetComponent<Fish>();
            if (fish != null)
                fish.OnHitByHarpoon();

            // 충돌 즉시 콜라이더 비활성화
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;

            // ? 작살 회수 타이밍 0.1초 딜레이
            Invoke(nameof(ReturnToPool), 0.1f);
        }
        else
        {
            // 그냥 벽 등에 부딪힌 경우 즉시 회수
            pool.ReturnHarpoon(gameObject);
        }
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnHarpoon(gameObject);
    }
}
