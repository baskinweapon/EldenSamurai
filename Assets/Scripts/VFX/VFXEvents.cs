using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace VFX {
    public class VFXEvents : MonoBehaviour {
        public VisualEffect effect;
        
        public float cooldown = 1;
        public float duration = 2;
        
        public UnityEvent OnPlay;
        public UnityEvent OnStop;

        private float time = 0;
        protected virtual void Start() {
            effect.Stop();
        }
        
        protected virtual void Update() {
            if (cooldown == 0) return;
            time += Time.deltaTime;
            if (time >= cooldown && !isPlaying) {
                Play();
            }

            if (!(time >= duration + cooldown)) return;
            Stop();
            time = 0f;
        }
        
        protected bool isPlaying;
        public virtual void Play() {
            effect.Play();
            OnPlay?.Invoke();
            isPlaying = true;
        }

        public virtual void Stop() {
            effect.SendEvent("OnStop");
            effect.Stop();
            OnStop?.Invoke();
            isPlaying = false;
        }
        
    }
}