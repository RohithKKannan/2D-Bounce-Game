using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BounceGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float wallSlidingSpeed = 2.5f;
        [SerializeField] float groundCheckRadius = 0.1f;
        [SerializeField] float wallCheckRadius = 0.1f;

        [Header("References")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask resetLayer;
        [SerializeField] private LayerMask wallLayer;

        private Rigidbody2D rb;

        private bool isGrounded;
        private bool isWalled;
        private bool isWallSliding;

        private bool isWallJumping;
        private float wallJumpingCounter;
        private int currentJumpCount;

        [Header("Wall Jump")]
        [SerializeField] private int wallJumpLimit = 2;
        [SerializeField] private float wallJumpingTime = 0.2f;
        [SerializeField] private float wallJumpingDuration = 0.4f;
        [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

        private float horizontalInput;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            isWalled = Physics2D.OverlapCircle(groundCheck.position, wallCheckRadius, wallLayer);

            horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            WallSlide();
            WallJump();

            if (isGrounded)
            {
                currentJumpCount = 0;
            }

            if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, resetLayer))
            {
                transform.position = Vector2.zero;
                horizontalInput = 0f;
                rb.velocity = Vector2.zero;
            }
        }

        private void WallSlide()
        {
            if (isWalled && !isGrounded && horizontalInput != 0)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }
        }

        private void WallJump()
        {
            if (isWallSliding)
            {
                isWallJumping = false;
                wallJumpingCounter = wallJumpingTime;

                CancelInvoke(nameof(StopWallJump));
            }
            else
            {
                wallJumpingCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f && currentJumpCount < wallJumpLimit)
            {
                isWallJumping = true;
                rb.velocity = new Vector2(-horizontalInput * wallJumpingPower.x, wallJumpingPower.y);
                wallJumpingCounter = 0;
                currentJumpCount++;

                Invoke(nameof(StopWallJump), wallJumpingDuration);
            }
        }

        private void StopWallJump()
        {
            isWallJumping = false;
        }

        private void FixedUpdate()
        {
            if (!isWallJumping)
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
    }
}
