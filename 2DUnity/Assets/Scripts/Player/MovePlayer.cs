using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
[RequireComponent(typeof(Rigidbody2D))]

public class MovePlayer : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    PlayerCtrls playerControls;
    SpriteRenderer spriteRenderer;

    public bool IsHarpoonReady { get; private set; } = false; // HarpoonFire.cs에서 접근용

    [SerializeField] private GameObject harpoon;
    [SerializeField] private float speed = 5f;

    private Vector2 lastMoveDir = Vector2.right;  // 최근 방향 기억용
    private Vector2 inputDirection;

    private Vector2 harpoonDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// 초기화 처리
    /// </summary>
    private void Awake()
    {
        playerControls = new PlayerCtrls();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        harpoon.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Hold.performed += OnHarpoonToggle;
        playerControls.Player.Hold.canceled += OnHarpoonRelease;
    }


    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Hold.performed -= OnHarpoonToggle;
        playerControls.Player.Hold.canceled -= OnHarpoonRelease;
    }

    private void Update()
    {
        Move();
        HarpoonAnim();
    }

    /// <summary>
    /// 플레이어 이동 처리
    /// </summary>
    private void Move()
    {
        if (IsHarpoonReady) // 작살 장전 중엔 이동하지 않음
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            animator.speed = 0.5f;
            return;
        }


        Vector2 moveDir = playerControls.Player.Move.ReadValue<Vector2>();
        rigidbody2D.linearVelocity = moveDir * speed;
        inputDirection = moveDir;

        bool isHorizontalOnly = Mathf.Abs(moveDir.x) > 0.01f && Mathf.Abs(moveDir.y) <= 0.01f;

        if (!IsHarpoonReady)
        {
            if (moveDir.x > 0.01f)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                lastMoveDir = Vector2.right;
            }
            else if (moveDir.x < -0.01f)
            {
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = true;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                lastMoveDir = Vector2.left;
            }
            else if (Mathf.Abs(moveDir.y) > 0.01f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else // 멈췄을 때
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spriteRenderer.flipY = false;

                // idle 상태에서 방향 유지
                spriteRenderer.flipX = (lastMoveDir == Vector2.left);
            }

            if (moveDir == Vector2.zero)
            {
                animator.speed = 0.5f;  // Idle 상태: 천천히 흔들
            }
            else
            {
                animator.speed = 1.5f;  // 이동 중: 빠르게 수영
            } 
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


    private void HarpoonAnim()
    {
        if (!IsHarpoonReady)
        {
            animator.SetInteger("AimDir", 0);
            return;
        }

        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();

        if(input.sqrMagnitude > 0.01f)
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

        // 오른쪽 대각선 위
        if (up && right)
        {
            animator.SetInteger("AimDir", 1); // 대각선 위
            SetHarpoonVisual(new Vector3(0.11f, 0.09f, 0f), 40f, false);
            harpoonDirection = new Vector2(1f, 1f);
        }
        //오른쪽 대각선 아래
        else if (down && right)
        {
            animator.SetInteger("AimDir", 2); // 대각선 아래
            SetHarpoonVisual(new Vector3(0.14f, -0.03f, 0f), -30f, false);
            harpoonDirection = new Vector2(1f, -1f);
        }
        // 왼쪽 대각선 위
        else if (up && left)
        {
            animator.SetInteger("AimDir", 1); // 대각선 위
            SetHarpoonVisual(new Vector3(-0.11f, 0.09f, 0f), -40f, true);
            harpoonDirection = new Vector2(-1f, 1f);
        }
        // 왼쪽 대각선 아래
        else if (down && left)
        {
            animator.SetInteger("AimDir", 2); // 대각선 아래
            SetHarpoonVisual(new Vector3(-0.14f, -0.03f, 0f), 30f, true);
            harpoonDirection = new Vector2(-1f, -1f);
        }

        // 정면 왼쪽
        else if (lastMoveDir.x <= -0.5f)
        {
            animator.SetInteger("AimDir", 0); // 왼쪽 정면
            SetHarpoonVisual(new Vector3(-0.14f, 0.03f, 0f), 0f, true);
            harpoonDirection = Vector2.left;
        }
        else // 정면 (오른쪽)
        {
            animator.SetInteger("AimDir", 0);
            SetHarpoonVisual(new Vector3(0.14f, 0.03f, 0f), 0f, false);
            harpoonDirection = Vector2.right;
        }



    }

    /// <summary>
    /// 작살 활성화 처리
    /// </summary>
    /// <param name="context"></param>
    private void OnHarpoonToggle(InputAction.CallbackContext context)
    {
        animator.SetInteger("AimDir", 0);
        harpoon.transform.localPosition = new Vector3(0.14f, 0.03f, 0f);
        harpoon.transform.localRotation = Quaternion.Euler(0, 0, 0f);

        harpoon.SetActive(true);
        animator.SetBool("IsHoldingHarpoon", true);
        IsHarpoonReady = true;
    }

    private void OnHarpoonRelease(InputAction.CallbackContext context)
    {
        harpoon.SetActive(false);
        animator.SetBool("IsHoldingHarpoon", false);
        IsHarpoonReady = false;
        
    }

    public Vector2 GetHarpoonDirection()
    {
        return harpoonDirection.normalized;
    }

}





