using System;
using System.Collections;
using Architecture.Interfaces;
using Enemies;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour, IResetAfterDamage {
    [SerializeField] private VisualEffect effect;
    public GameObject damager;
    public Rigidbody2D rigidbody;
    public float speed = 1f;
    
    private void Awake() {
        ResetObj();
        Physics2D.IgnoreCollision(Player.instance.bodyTransform.GetComponent<Collider2D>(), rigidbody.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(rigidbody.GetComponent<Collider2D>(), damager.GetComponent<Collider2D>());
    }
    
    public void StartCast(float duration) {
        ResetObj();
        
        this.duration = duration;
        effect.gameObject.SetActive(true);

        target = Player.instance.bodyTransform.position;
        isCasting = true;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Collision Enter");
        isCasting = false;
        StartCoroutine(SecondStage());
    }

    private Vector3 target;
    private float height;
    
    private bool isCasting;
    private void FixedUpdate() {
        if (!isCasting) return;
        var gun = transform.parent.position;
        var dist = target.x - gun.x;
        var nextX = Mathf.MoveTowards(transform.position.x, target.x, speed * Time.deltaTime);
        var baseY = Mathf.Lerp(gun.y, target.y, (nextX - gun.x) / dist);
        var height = 2 * (nextX - gun.x) * (nextX - target.x) / (-0.25f * dist * dist);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (transform.position == target) {
            Debug.Log("Shot");
        }
    }

    public Quaternion LookAtTarget(Vector2 rotation) {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private float duration;
    IEnumerator SecondStage() {
        yield return new WaitForSeconds(duration - duration / 2.5f);
        
        effect.SendEvent("OnExplode");
        damager.SetActive(true);
    }
    
    public void ResetObj() {
        // effect.SendEvent("OnStop");
        // effect.Reinit();
        // effect.Stop();
        StopAllCoroutines();
        effect.gameObject.SetActive(false);
        damager.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
