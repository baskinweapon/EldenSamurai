using System;
using Architecture.Interfaces;
using UnityEngine;

namespace Abilities {
    public class BaseAbilityCooldown: MonoBehaviour {
        [Header("Cast object need ICastAbility")]
        public GameObject castObject;
        
        [SerializeField, Header("Parent Ability")]
        protected Transform parent;
        
        [SerializeField, Header("Main Ability")] 
        protected Ability ability;

        [SerializeField, Header("abilit Game Object")]
        private GameObject abilityHolder;
        
        [SerializeField]
        protected AudioSource abilitySource;
        
        protected float coolDownDuration;
        protected float nextReadyTime;
        protected float coolDownTimeLeft;

        private Damager damager;
        private ICastAbility castAbility;
        
        private void Start() {
            castAbility = castObject.GetComponent<ICastAbility>();
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

        private bool isCasting;
        private float time;
        private void LateUpdate() {
            if (isCasting) {
                time += Time.deltaTime;
                if (time >= ability.castTime) {
                    castAbility.EndCasting();
                    time = 0;
                }
            }
        }

        public void Initiallize(Ability ability, GameObject _abilityHolder) {
            this.ability = ability;
            coolDownDuration = ability.baseCooldown;
            damager = ability.Initiliaze(_abilityHolder, parent);
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
            
            castAbility.StartCasting();
            
            abilitySource.clip = ability.sound;
            abilitySource.Play();
            ability.TriggerAbility(damager);
        }
    }
    
}