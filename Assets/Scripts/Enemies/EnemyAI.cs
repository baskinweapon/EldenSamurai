using System;
using Abilities;
using Architecture.Interfaces;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour, ICastAbility {
    public SpriteRenderer enemyGFX;

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
    
    private Path path;
    private int currentWapoint = 0;
    private bool isGrounded = false;
    private bool isGroundedNextWayport = false;
        
    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D col;
    private Owner owner;

    public Animator animator;

    public Action OnMyWay;
    public Action OnFindTarget;
    public Action OnLostTarget;
    public Action OnAttack;

    [Header("Patrol")]
    public bool mustPatrol;
    public float distanceToReturn = 3f;

    private Vector3 spawnedPosition;
    
    private void Start() {
        target = Player.instance.bodyTransform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        owner = GetComponentInParent<Owner>();

        spawnedPosition = rb.position;
        InvokeRepeating(nameof(UpdatePath), 0, .5f);
    }

    #region Animation

    public void DamageAnim() {
        animator.SetTrigger("Damage");
    }

    public void DieAnim() {
        animator.SetTrigger("Die");
        Invoke(nameof(OnDestroy), 0.2f);
    }

    #endregion

    void UpdatePath() {
        if (followEnable && TargetInDistance() && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        else if (mustPatrol && Vector2.Distance(rb.position, spawnedPosition) >= distanceToReturn && seeker.IsDone()) {
            seeker.StartPath(rb.position, spawnedPosition, OnPathComplete);
        }
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
            animator.SetInteger("AnimState", 0);
        }
        
        //anim
        if (rb.velocity.x != 0) {
            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }
        
        if (TargetInDistance() && followEnable) {
            PathFollow();
        } else if (mustPatrol) {
            Patrol();
        }
    }

    private void Patrol() {
        PathFollow();
    }

    void PathFollow() {
        if (path == null) return;
        if (currentWapoint >= path.vectorPath.Count) return;

        isGrounded = IsGrounded();
        isGroundedNextWayport = IsGroundedNextWayport();
        
        if (isGroundedNextWayport) {
            rb.velocity = Vector2.zero;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWapoint] - rb.position).normalized;
        Debug.Log("Direction = " + direction);
        Vector2 force =  speed * Time.deltaTime * direction;
        
        Debug.Log("Force = " + force);
        //Jump
        if (jumpEnable && isGrounded) {
            if (direction.y > jumpNodeHeightRequirement) 
                rb.velocity += speed * jumpModifier * Vector2.up;
        }

        rb.velocity += force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
        if (distance < nextWaypointDistance) {
            StopAllCoroutines();
            currentWapoint++;
        }
        
        if (directionLookEnabled) {
            if (rb.velocity.x >= 0.01f) {
                enemyGFX.flipX = false;
            } else if (rb.velocity.x <= -0.01f) {
                enemyGFX.flipX = true;
            }
        }

        if (!isGrounded) return;
        if (Vector2.Distance(rb.position, target.position) < distanceToAttack) {
            Attack();
        }
    }
    
    private void Attack() {
        OnAttack?.Invoke();
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
        fallChecker.localPosition = enemyGFX.flipX ? new Vector3(-1, -1f, 0f) : new Vector3(1, -1f, 0f);
        return Physics2D.Raycast(fallChecker.position, -Vector2.up, 0.05f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }

    private bool isCasting;
    public void StartCasting() {
        isCasting = true;
    }

    public void EndCasting() {
        isCasting = false;
    }
}
