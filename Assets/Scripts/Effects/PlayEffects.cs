using UnityEngine;

namespace Effects {
    public class PlayEffects : MonoBehaviour {
        public ParticleSystem[] particleSystems;

        public void Play() {
            foreach (var par in particleSystems) {
                par.Play();
            }
        }

        public void Stop() {
            foreach (var par in particleSystems) {
                par.Stop();
            }
        }
    }
}