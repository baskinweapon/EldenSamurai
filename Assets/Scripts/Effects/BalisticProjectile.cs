using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

// ReSharper disable Unity.InefficientPropertyAccess

public class BalisticProjectile : MonoBehaviour {
    [SerializeField] private VisualEffect effect;
    [SerializeField] private Collider2D col;
    public GameObject damager;
    public Rigidbody2D rb;
    private float speed;
    
    private void Awake() {
        ResetObj();
        Physics2D.IgnoreCollision(Player.instance.bodyTransform.GetComponent<Collider2D>(), rb.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), damager.GetComponent<Collider2D>());
    }
    
    public void StartCast(float _duration) {
        ResetObj();
        
        duration = _duration;
        effect.gameObject.SetActive(true);
        
        target = Player.instance.bodyTransform.position;
        speed = Mathf.Abs(target.x - transform.position.x);
        isCasting = true;
    }
    
    private Vector3 target;
    private float height;
    
    private bool isCasting;
    private void FixedUpdate() {
        if (!isCasting) return;
        if (IsGroundNearest()) {
            isCasting = false;
            StartCoroutine(SecondStage());
        }

        var tr = transform;
        var gun = tr.parent.position;
        var dist = target.x - gun.x;
        var nextX = Mathf.MoveTowards(tr.position.x, target.x,  speed * Time.deltaTime);
        var baseY = Mathf.Lerp(gun.y, target.y, (nextX - gun.x) / dist);
        var h = 2 * (nextX - gun.x) * (nextX - target.x) / (-0.25f * dist * dist);

        
        Vector3 movePosition = new Vector3(nextX, baseY + h, tr.position.z);
        tr.rotation = LookAtTarget(movePosition - tr.position);
        tr.position = movePosition;
        
        if (transform.position == target) {
            Debug.Log("Shot");
        }
    }

    private bool IsGroundNearest() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Obstacle"));
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

    private float duration;
    IEnumerator SecondStage() {
        col.enabled = true;
        yield return new WaitForSeconds(duration / 3);
        
        effect.SendEvent("OnExplode");
        damager.SetActive(true);
        Invoke(nameof(ResetObj), duration / 3);
    }
    
    public void ResetObj() {
        StopAllCoroutines();
        effect.gameObject.SetActive(false);
        damager.SetActive(false);
        col.enabled = false;
        transform.localPosition = Vector3.zero;
    }
}
