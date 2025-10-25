using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarpoonFire : MonoBehaviour
{
    [SerializeField] private GameObject harpoonTip;
    [SerializeField] private Transform firePoint; // 총구 위치
    [SerializeField] private MovePlayer movePlayer;                  // 작살 들고 있는지 체크용
    [SerializeField] private float fireSpeed = 10f;

    private PlayerCtrls playerControls;
    private Rigidbody2D harpoon2D;

    private bool isFiring = false;

    private void Awake()
    {
        playerControls = new PlayerCtrls();

        harpoon2D = harpoonTip.GetComponent<Rigidbody2D>();
        harpoonTip.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Fire.performed += OnFireHarpoon;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Fire.performed -= OnFireHarpoon;
    }

    private void OnFireHarpoon(InputAction.CallbackContext context)
    {
        if (!movePlayer.IsHarpoonReady || isFiring) return;

        isFiring = true;
        harpoonTip.SetActive(true);
        harpoonTip.transform.position = firePoint.position;
        // 마우스 방향으로 쏘기
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        harpoon2D.linearVelocity = direction * fireSpeed;

        // 방향에 따라 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        harpoonTip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Invoke("ResetHarpoon", 1.0f); // 1초 후에 작살 리셋
    }

    private void ResetHarpoon()
    {
        harpoonTip.SetActive(false);
        harpoon2D.linearVelocity = Vector2.zero;
        harpoonTip.transform.position = firePoint.position;
        isFiring = false;
    }

}
