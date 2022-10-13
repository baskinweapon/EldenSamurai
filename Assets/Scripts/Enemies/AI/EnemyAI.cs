using System;
using Architecture.Interfaces;
using Pathfinding;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyAI : MonoBehaviour, ICastAbility {
    public Transform lookTransform;
    
    public Transform fallChecker;
    private Transform target;
    
    [Header("Distance to Action")]
    public float distanceToAttack = 2f;
    public float activateDistance = 50f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnable = true;
    public bool jumpEnable = true;
    public bool directionLookEnabled = true;

    public PlayableDirector playableDirector;
    
    private Path path;
    private int currentWapoint;
    private bool isGrounded;
    private bool isGroundedNextWayport;
    
    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D col;
    private Owner owner;

    public Animator animator;

    public Action OnMyWay;
    public Action OnFindTarget;
    public Action OnLostTarget;
    public Action OnAttack;
    
    private void Start() {
        // Set Anim To Hash
        SetAnimToHash();
        
        target = Player.instance.bodyTransform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        owner = GetComponentInParent<Owner>();
        
        attackTime = (float)playableDirector.duration;
        
        InvokeRepeating(nameof(UpdatePath), 0, .5f);
    }

    #region Animation
    
    private static int DamageString;
    private static int DieString;
    private static int AnimState;
    private static int AttackString;
    
    [Header("Animation Name")]
    [SerializeField, TextArea]
    private string damageString;
    [SerializeField, TextArea]
    private string dieString;
    [SerializeField, TextArea]
    private string animState;
    [SerializeField, TextArea]
    private string attackString;
    
    void SetAnimToHash() {
        DamageString = Animator.StringToHash(damageString);
        DieString = Animator.StringToHash(dieString);
        AnimState = Animator.StringToHash(animState);
        AttackString = Animator.StringToHash(attackString);
    }

    private void StateAnim(int _state) {
        animator.SetInteger(AnimState, _state);
    }

    private void AttackAnim() {
        animator.SetTrigger(AttackString);
    }
    
    public void DamageAnim() {
        Debug.Log(damageString);
        animator.SetTrigger(DamageString);
    }

    public void DieAnim() {
        animator.SetTrigger(DieString);
        Invoke(nameof(OnDestroy), 0.5f);
    }

    #endregion


    void UpdatePath() {
        if (followEnable && TargetInDistance() && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        // else if (mustPatrol && Vector2.Distance(rb.position, spawnedPosition) >= distanceToReturn && seeker.IsDone()) {
        //     seeker.StartPath(rb.position, spawnedPosition, OnPathComplete);
        // }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWapoint = 0;
        }
    }

    private void FixedUpdate() {
        if (isCasting) {
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        
        //anim
        StateAnim(rb.velocity.x != 0 ? 1 : 0);
        
        if (TargetInDistance() && followEnable) {
            PathFollow();
        } 
    }

    private float attackTime;
    void PathFollow() {
        if (path == null) return;
        if (currentWapoint >= path.vectorPath.Count) return;
        
        isGrounded = IsGrounded();
        isGroundedNextWayport = IsGroundedNextWayport();

        // if (!isGrounded) return;

        if (isAttack) {
            attackTime -= Time.deltaTime;
            if (attackTime < 0) {
                attackTime = (float)playableDirector.duration;
                isAttack = false;
            }
            return;
        }
        
        SetDirection();
        if (Vector2.Distance(rb.position, target.position) < distanceToAttack) {
            Attack();
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWapoint] - rb.position).normalized;
        Vector2 force = speed * Time.deltaTime * direction;
        force.y = 0f;
        
        //Jump
        if (jumpEnable && isGrounded) {
            if (direction.y > jumpNodeHeightRequirement) 
                rb.velocity += speed * jumpModifier * Vector2.up;
        }

        if (isGrounded)
            rb.velocity += force;

        if (!isGroundedNextWayport) {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
        if (distance < nextWaypointDistance) {
            currentWapoint++;
        }
    }

    private bool isAttack;
    private void Attack() {
        rb.velocity = Vector2.zero;
        playableDirector.Play();
        isAttack = true;
        // AttackAnim();
        // OnAttack?.Invoke();
    }

    private void SetDirection() {
        if (!directionLookEnabled) return;
        if (rb.velocity.x >= 0.01f || Player.instance.bodyTransform.position.x > rb.position.x) {
            lookTransform.localScale = new Vector3(
                1f,
                lookTransform.localScale.y,
                lookTransform.localScale.z);
        } else if (rb.velocity.x <= -0.01f || Player.instance.bodyTransform.position.x < rb.position.x) {
            lookTransform.localScale = new Vector3(
                -1f,
                lookTransform.localScale.y,
                lookTransform.localScale.z);
        }
    }

    private void OnDestroy() {
        Destroy(owner.gameObject);
    }
    
    bool TargetInDistance() {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    bool IsGrounded() {
        var startOffset = transform.position - new Vector3(0, col.bounds.extents.y + jumpCheckOffset);
        return Physics2D.Raycast(startOffset, -Vector2.up, 0.05f);
    }

    bool IsGroundedNextWayport() {
        fallChecker.localPosition = lookTransform.localScale.x < 0 ? new Vector3(-1, -1f, 0f) : new Vector3(1, -1f, 0f);
        return Physics2D.Raycast(fallChecker.position, Vector2.down, 0.5f, LayerMask.GetMask("Obstacle"));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }
    
#endif
   

    private bool isCasting;
    public void StartCasting() {
        isCasting = true;
    }

    public void EndCasting() {
        isCasting = false;
    }
}
