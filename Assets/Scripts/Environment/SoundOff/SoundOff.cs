using System;
using UnityEngine;

namespace Environment.SoundOff {
    public class SoundOff : MonoBehaviour {
        [SerializeField] private Health health;
        
        
        public void OnEnable() {
            health.OnDeath.AddListener(DropSound);
        }

        private void DropSound() {
            Debug.Log("Stop Effect");
            AudioManager.instance.StopEffect();
        }

        private void OnDisable() {
            health.OnDeath.RemoveListener(DropSound);
        }
    }
}