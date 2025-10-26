using UnityEngine;
using UnityEngine.InputSystem;

public class HarpoonAim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private Transform harpoon;

    private bool isAiming = false;

    // Update is called once per frame
    void Update()
    {
        if (!isAiming) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Animator�� ����
        animator.SetFloat("HarpoonAngle", angle);

        // �¿�����
        bool isLeft = angle > 90 || angle < -90;
        playerRenderer.flipX = isLeft;

        // �ۻ� ȸ�� (�¿쿡 ���� ����)
        if (!isLeft)
            harpoon.rotation = Quaternion.Euler(0, 0, angle);
        else
            harpoon.rotation = Quaternion.Euler(0, 180, -angle);
    }

    public void SetAim(bool aiming)
    {
        isAiming = aiming;
        animator.SetBool("IsHoldingHarpoon", aiming);
        harpoon.gameObject.SetActive(aiming);
    }
}
