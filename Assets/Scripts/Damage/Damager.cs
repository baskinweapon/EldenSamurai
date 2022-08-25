using System;
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
                var owner = GetComponentInParent<Owner>();
                var triggeredOwner = col.GetComponentInParent<Owner>();
                if (owner && triggeredOwner && owner == triggeredOwner) return;
                
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

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        resetObject?.ResetObj();
        OnEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
