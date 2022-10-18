using System;
using System.Collections;
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
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWapoint = 0;
        }
    }

    enum Stage {
        sleep,
        patrol,
        casting,
        findPath,
        jump,
        attack,
    }

    private Stage s = Stage.sleep;
    private Vector2 force;
    private void FixedUpdate() {
        isGrounded = IsGrounded();
        isGroundedNextWayport = IsGroundedNextWayport();
        
        //anim
        // StateAnim(rb.velocity.x != 0 ? 1 : 0);
        
        force = Vector2.zero;
        switch (s) {
            case Stage.sleep:
                Debug.Log("Sleep stage");
                s = Stage.findPath;
                break;
            case Stage.patrol:
                Debug.Log("Patrol stage");
                if (TargetInDistance() && followEnable)
                    s = Stage.findPath;
                break;
            case Stage.casting:
                Debug.Log("Casting stage");
                break;
            case Stage.findPath:
                Debug.Log("Find Path stage");
                PathFollow();
                break;
            case Stage.attack:
                Debug.Log("Attack stage");
                break;
            case Stage.jump:
                Debug.Log("Jump stage");
                break;
        }

        if (isGrounded) {
            Debug.DrawRay(rb.position, rb.position + force);
            rb.AddForce(force);
        }
    }
    
    private void LateUpdate() {
        if (path == null) return;
        if (currentWapoint >= path.vectorPath.Count) return;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWapoint]);
        if (distance < nextWaypointDistance) {
            currentWapoint++;
        }
    }
    
    void PathFollow() {
        if (path == null) return;
        if (currentWapoint >= path.vectorPath.Count) return;
        
        if (Vector2.Distance(rb.position, target.position) < distanceToAttack) {
            Attack();
            s = Stage.attack;
        }
        
        SetLookSide();
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWapoint] - rb.position).normalized;

        
        force = speed * direction;
        
        if (jumpEnable && isGrounded) {
            if (direction.y > jumpNodeHeightRequirement) {
                force += speed * jumpModifier * Vector2.up;
            }
        }
    }
    
    private void Attack() {
        playableDirector.Play();
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine() {
        yield return new WaitForSeconds((float)playableDirector.duration);
        s = Stage.patrol;
    }

    private void SetLookSide() {
        if (!directionLookEnabled) return;
        var distanceToPlayer = Player.instance.bodyTransform.position.x > rb.position.x;
        if (rb.velocity.x >= 0.01f || distanceToPlayer)
            lookTransform.ChangeScale(x: 1f);
        if (rb.velocity.x <= -0.01f || !distanceToPlayer)
            lookTransform.ChangeScale(x: -1f);
    }

    private void OnDestroy() {
        Destroy(owner.gameObject);
    }
    
    bool TargetInDistance() {
        return Vector2.Distance(rb.position, target.transform.position) < activateDistance;
    }

    bool IsGrounded() {
        var startOffset = transform.position - new Vector3(0, col.bounds.extents.y + jumpCheckOffset);
        return Physics2D.Raycast(startOffset, -Vector2.up, 0.05f);
    }

    bool IsGroundedNextWayport() {
        var rayColor = Color.yellow;
        Debug.DrawRay(fallChecker.position, Vector2.down, rayColor);
        return Physics2D.Raycast(fallChecker.position, Vector2.down, 0.5f, LayerMask.GetMask("Obstacle"));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }
    
#endif
    
    public void StartCasting() {
        s = Stage.casting;
    }

    public void EndCasting() {
        s = Stage.patrol;
    }
}
