using System.Collections;
using UnityEngine;

namespace VFX {
    public class FireballVFXEvent : VFXEvents {
        
        
        protected override void Update() {
            base.Update();
        }

        public override void Play() {
            base.Play();
            
        }


        public override void Stop() {
            StopAllCoroutines();
            base.Stop();
        }
    }
}