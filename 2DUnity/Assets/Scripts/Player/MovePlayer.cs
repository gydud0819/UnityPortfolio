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

    [SerializeField] private GameObject harpoon;

    [SerializeField] private float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }
        

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Hold.performed -= OnHarpoonToggle;
    }

    private void Update()
    {
        Vector2 moveDir = playerControls.Player.Move.ReadValue<Vector2>();
        rigidbody2D.linearVelocity = moveDir * speed;

        bool isHorizontalOnly = Mathf.Abs(moveDir.x) > 0.01f && Mathf.Abs(moveDir.y) <= 0.01f;
        animator.SetBool("IsMoving", moveDir != Vector2.zero);
        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);

        if (moveDir.x > 0.01)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (moveDir.x < -0.01)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            spriteRenderer.flipY = true;
        }
        else if(Mathf.Abs(moveDir.y) > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipY = false;
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

    private void OnHarpoonToggle(InputAction.CallbackContext context)
    {
        harpoon.SetActive(true);
        animator.SetBool("HasHarpoon", true);
        // 일정 시간 후 자동 비활성화 시키고 싶으면
        //StartCoroutine(HideHarpoonAfterDelay(0.5f));
    }

    private IEnumerator HideHarpoonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        harpoon.SetActive(false);
        animator.SetBool("HasHarpoon", false);
    }
}





