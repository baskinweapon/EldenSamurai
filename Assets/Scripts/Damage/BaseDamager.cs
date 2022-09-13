using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Damage {
    [RequireComponent(typeof(Collider2D))]
    public class BaseDamager : MonoBehaviour {
        
        public UnityEvent OnDamaged;
        public UnityEvent OnTriggered;
        public UnityEvent OnEnd;

        [HideInInspector]
        public float damageValue = 10f;
        [HideInInspector]
        public float duration = 1f;
        
        protected virtual void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.layer != 10) { // Damager layer
                var health = col.GetComponentInParent<Health>();
                if (health) {
                    var owner = GetComponentInParent<Owner>();
                    var triggeredOwner = col.GetComponentInParent<Owner>();
                    if (CheckOwners(col)) return;
                    if (damageValue == 0) return;
                    
                    //pass damage
                    health.Damage(damageValue);
                    OnDamaged?.Invoke();
                } else {
                    OnTriggered?.Invoke();
                }
            }
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
    }
}