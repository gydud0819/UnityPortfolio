using UnityEditor.ShaderGraph;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    PlayerControls playerControls;
    SpriteRenderer spriteRenderer;

    [SerializeField]  private float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    private void Update()
    {
        Vector2 moveDir = playerControls.Player.Move.ReadValue<Vector2>();
        rigidbody2D.linearVelocity = moveDir * moveSpeed;

        bool isHorizontalOnly = Mathf.Abs(moveDir.x) > 0.01f && Mathf.Abs(moveDir.y) <= 0.01f;
        animator.SetBool("IsMoving", moveDir != Vector2.zero);
        animator.SetFloat("MoveX", moveDir.x);

        if(moveDir.x > 0.01)
        {
            spriteRenderer.flipX = false;
        }
        else if(moveDir.x < -0.01)
        {
            spriteRenderer.flipX = true;
        }
    }

}
