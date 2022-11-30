using System;
using UnityEngine;
using UnityEngine.Events;

namespace Damage {
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour {
        
        public UnityEvent OnDamaged;
        public UnityEvent OnTriggered;
        
        [HideInInspector]
        public float damageValue = 10f;
        [HideInInspector]
        public float duration = 1f;


        private void OnEnable() {
            OnTriggerEnter2D(lastCollider);
        }

        private Collider2D lastCollider;
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.layer == 10) return; // Damager layer
            var health = col.GetComponentInParent<Health>();
            if (health) {
                if (CompareOwners(col)) return;
                if (damageValue == 0) return;
                if (col.transform == transform.parent) return;
                    
                //pass damage
                health.Damage(damageValue, col);
                lastCollider = col;
                OnDamaged?.Invoke();
            } else {
                OnTriggered?.Invoke();
            }
        }
        
        private bool CompareOwners(Collider2D col) {
            var damager = transform.parent;
            if (damager == null) return false;
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
            if (victim == null) return false;
            var victimOwner = victim.GetComponent<Owner>();
            
            if (owner != null && victimOwner != null && owner.gameObject.layer == victimOwner.gameObject.layer)
                return true;
            
            if (victimOwner != null) return owner && victimOwner && owner == victimOwner;
            while (victimOwner == null) {
                victim = victim.parent;
                if (!victim) break;
                victimOwner = victim.GetComponent<Owner>();
                if (victimOwner != null) break;
            }
            
            return owner && victimOwner && owner == victimOwner;
        }
    }
}