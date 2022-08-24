using System;
using System.Collections;
using Architecture.Interfaces;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
    [Header("no ended")]
    public bool permanent;
    
    public Action OnDamaged;
    public Action OnEnd;

    [HideInInspector]
    public float damageValue = 10f;
    [HideInInspector]
    public float duration = 1f;
    
    private IResetAfterDamage resetObject;
    public void Set(IResetAfterDamage resetObject) {
        this.resetObject = resetObject;
    }
    
    private void OnEnable() {
        if (permanent) return;
        StopAllCoroutines();
        StartCoroutine(EndProcess());
    }

    public void TriggerAbility() {
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
