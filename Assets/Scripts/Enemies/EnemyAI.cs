using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemyAI : MonoBehaviour {
    public Transform enemyGFX;
    public GameObject effect;
    
    private Transform target;
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

    private void Start() {
        target = Player.instance.bodyTransform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        InvokeRepeating(nameof(UpdatePath), 0, .5f);
    }

    void UpdatePath() {
        if (followEnable && TargetInDistance() && seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWapoint = 0;
        }
    }

    private void FixedUpdate() {
        if (rb.velocity.x != 0) {
            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }
        if (TargetInDistance() && followEnable) {
            PathFollow();
        }
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
                rb.AddForce(speed * jumpModifier * Vector2.up);
            }
        }
        
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
        if (distance < nextWaypointDistance) {
            StopAllCoroutines();
            effect.SetActive(false);
            attacking = false;
            currentWapoint++;
        } 

        
        if (directionLookEnabled) {
            if (rb.velocity.x >= 0.01f) {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            } else if (rb.velocity.x <= -0.01f) {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        if (attacking) return;
        if (Vector2.Distance(rb.position, target.position) < distanceToAttack) {
            Attack();
        }
    }

    private void Attack() {
        StartCoroutine(AttackProcess());
    }

    private bool attacking = false;
    private WaitForSeconds wfs = new WaitForSeconds(1f);
    IEnumerator AttackProcess() {
        attacking = true;
        effect.SetActive(false);
        while (true) {
            animator.SetTrigger("Attack");
            effect.SetActive(true);
            yield return wfs;
        }
    }
    
    
    
    bool TargetInDistance() {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }
    

    // private void FixedUpdate() {
    //     if (path == null) return;
    //     if (currentWapoint >= path.vectorPath.Count) {
    //         reachedEndOfPath = true;
    //         return;
    //     } else {
    //         reachedEndOfPath = false;
    //     }
    //
    //     Vector2 direction = ((Vector2)path.vectorPath[currentWapoint] - rb.position).normalized;
    //     Vector2 force =  speed * Time.deltaTime * direction;
    //
    //     rb.AddForce(force);
    //     
    //     float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
    //     if (distance < nextWaypointDistance) {
    //         currentWapoint++;
    //     }
    //     
    //     if (force.x >= 0.01f) {
    //         enemyGFX.localScale = new Vector3(1f, 1f, 1f);
    //     } else if (force.x <= -0.01f) {
    //         enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
    //     }
    // }
}
