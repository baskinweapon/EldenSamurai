using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

public class BalisticProjectile : MonoBehaviour {
    [SerializeField] private Collider2D col;
    public GameObject damager;
    public Rigidbody2D rb;
    
    public float speed;
    private Vector3 startPos;
    
    void Awake() {
        startPos = transform.localPosition;
        ResetObj();
    }

    private void OnEnable() {
        StartCast();
    }

    public void StartCast() {
        ResetObj();
        
        damager.SetActive(true);
        
        target = Player.instance.bodyTransform.position;
        isCasting = true;
        applyDynamic = false;
        Invoke(nameof(ApplyDynamic), 0.2f);
    }
    
    private Vector3 target;
    private float height;
    
    private bool isCasting;
    private void FixedUpdate() {
        if (!isCasting) return;
        var isGrounded = IsGroundNearest();
        if (isGrounded && applyDynamic) {
            rb.bodyType = RigidbodyType2D.Dynamic;
            isCasting = false;
        }
        Calculate();
    }

    private bool applyDynamic;
    private void ApplyDynamic() {
        applyDynamic = true;
    }

    private void Calculate() {
        var tr = transform;
        var gun = tr.parent.position;
        
        var speedCoef = Vector2.Distance(gun, target) * speed;
        
        
        var dist = target.x - gun.x;
        var nextX = Mathf.MoveTowards(tr.position.x, target.x,  speedCoef * Time.deltaTime);
        var baseY = Mathf.Lerp(gun.y, target.y, (nextX - gun.x) / dist);
        var h = 2 * (nextX - gun.x) * (nextX - target.x) / (-0.25f * dist * dist);
        
        Vector3 movePosition = new Vector3(nextX, baseY + h, tr.position.z);
        tr.rotation = LookAtTarget(movePosition - tr.position);
        tr.position = movePosition;

        if (Vector2.Distance(tr.position, target) <= 0.02f) {
            
        }
    }

    private bool IsGroundNearest() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Obstacle"));
        Color rayColor;
        var isGround = hit.collider != null;
        
        // Debug
        rayColor = isGround ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down * 0.5f, rayColor);
        
        return isGround;
    }

    private Quaternion LookAtTarget(Vector2 rotation) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void ResetObj() {
        StopAllCoroutines();
        damager.SetActive(false);
        transform.localPosition = startPos;
    }
}
