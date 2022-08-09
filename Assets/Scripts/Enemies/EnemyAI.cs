using System;
using System.Collections;
using Abilities;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    
    public BaseAbilityCooldown abilityCooldown;
    public Transform enemyGFX;
    
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
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D col;

    public Animator animator;

    public Action OnMyWay;
    public Action OnFindTarget;
    public Action OnLostTarget;
    public Action OnAttack;

    [Header("Patrol")]
    public bool mustPatrol;
    public float distanceToSpawnPoint = 3f;

    private Vector3 spawnedPosition;
    
    private void Start() {
        target = Player.instance.bodyTransform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        spawnedPosition = rb.position;
        InvokeRepeating(nameof(UpdatePath), 0, .5f);
    }

    #region Animation

    public void DamageAnim() {
        animator.SetTrigger("Damage");
    }

    public void DieAnim() {
        animator.SetTrigger("Die");
        
        Invoke(nameof(OnDestroy), 1f);
    }

    #endregion

    void UpdatePath() {
        if (followEnable && TargetInDistance() && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        else if (mustPatrol && Vector2.Distance(rb.position, spawnedPosition) >= distanceToSpawnPoint) {
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

    public void Patrol() {
        PathFollow();
    }

    void PathFollow() {
        if (path == null) return;
        if (currentWapoint >= path.vectorPath.Count) return;

        var startOffset = transform.position - new Vector3(0, col.bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector2.up, 0.05f);

        Vector2 direction = ((Vector2)path.vectorPath[currentWapoint] - rb.position).normalized;
        Vector2 force =  speed * Time.deltaTime * direction;
        
        //Jump
        if (jumpEnable && isGrounded) {
            if (direction.y > jumpNodeHeightRequirement) {
                rb.velocity += speed * jumpModifier * Vector2.up;
            }
        }

        rb.velocity += force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
        if (distance < nextWaypointDistance) {
            StopAllCoroutines();
            currentWapoint++;
        } 
        
        if (directionLookEnabled) {
            if (rb.velocity.x >= 0.01f) {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            } else if (rb.velocity.x <= -0.01f) {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        
        if (Vector2.Distance(rb.position, target.position) < distanceToAttack) {
            Attack();
        }
    }

    private void Attack() {
        if (abilityCooldown.CooldownComplete())
            animator.SetTrigger("Attack");
        abilityCooldown.Triggered();
    }

    private void OnDestroy() {
        Destroy(gameObject);   
    }
    
    bool TargetInDistance() {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }

}
