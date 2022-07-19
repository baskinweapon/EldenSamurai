using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour {
    public float playerSpeed = 10f;
    
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 1f;
    public float jumpMultiplier = 1f;
    
    private bool isGrounded;
    private Animator animator;
    private Rigidbody2D rb;
    private CapsuleCollider2D collider;
    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Start() {
        InputSystem.OnJump += Jump;
    }

    private void Update() {
        isGrounded = IsGrounded();
    }

    private void Jump() {
        if (!isGrounded) return;
        animator.SetTrigger("Jump");
        rb.velocity += Vector2.up * jumpMultiplier;
    }

    private void FixedUpdate() {
        if (rb.velocity.y < 0) {
            rb.velocity += Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime * Vector2.up;
        } else if (rb.velocity.y > 0 && !InputSystem.instance.IsJumping()) {
            rb.velocity += Physics.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime * Vector2.up;
        }
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        float move = InputSystem.instance.GetMoveVector().x;
        if (move != 0f) {
            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }
        
        transform.rotation = Quaternion.Euler(0, move > 0 ? 0 : 180, 0);
        
        Vector2 velocity = new Vector2(move * playerSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    private float extraHeight = .05f;
    private bool IsGrounded() {
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + extraHeight, LayerMask.GetMask("Tilemap"));
        Color rayColor;
        if (hit.collider != null) {
            rayColor = Color.green;
        } else rayColor = Color.red;
        Debug.DrawRay(collider.bounds.center, Vector2.down * (collider.bounds.extents.y + extraHeight), rayColor);
        return hit.collider != null;
    }
}
