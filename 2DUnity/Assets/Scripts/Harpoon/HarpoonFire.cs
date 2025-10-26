using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarpoonFire : MonoBehaviour
{
    [SerializeField] private GameObject harpoonTip;
    [SerializeField] private Transform firePoint; // �ѱ� ��ġ
    [SerializeField] private MovePlayer movePlayer;                  // �ۻ� ��� �ִ��� üũ��
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
        harpoonTip.transform.position = firePoint.position;

        // �÷��̾ ������ ������ �������� �߻�
        Vector2 direction = movePlayer.GetHarpoonDirection();

        // ���⿡ ���� ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        harpoonTip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        harpoonTip.SetActive(true);
        harpoonTip.GetComponent<HarpoonTip>().Fire(direction);

        Invoke("ResetHarpoon", 1.0f); // 1�� �Ŀ� �ۻ� ����
    }

    private void ResetHarpoon()
    {
        harpoonTip.SetActive(false);
        harpoon2D.linearVelocity = Vector2.zero;
        harpoonTip.transform.position = firePoint.position;
        isFiring = false;
    }

}
