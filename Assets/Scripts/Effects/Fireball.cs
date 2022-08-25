using System.Collections;
using Architecture.Interfaces;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour, IResetAfterDamage {
    [SerializeField] private VisualEffect effect;
    [SerializeField] private GameObject damager;
    public Rigidbody2D rigidbody;

    private void Awake() {
        effect.SendEvent("OnStop");
        effect.Stop();
        Physics2D.IgnoreCollision(Player.instance.bodyTransform.GetComponent<Collider2D>(), rigidbody.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(rigidbody.GetComponent<Collider2D>(), damager.GetComponent<Collider2D>());
    }

    public void StartCast(float duration) {
        
        ResetObj();
        
        effect.Reinit();
        this.duration = duration;
        var fromTo = Player.instance.bodyTransform.position - transform.position;
        fromTo.y += 3f;
        var angle = Vector2.Angle(Player.instance.bodyTransform.position + Vector3.up * 3f, transform.position);
        Debug.Log(angle);
        rigidbody.velocity = fromTo * 3f;
        StartCoroutine(SecondStage());
    }

    private float duration;
    IEnumerator SecondStage() {
        yield return new WaitForSeconds(duration - duration / 3f);
        
        effect.SendEvent("OnExplode");
        damager.SetActive(true);
    }
    
    public void ResetObj() {
        effect.SendEvent("OnStop");
        effect.Stop();
        damager.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
