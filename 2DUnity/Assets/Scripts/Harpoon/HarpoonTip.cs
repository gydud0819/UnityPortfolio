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
        startPos = transform.position; // ���� ��ġ ����
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //  ��� 1�ܰ�: ��Ȱ��ȭ/Ǯ null�̸� �ƹ��͵� �� ��
        if (!gameObject.activeInHierarchy || pool == null) return;

        //  ����� �¾��� ��
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish hit.");

            Fish fish = collision.GetComponent<Fish>();
            if (fish != null)
                fish.OnHitByHarpoon(); // �ڷ�ƾ���� �����
        }

        //  ������ ���� collider ���� (�ߺ� �浹 ����)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        //  �ۻ� ȸ��
        pool.ReturnHarpoon(gameObject);
    }
}
