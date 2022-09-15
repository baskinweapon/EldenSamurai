using System.Collections;
using System.Collections.Generic;
using Damage;
using UnityEngine;
using UnityEngine.VFX;

namespace Effects {
    public class PlayEffects : MonoBehaviour {
        public VisualEffect vfx;
        
        public void Play(float _duration) {
            vfx.Reinit();
            StartCoroutine(WaitEnd(_duration));
        }

        public void Stop() {
            vfx.Stop();
        }
        
        IEnumerator WaitEnd(float duration) {
            yield return new WaitForSeconds(duration);
            vfx.Stop();
        }
    }
}