using UnityEngine;
using UnityEngine.VFX;

namespace Effects {
    public class PlayEffects : MonoBehaviour {
        public VisualEffect vfx;
        public Damager damager;

        private void OnEnable() {
            damager.OnEnd.AddListener(Stop);
        }

        public void Play() {
            vfx.Reinit();
        }

        public void Stop() {
            vfx.Stop();
        }

        private void OnDisable() {
            damager.OnEnd.RemoveListener(Stop);
        }
    }
}