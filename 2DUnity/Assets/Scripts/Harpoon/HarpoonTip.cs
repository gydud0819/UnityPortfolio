using UnityEngine;

public class HarpoonTip : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 6f;

    private Vector3 direction;
    private HarpoonPool pool;

    // 🔹 HarpoonPool에서 연결할 때 호출됨
    public void SetPool(HarpoonPool poolRef)
    {
        pool = poolRef;
    }

    // 🔹 작살 발사 시 초기화
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
            pool?.ReturnHarpoon(gameObject);
        }
    }

    void OnEnable()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy || pool == null) return;

        if (collision.CompareTag("Fish"))
        {
            Fish fish = collision.GetComponent<Fish>();
            if (fish == null) return;
            if (fish.isCaught) return;

            fish.isCaught = true;
            string fishName = fish.fishType.ToString();

            SpriteRenderer sr = collision.GetComponent<SpriteRenderer>();
            Sprite fishSprite = sr != null ? sr.sprite : null;

            Debug.Log($"🎯 [HarpoonTip] {fishName} 잡음!");

            OceanManager oceanManager = FindObjectOfType<OceanManager>();
            if (oceanManager != null)
            {
                try
                {
                    oceanManager.AddCaughtFish(fishName);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[HarpoonTip] AddCaughtFish 실패: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("[HarpoonTip] OceanManager를 찾을 수 없음");
            }

            fish.OnHitByHarpoon();

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Invoke(nameof(ReturnToPool), 0.1f);
        }
        else
        {
            pool?.ReturnHarpoon(gameObject);
        }
    }

    private void ReturnToPool()
    {
        pool?.ReturnHarpoon(gameObject);
    }
}
