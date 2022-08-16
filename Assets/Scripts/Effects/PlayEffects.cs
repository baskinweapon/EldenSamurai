using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Effects {
    public class PlayEffects : MonoBehaviour {
        public ParticleSystem[] particleSystems;
        public VisualEffect vfx;
        
        public void Play() {
            vfx.Play();
            // foreach (var par in particleSystems) {
            //     par.Play();
            // }
        }

        public void Stop() {
            vfx.Stop();
            // foreach (var par in particleSystems) {
            //     par.Stop();
            // }
        }
    }
}