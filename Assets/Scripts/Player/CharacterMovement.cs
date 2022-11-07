using Architecture.Interfaces;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, ICastAbility {
    public float playerSpeed = 10f;
    
    [Header("Jump Settings")]
    public float lowJumpMultiplier = 1f;
    public float jumpMultiplier = 1f;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [Header("Physics")]
    [SerializeField] private CapsuleCollider2D col;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Effects")]
    [SerializeField] GameObject m_RunStopDust;
    [SerializeField] GameObject m_LandingDust;
    
    private bool isGrounded;
    
    private void Start() {
        InputSystem.OnJump += Jump;
    }

    private void Jump() {
        if (!isGrounded) return;
        animator.SetTrigger(JumpString);
        rb.velocity += Vector2.up * jumpMultiplier;
    }
    
    private void Update() {
        isGrounded = IsGrounded();
    }
    
    private void FixedUpdate() {
        if (isCasting) {
            animator.SetInteger(AnimState, 0);
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        if (rb.velocity.y <= 0) {
            rb.velocity += Physics.gravity.y * Time.deltaTime * Vector2.up;
        } else if (rb.velocity.y > 0 && !InputSystem.instance.IsJumping()) {
            rb.velocity += Physics.gravity.y * lowJumpMultiplier * Time.deltaTime * Vector2.up;
        }
        
        // Anim
        if (rb.velocity.y > 0) {
            animator.SetFloat(AirSpeedY, 0);
        } else if (rb.velocity.y < -1f) {
            animator.SetFloat(AirSpeedY, 1f);
        }

        float move = InputSystem.instance.GetMoveVector().x;
        if (isGrounded) {
            animator.SetFloat(AnimState, Mathf.InverseLerp(0, 1, Mathf.Abs(move)));
        }

        if (move > 0) spriteRenderer.flipX = false;
        else if (move < 0) spriteRenderer.flipX = true;
        
        Vector2 velocity = new Vector2(move * playerSpeed * Time.deltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    private const float extraHeight = .05f;

    private bool IsGrounded() {
        var bound = col.bounds;
        RaycastHit2D hit = Physics2D.Raycast(bound.center, Vector2.down, bound.extents.y + extraHeight, LayerMask.GetMask("Obstacle"));
        var isGround = hit.collider != null;

        // Debug
        var rayColor = isGround ? Color.green : Color.red;
        Debug.DrawRay(bound.center, Vector2.down * (bound.extents.y + extraHeight), rayColor);
        
        animator.SetBool(Grounded, isGround);
        
        return isGround;
    }
    
    void SpawnDustEffect(GameObject dust, int dir, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = (Vector3)rb.position + new Vector3(dustXOffset, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity);
            // Turn dust in correct X direction
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(dir, 1, 1);
        }
    }

    private bool isCasting;
    private static readonly int JumpString = Animator.StringToHash("Jump");
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int AirSpeedY = Animator.StringToHash("VelocityY");
    private static readonly int Grounded = Animator.StringToHash("IsGround");

    public void StartCasting() {
        isCasting = true;
    }

    public void EndCasting() {
        isCasting = false;
    }
}
