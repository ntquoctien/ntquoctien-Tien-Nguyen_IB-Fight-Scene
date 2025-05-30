using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody rb;
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    private bool hasJumped = false;
    private bool isAttacking = false;
    public Button jumpButton;

    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
        animator.SetBool("isAttacking", attacking);
    }
    void Start()
    {
        joystick = FindAnyObjectByType<Joystick>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (jumpButton == null)
        {
            jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        HandleMovement();
        HandleJump();
        HandleAnimation();
    }
    private void HandleMovement()
    {
        if (isAttacking) return;

        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);

        rb.velocity = new Vector3(moveX * moveSpeed, rb.velocity.y, moveZ * moveSpeed);
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }
    private void HandleJump()
    {
        if (jumpButton.isPressed && isGrounded && !hasJumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            hasJumped = true;
        }
        if (!jumpButton.isPressed && isGrounded)
        {
            hasJumped = false;
        }
    }

    private void HandleAnimation()
    {
        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;
        bool isWalking = Mathf.Abs(moveX) > 0.2f || Mathf.Abs(moveZ) > 0.2f;
        bool isJumping = !isGrounded;
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);
    }

}
