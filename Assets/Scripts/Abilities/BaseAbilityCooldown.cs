using UnityEngine;

namespace Abilities {
    public class BaseAbilityCooldown: MonoBehaviour {
        [SerializeField, Header("Main Owner ability")]
        protected Transform owner;
        
        [SerializeField, Header("Main Ability")] 
        protected Ability ability;

        [SerializeField, Header("abilit Game Object")]
        private GameObject abilityHolder;
        
        [SerializeField]
        protected AudioSource abilitySource;
        
        protected float coolDownDuration;
        protected float nextReadyTime;
        protected float coolDownTimeLeft;

        private Damager _damager;
        
        private void Start() {
            Initiallize(ability, abilityHolder);
        }
        
        protected void Update() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (coolDownComplete) {
                AbilityReady();
            } else {
                CoolDown();
            }
        }
        
        public void Initiallize(Ability ability, GameObject _abilityHolder) {
            this.ability = ability;
            coolDownDuration = ability.baseCooldown;
            _damager = ability.Initiliaze(_abilityHolder, owner);
            AbilityReady();
        }
        
        protected virtual void AbilityReady() {
            
        }
        
        private void CoolDown() {
            coolDownTimeLeft -= Time.deltaTime;
        }

        public bool CooldownComplete() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            return coolDownComplete;
        }
        
        public void Triggered() {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (!coolDownComplete) return;
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;
            
            abilitySource.clip = ability.sound;
            abilitySource.Play();
            ability.TriggerAbility(_damager);
        }
    }
    
}