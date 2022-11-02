using Scriptble;
using UnityEngine;

namespace Enemies {
    public class Enemy : MonoBehaviour {
        public Owner owner;
        public EnemyAI enemyAI;
        public Animator animator;
        public Rigidbody2D rb;
        public Health health;

        public AudioSource audioSource;
        public SoundCharacterAsset soundAsset;
        
        [Header("Exp for Player")]
        public float expirience = 10f;

        protected virtual void OnEnable() {
            health.OnDeath.AddListener(Death);
        }

        private void Death() {
            Player.instance.expirience.ExirienceUp(expirience);
        }

        private void OnDisable() {
            health.OnDeath.RemoveListener(Death);
        }


        #region Audio

        public void WalkAudio() {
            audioSource.PlayOneShot(soundAsset.walk);
        }
        
        public void HurtAudio() {
            audioSource.PlayOneShot(soundAsset.hurt);
        }

        public void DeathAudio() {
            audioSource.PlayOneShot(soundAsset.die);
        }

        #endregion
    }
}