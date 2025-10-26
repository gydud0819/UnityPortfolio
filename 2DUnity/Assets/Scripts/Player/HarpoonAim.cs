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

        // Animator에 전달
        animator.SetFloat("HarpoonAngle", angle);

        // 좌우판정
        bool isLeft = angle > 90 || angle < -90;
        playerRenderer.flipX = isLeft;

        // 작살 회전 (좌우에 따라 보정)
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
