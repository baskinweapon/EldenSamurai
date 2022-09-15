using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour {
    [SerializeField] private VisualEffect effect;
    public GameObject damager;
    public Rigidbody2D rb;
    public float speed = 1f;
    
    private void Awake() {
        ResetObj();
        Physics2D.IgnoreCollision(Player.instance.bodyTransform.GetComponent<Collider2D>(), rb.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), damager.GetComponent<Collider2D>());
    }

    private Vector2 startPos;
    public void StartCast(float _duration) {
        ResetObj();
        
        duration = _duration;
        effect.gameObject.SetActive(true);

        target = Player.instance.bodyTransform.position;
        startPos = transform.position;
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
        var _speed = Mathf.Abs(target.x - startPos.x);
        var gun = transform.parent.position;
        var dist = target.x - gun.x;
        var nextX = Mathf.MoveTowards(transform.position.x, target.x,  _speed * Time.deltaTime);
        var baseY = Mathf.Lerp(gun.y, target.y, (nextX - gun.x) / dist);
        var height = 2 * (nextX - gun.x) * (nextX - target.x) / (-0.25f * dist * dist);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (transform.position == target) {
            Debug.Log("Shot");
        }
    }

    private bool IsGroundNearest() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Obstacle"));
        Color rayColor;
        var isGround = hit.collider != null;
        
        // Debug
        if (isGround) {
            rayColor = Color.green;
        } else rayColor = Color.red;
        Debug.DrawRay(transform.position, Vector2.down * 0.5f, rayColor);
        
        return isGround;
    }

    private Quaternion LookAtTarget(Vector2 rotation) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private float duration;
    IEnumerator SecondStage() {
        yield return new WaitForSeconds(duration / 3);
        Debug.Log("Second Stage");
        
        effect.SendEvent("OnExplode");
        damager.SetActive(true);
        Invoke(nameof(ResetObj), duration / 3);
    }
    
    public void ResetObj() {
        StopAllCoroutines();
        effect.gameObject.SetActive(false);
        damager.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
