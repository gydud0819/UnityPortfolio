using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject harpoon;
    [SerializeField] private MovePlayer movePlayer;
    [SerializeField] private PlayerCtrls playerControls;
    [SerializeField] private HarpoonFire harpoonFire;
    public Vector2 HarpoonDirection { get; private set; } = Vector2.right;

    private Vector2 lastMoveDir = Vector2.right;

    private void Awake()
    {
        playerControls = new PlayerCtrls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        HarpoonDirection = lastMoveDir;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {

        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();

        //  ����Ű ��� ����
        if (input.sqrMagnitude > 0.01f)
        {
            if (input.x < -0.5f)
                lastMoveDir = Vector2.left;
            else if (input.x > 0.5f)
                lastMoveDir = Vector2.right;
        }

        if (movePlayer.IsHarpoonReady && Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            harpoonFire.FireHarpoon(HarpoonDirection);
        }

        // ���� ���� ���� �ۻ� ���־� ����
        if (movePlayer.IsHarpoonReady)
        {
            UpdateAimVisual(input);
        }
        else
        {
            animator.SetInteger("AimDir", 0);
        }
    }

    private void SetHarpoonVisual(Vector3 localPos, float zRotation, bool flipX)
    {
        harpoon.transform.localPosition = localPos;
        harpoon.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
        SpriteRenderer sr = harpoon.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = flipX;
    }


    private void UpdateAimVisual(Vector2 input)
    {

        if (!movePlayer.IsHarpoonReady)
        {
            animator.SetInteger("AimDir", 0);
            return;
        }

        if (input.sqrMagnitude > 0.01f)
        {
            if (input.x < -0.5f)
                lastMoveDir = Vector2.left;
            else if (input.x > 0.5f)
                lastMoveDir = Vector2.right;
        }

        bool up = input.y > 0.5f;
        bool down = input.y < -0.5f;
        bool right = input.x > 0.5f;
        bool left = input.x < -0.5f;

        // ������ �밢�� ��
        if (up && right)
        {
            animator.SetInteger("AimDir", 1); // �밢�� ��
            SetHarpoonVisual(new Vector3(0.11f, 0.09f, 0f), 40f, false);
            HarpoonDirection = new Vector2(1f, 1f);
        }
        //������ �밢�� �Ʒ�
        else if (down && right)
        {
            animator.SetInteger("AimDir", 2); // �밢�� �Ʒ�
            SetHarpoonVisual(new Vector3(0.14f, -0.03f, 0f), -30f, false);
            HarpoonDirection = new Vector2(1f, -1f);
        }
        // ���� �밢�� ��
        else if (up && left)
        {
            animator.SetInteger("AimDir", 1); // �밢�� ��
            SetHarpoonVisual(new Vector3(-0.11f, 0.09f, 0f), -40f, true);
            HarpoonDirection = new Vector2(-1f, 1f);
        }
        // ���� �밢�� �Ʒ�
        else if (down && left)
        {
            animator.SetInteger("AimDir", 2); // �밢�� �Ʒ�
            SetHarpoonVisual(new Vector3(-0.14f, -0.03f, 0f), 30f, true);
            HarpoonDirection = new Vector2(-1f, -1f);
        }

        // ���� ����
        else if (lastMoveDir.x < -0.01f)
        {
            animator.SetInteger("AimDir", 0); // ���� ����
            SetHarpoonVisual(new Vector3(-0.14f, 0.03f, 0f), 0f, true);
            HarpoonDirection = Vector2.left;
        }
        else if(lastMoveDir.x > 0.01f)// ���� (������)
        {
            animator.SetInteger("AimDir", 0);
            SetHarpoonVisual(new Vector3(0.14f, 0.03f, 0f), 0f, false);
            HarpoonDirection = Vector2.right;
        }

    }
}
