using System.Collections;
using Architecture.Interfaces;
using Damage;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : BaseDamager {
    [Header("no ended")]
    public bool permanent;
    
    private IResetAfterDamage resetObject;
    public void Set(IResetAfterDamage resetObject) {
        this.resetObject = resetObject;
    }
    
    private void OnEnable() {
        if (permanent) return;
        StopAllCoroutines();
        StartCoroutine(EndProcess());
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer != 10) { // Damager layer
            var health = col.GetComponentInParent<Health>();
            if (health) {
                if (CheckOwners(col)) return;
                
                //pass damage
                health.Damage(damageValue);
            }
        }
        
        if (col.transform == this.transform.parent) return;
        
        OnDamaged?.Invoke();
        if (resetObject == null) return;
        StopAllCoroutines();
        resetObject.ResetObj();
    }

    private bool CheckOwners(Collider2D col) {
        var damager = transform.parent;
        var owner = damager.GetComponent<Owner>();
        if (owner == null) {
            while (owner == null) {
                damager = damager.parent;
                if (!damager) break;
                owner = damager.GetComponent<Owner>();
                if (owner != null) break;
            }
        }
        var victim = col.transform.parent;
        var victimOwner = victim.GetComponent<Owner>();
        if (victimOwner == null) {
            while (victimOwner == null) {
                victim = victim.parent;
                if (!victim) break;
                victimOwner = victim.GetComponent<Owner>();
                if (victimOwner != null) break;
            }
        }
        if (owner && victimOwner && owner == victimOwner) return true;
        return false;
    }

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        resetObject?.ResetObj();
        OnEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
