using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
    public float damageValue = 10f;
    public float duration = 1f;
    
    private void OnEnable() {
        StartCoroutine(EndProcess());
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        var health = col.GetComponentInParent<Health>();
        if (!health) return;
        var owner = GetComponentInParent<Owner>();
        var triggeredOwner = col.GetComponentInParent<Owner>();
        if (owner && triggeredOwner && owner == triggeredOwner) return;
        
        health.Damage(damageValue);
    }

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
