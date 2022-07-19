using System;
using System.Collections;
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
    private SpriteRenderer spriteRenderer;
    
    [Header("Effects")]
    [SerializeField] GameObject m_RunStopDust;
    [SerializeField] GameObject m_LandingDust;
    
    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        SpawnDustEffect(m_LandingDust, 1);
        rb.velocity += Vector2.up * jumpMultiplier;
    }
    
    private bool isMove;
    private void FixedUpdate() {
        if (rb.velocity.y < 0) {
            rb.velocity += Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime * Vector2.up;
        } else if (rb.velocity.y > 0 && !InputSystem.instance.IsJumping()) {
            rb.velocity += Physics.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime * Vector2.up;
        }
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        float move = InputSystem.instance.GetMoveVector().x;
        if (move != 0f) {
            if (!isMove)
                SpawnDustEffect(m_RunStopDust, move < 0 ? -1 : 1);
            isMove = true;
            animator.SetInteger("AnimState", 1);
        } else {
            isMove = false;
            animator.SetInteger("AnimState", 0);
        }

        if (move > 0) spriteRenderer.flipX = true;
        else if (move < 0) spriteRenderer.flipX = false;
        
        Vector2 velocity = new Vector2(move * playerSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    private float extraHeight = .05f;
    private bool IsGrounded() {
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + extraHeight, LayerMask.GetMask("Tilemap"));
        Color rayColor;
        var isGround = hit.collider != null;
        if (isGround) {
            rayColor = Color.green;
        } else rayColor = Color.red;
        Debug.DrawRay(collider.bounds.center, Vector2.down * (collider.bounds.extents.y + extraHeight), rayColor);
        return isGround;
    }
    
    void SpawnDustEffect(GameObject dust, int dir, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(dir, 1, 1);
        }
    }
}
