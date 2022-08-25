using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Damage {
    public class BaseDamager : MonoBehaviour {
        
        public UnityEvent OnDamaged;
        public UnityEvent OnTriggered;
        public UnityEvent OnEnd;

        [HideInInspector]
        public float damageValue = 10f;
        [HideInInspector]
        public float duration = 1f;
        
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.layer != 10) { // Damager layer
                var health = col.GetComponentInParent<Health>();
                if (health) {
                    var owner = GetComponentInParent<Owner>();
                    var triggeredOwner = col.GetComponentInParent<Owner>();
                    if (owner && triggeredOwner && owner == triggeredOwner) return;
                
                    //pass damage
                    health.Damage(damageValue);
                    OnDamaged?.Invoke();
                } else {
                    OnTriggered?.Invoke();
                }
            }
        }
        
    }
}