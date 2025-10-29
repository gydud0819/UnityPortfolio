using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarpoonFire : MonoBehaviour
{

    [SerializeField] private HarpoonPool harpoonPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float harpoonSpeed = 10f;

    public void FireHarpoon(Vector2 direction)
    {
        GameObject harpoon = harpoonPool.GetHarpoon();
        harpoon.transform.position = firePoint.position;
        harpoon.transform.rotation = Quaternion.identity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        harpoon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * harpoonSpeed;

        // ���⿡ ���� ��������Ʈ ���� (����)
        SpriteRenderer sr = harpoon.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = direction.x < 0;

        // �ۻ� ���� �Ѱ��ֱ�
        HarpoonTip tip = harpoon.GetComponent<HarpoonTip>();
        if (tip != null)
            tip.Fire(direction, harpoonPool);
    }
}
