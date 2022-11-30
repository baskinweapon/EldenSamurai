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
    public Rigidbody2D rb;
    
    [SerializeField] private GameObject attackColider;
    
    
    private bool isGrounded;

    private PlayerStates state;
    public void ChangeState(PlayerStates _state) {
        state = _state;
    }
    
    private void Start() {
        InputSystem.OnJump += Jump;
        InputSystem.OnFirstAbility += Attack;

        // state = new IdleState(this);
    }

    private void Jump() {
        state.PressJump();
        
        if (!isGrounded) return;
        animator.SetTrigger(JumpString);
        rb.velocity += Vector2.up * jumpMultiplier;
    }
    
    private void Attack() {
        state.PressAttack();
        
        animator.SetTrigger("Attack");
        isCasting = true;
        attackColider.SetActive(true);
        Invoke(nameof(EndCasting), 0.3f);
    }
    
    
    private void FixedUpdate() {
        isGrounded = IsGrounded();

        if (passDamage) {
            state.PassDamage();
            Invoke(nameof(WaitDamagePass), Player.instance.damagePassTime);
            return;
        }
        
        if (isCasting) {
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

        if (move > 0) spriteRenderer.transform.localScale = new Vector3(Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);
        else if (move < 0) spriteRenderer.transform.localScale = new Vector3(-Mathf.Abs(spriteRenderer.transform.localScale.x), spriteRenderer.transform.localScale.y, spriteRenderer.transform.localScale.z);;
        
        Vector2 velocity = new Vector2(move * playerSpeed * Time.deltaTime, rb.velocity.y);
        rb.velocity = velocity;
    }

    
    //Activate Damage effect
    private bool passDamage;
    private void OnCollisionEnter2D(Collision2D col) {
        state.PassDamage();
        
        if (passDamage) return;
        var velocity = col.relativeVelocity;
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("Damager")) {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                Player.instance.health.Damage(50f, col.collider);
            }
            
            passDamage = true;
            animator.SetTrigger("Damaged");
            rb.AddForce(Vector2.Reflect(velocity,Vector2.up) * 1000f);
        }
    }

    private void WaitDamagePass() {
        passDamage = false;
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
    
    private bool isCasting;
    private static readonly int JumpString = Animator.StringToHash("Jump");
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int AirSpeedY = Animator.StringToHash("VelocityY");
    private static readonly int Grounded = Animator.StringToHash("IsGround");

    public void StartCasting() {
        isCasting = true;
    }

    public void EndCasting() {
        attackColider.SetActive(false);
        isCasting = false;
    }
}
