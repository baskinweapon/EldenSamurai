using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
    
    public float damageValue = 10f;
    public float duration = 1f;
    
    private IResetAfterDamage resetObject;
    public void Set(IResetAfterDamage resetObject) {
        this.resetObject = resetObject;
    }
    
    private void OnEnable() {
        StopAllCoroutines();
        StartCoroutine(EndProcess());
    }

    public void TriggerAbility() {
        StopAllCoroutines();
        StartCoroutine(EndProcess());
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        var health = col.GetComponentInParent<Health>();
        if (health) {
            var owner = GetComponentInParent<Owner>();
            var triggeredOwner = col.GetComponentInParent<Owner>();
            if (owner && triggeredOwner && owner == triggeredOwner) return;
            
            //pass damage
            health.Damage(damageValue);
        }
        
        if (resetObject == null) return;
        StopAllCoroutines();
        resetObject.ResetObj();
    }

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
